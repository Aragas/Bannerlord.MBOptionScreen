using Bannerlord.ModuleManager;

using BUTR.DependencyInjection;

using ComparerExtensions;

using MCM.Abstractions;
using MCM.UI.Dropdown;
using MCM.UI.Extensions;
using MCM.UI.Patches;
using MCM.UI.Utils;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class ModOptionsVM : ViewModel
    {
        private readonly ILogger<ModOptionsVM> _logger;

        private string _titleLabel = string.Empty;
        private string _doneButtonText = string.Empty;
        private string _cancelButtonText = string.Empty;
        private string _modsText = string.Empty;
        private string _selectedDisplayName = string.Empty;
        private string _hintText = string.Empty;
        private SettingsVM? _selectedMod;
        private string _searchText = string.Empty;

        [DataSourceProperty]
        public string Name { get => _titleLabel; set => SetField(ref _titleLabel, value, nameof(Name)); }
        [DataSourceProperty]
        public string DoneButtonText { get => _doneButtonText; set => SetField(ref _doneButtonText, value, nameof(DoneButtonText)); }
        [DataSourceProperty]
        public string CancelButtonText { get => _cancelButtonText; set => SetField(ref _cancelButtonText, value, nameof(CancelButtonText)); }
        [DataSourceProperty]
        public string ModsText { get => _modsText; set => SetField(ref _modsText, value, nameof(ModsText)); }
        [DataSourceProperty]
        public MBBindingList<SettingsVM> ModSettingsList { get; } = new();
        [DataSourceProperty]
        public SettingsVM? SelectedMod
        {
            get => _selectedMod;
            set
            {
                if (_selectedMod?.PresetsSelector is { } oldModPresetsSelector)
                {
                    oldModPresetsSelector.PropertyChanged -= OnModPresetsSelectorChange;
                }

                if (SetField(ref _selectedMod, value, nameof(SelectedMod)))
                {
                    OnPropertyChanged(nameof(SelectedDisplayName));
                    OnPropertyChanged(nameof(SomethingSelected));

                    DoPresetsSelectorCopyWithoutEvents(() =>
                    {
                        if (SelectedMod?.PresetsSelector is { } modPresetsSelector)
                        {
                            modPresetsSelector.PropertyChanged += OnModPresetsSelectorChange;
                            PresetsSelectorCopy.Refresh(SelectedMod.PresetsSelector.ItemList.Select(x => x.OriginalItem), SelectedMod.PresetsSelector.SelectedIndex, null);
                        }
                        else
                        {
                            PresetsSelectorCopy.Refresh(Enumerable.Empty<PresetKey>(), -1, null);
                        }
                    });
                    OnPropertyChanged(nameof(IsPresetsSelectorVisible));
                }
            }
        }
        [DataSourceProperty]
        public string SelectedDisplayName => SelectedMod is null ? new TextObject("{=ModOptionsVM_NotSpecified}Mod Name not Specified.").ToString() : SelectedMod.DisplayName;
        [DataSourceProperty]
        public bool SomethingSelected => SelectedMod is not null;
        [DataSourceProperty]
        public string HintText
        {
            get => _hintText;
            set
            {
                if (SetField(ref _hintText, value, nameof(HintText)))
                {
                    OnPropertyChanged(nameof(IsHintVisible));
                }
            }
        }
        [DataSourceProperty]
        public bool IsHintVisible => !string.IsNullOrWhiteSpace(HintText);
        [DataSourceProperty]
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetField(ref _searchText, value, nameof(SearchText)) && SelectedMod?.SettingPropertyGroups.Count > 0)
                {
                    foreach (var group in SelectedMod.SettingPropertyGroups)
                        group.NotifySearchChanged();
                }
            }
        }
        [DataSourceProperty]
        public MCMSelectorVM<MCMSelectorItemVM<PresetKey>> PresetsSelectorCopy { get; } = new(Enumerable.Empty<PresetKey>(), -1, null);
        [DataSourceProperty]
        public bool IsPresetsSelectorVisible => SelectedMod is not null;
        /// <summary>
        /// We have a strange bug and I'm not sure if this is the lack of my skills
        /// Selecting via mouse in Gameplay section the language triggers preset change
        /// Somehow ~palpatine returned~ the game triggers input change on my non visible VM
        /// </summary>
        [DataSourceProperty]
        public bool IsDisabled { get; set; }

        public ModOptionsVM()
        {
            _logger = GenericServiceProvider.GetService<ILogger<ModOptionsVM>>() ?? NullLogger<ModOptionsVM>.Instance;

            SearchText = string.Empty;

            InitializeModSettings();
            RefreshValues();
        }

        private void InitializeModSettings()
        {
            // Execute code in a non UI-Thread to avoid blokcking
            _ = Task.Factory.StartNew(syncContext => // Build the options in a separate context if possible
            {
                try
                {
                    if (syncContext is SynchronizationContext uiContext)
                    {
                        if (SynchronizationContext.Current == uiContext)
                        {
                            _logger.LogWarning("SynchronizationContext.Current is the UI SynchronizationContext");
                        }

                        var settingsVM = BaseSettingsProvider.Instance?.SettingsDefinitions
                            .Parallel()
                            .Select(s =>
                            {
                                try
                                {
                                    return new SettingsVM(s, this);
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e, "Error while creating a ViewModel for settings {Id}", s.SettingsId);
                                    InformationManager.DisplayMessage(new InformationMessage(
                                        new TextObject($"{{=HNduGf7H5a}}There was an error while parsing settings from '{s.SettingsId}'! Please contact the MCM developers and the mod developer!").ToString(),
                                        Colors.Red));
                                    return null;
                                }
                            })
                            .Where(vm => vm is not null);

                        foreach (var viewModel in settingsVM ?? Enumerable.Empty<SettingsVM>())
                        {
                            uiContext.Send(state =>
                            {
                                if (state is SettingsVM vm)
                                {
                                    vm.AddSelectCommand(ExecuteSelect);
                                    ModSettingsList.Add(vm);
                                    vm.RefreshValues();
                                }
                            }, viewModel);
                        }

                        // Yea, I imported a whole library that converts LINQ style order to IComparer
                        // because I wasn't able to recreate the logic via IComparer. TODO: Fix that
                        ModSettingsList.Sort(KeyComparer<SettingsVM>
                            .OrderByDescending(x => x.SettingsDefinition.SettingsId.StartsWith("MCM") ||
                                                    x.SettingsDefinition.SettingsId.StartsWith("Testing") ||
                                                    x.SettingsDefinition.SettingsId.StartsWith("ModLib"))
                            .ThenByDescending(x => x.DisplayName, new AlphanumComparatorFast()));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while creating ViewModels for the settings");
                    InformationManager.DisplayMessage(new InformationMessage(
                        new TextObject("{=JLKaTyJcyu}There was a major error while building the settings list! Please contact the MCM developers!").ToString(),
                        Colors.Red));
                }
            }, SynchronizationContext.Current);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            Name = new TextObject("{=ModOptionsVM_Name}Mod Options").ToString();
            DoneButtonText = new TextObject("{=WiNRdfsm}Done").ToString();
            CancelButtonText = new TextObject("{=3CpNUnVl}Cancel").ToString();
            ModsText = new TextObject("{=ModOptionsPageView_Mods}Mods").ToString();

            PresetsSelectorCopy.RefreshValues();

            foreach (var viewModel in ModSettingsList)
                viewModel.RefreshValues();
        }

        private void OnPresetsSelectorChange(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (sender is not MCMSelectorVM<MCMSelectorItemVM<PresetKey>> selector) return;
            if (propertyChangedEventArgs.PropertyName != "SelectedIndex") return;

            if (IsDisabled)
            {
                // GaultletUI is resetting SelectorVM's Index after RefreshLayout, restore the index manually
                if (SelectedMod?.PresetsSelector is not null && selector.SelectedIndex == -1)
                    DoPresetsSelectorCopyWithoutEvents(() => PresetsSelectorCopy.SelectedIndex = SelectedMod.PresetsSelector.SelectedIndex);
                return;
            }

            if (selector.SelectedItem is null || selector.SelectedIndex == -1) return;
            if (selector.ItemList.Count < selector.SelectedIndex) return;

            var presetKey = selector.ItemList[selector.SelectedIndex].OriginalItem;
            InformationManager.ShowInquiry(new InquiryData(
                new TextObject("{=ModOptionsVM_ChangeToPreset}Change to preset '{PRESET}'", new()
                {
                    { "PRESET", presetKey.Name }
                }).ToString(),
                new TextObject("{=ModOptionsVM_Discard}Are you sure you wish to discard the current settings for {NAME} to '{ITEM}'?", new()
                {
                    { "NAME", SelectedMod?.DisplayName },
                    { "ITEM", presetKey.Name }
                }).ToString(),
                true, true, new TextObject("{=aeouhelq}Yes").ToString(),
                new TextObject("{=8OkPHu4f}No").ToString(),
                () =>
                {
                    if (SelectedMod is not null)
                    {
                        SelectedMod.ChangePreset(presetKey.Id);
                        var selectedMod = SelectedMod;
                        ExecuteSelect(null);
                        ExecuteSelect(selectedMod);
                    }
                },
                () =>
                {
                    DoPresetsSelectorCopyWithoutEvents(() => PresetsSelectorCopy.SelectedIndex = SelectedMod?.PresetsSelector?.SelectedIndex ?? -1);
                }));
        }
        private void OnModPresetsSelectorChange(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (sender is not MCMSelectorVM<MCMSelectorItemVM<PresetKey>> selector) return;
            if (propertyChangedEventArgs.PropertyName != "SelectedIndex") return;

            DoPresetsSelectorCopyWithoutEvents(() => PresetsSelectorCopy.SelectedIndex = selector.SelectedIndex);
        }

        public void ExecuteClose()
        {
            if (IsDisabled) return;

            foreach (var viewModel in ModSettingsList)
            {
                viewModel.URS.UndoAll();
                viewModel.URS.ClearStack();
            }
        }

        public bool ExecuteCancel() => ExecuteCancelInternal(true);

        public void StartTyping() => OptionsVMPatch.BlockSwitch = true;
        public void StopTyping() => OptionsVMPatch.BlockSwitch = false;

        public bool ExecuteCancelInternal(bool popScreen, Action? onClose = null)
        {
            if (IsDisabled) return false;

            OnFinalize();
            if (popScreen) ScreenManager.PopScreen();
            else onClose?.Invoke();
            foreach (var viewModel in ModSettingsList)
            {
                viewModel.URS.UndoAll();
                viewModel.URS.ClearStack();
            }
            return true;
        }

        public void ExecuteDone() => ExecuteDoneInternal(true);
        public void ExecuteDoneInternal(bool popScreen, Action? onClose = null)
        {
            if (IsDisabled) return;

            if (!ModSettingsList.Any(x => x.URS.ChangesMade))
            {
                OnFinalize();
                if (popScreen) ScreenManager.PopScreen();
                else onClose?.Invoke();
                return;
            }

            // Save the changes to file.
            var changedModSettings = ModSettingsList.Where(x => x.URS.ChangesMade).ToList();

            var requireRestart = changedModSettings.Any(x => x.RestartRequired());
            if (requireRestart)
            {
                InformationManager.ShowInquiry(new InquiryData(new TextObject("{=ModOptionsVM_RestartTitle}Game Needs to Restart").ToString(),
                    new TextObject("{=ModOptionsVM_RestartDesc}The game needs to be restarted to apply mod settings changes. Do you want to close the game now?").ToString(),
                    true, true, new TextObject("{=aeouhelq}Yes").ToString(), new TextObject("{=3CpNUnVl}Cancel").ToString(),
                    () =>
                    {
                        foreach (var changedModSetting in changedModSettings)
                        {
                            changedModSetting.SaveSettings();
                            changedModSetting.URS.ClearStack();
                        }

                        OnFinalize();
                        onClose?.Invoke();
                        Utilities.QuitGame();
                    }, () => { }));
            }
            else
            {
                foreach (var changedModSetting in changedModSettings)
                {
                    changedModSetting.SaveSettings();
                    changedModSetting.URS.ClearStack();
                }

                OnFinalize();
                if (popScreen) ScreenManager.PopScreen();
                else onClose?.Invoke();
            }
        }

        public void ExecuteSelect(SettingsVM? viewModel)
        {
            if (IsDisabled) return;

            if (SelectedMod != viewModel)
            {
                if (SelectedMod is not null)
                    SelectedMod.IsSelected = false;

                SelectedMod = viewModel;

                if (SelectedMod is not null)
                    SelectedMod.IsSelected = true;
            }
        }

        public override void OnFinalize()
        {
            foreach (var modSettings in ModSettingsList)
                modSettings.OnFinalize();

            SettingPropertyDefinitionCache.Clear();

            base.OnFinalize();
        }

        private void DoPresetsSelectorCopyWithoutEvents(Action action)
        {
            PresetsSelectorCopy.PropertyChanged -= OnPresetsSelectorChange;
            action();
            PresetsSelectorCopy.PropertyChanged += OnPresetsSelectorChange;
        }
    }
}