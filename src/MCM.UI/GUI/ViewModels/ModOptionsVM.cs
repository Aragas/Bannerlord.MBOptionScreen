using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ModuleManager;

using BUTR.DependencyInjection;

using ComparerExtensions;

using MCM.Abstractions;
using MCM.UI.Dropdown;
using MCM.UI.Extensions;
using MCM.UI.Patches;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
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
        private string _cancelButtonText = string.Empty;
        private string _doneButtonText = string.Empty;
        private string _modsText = string.Empty;
        private SettingsVM? _selectedMod;
        private MBBindingList<SettingsVM> _modSettingsList = new();
        private string _hintText = string.Empty;
        private string _searchText = string.Empty;

        [DataSourceProperty]
        public string Name
        {
            get => _titleLabel;
            set
            {
                _titleLabel = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        [DataSourceProperty]
        public string DoneButtonText
        {
            get => _doneButtonText;
            set
            {
                _doneButtonText = value;
                OnPropertyChanged(nameof(DoneButtonText));
            }
        }
        [DataSourceProperty]
        public string CancelButtonText
        {
            get => _cancelButtonText;
            set
            {
                _cancelButtonText = value;
                OnPropertyChanged(nameof(CancelButtonText));
            }
        }
        [DataSourceProperty]
        public string ModsText
        {
            get => _modsText;
            set
            {
                _modsText = value;
                OnPropertyChanged(nameof(ModsText));
            }
        }
        [DataSourceProperty]
        public MBBindingList<SettingsVM> ModSettingsList
        {
            get => _modSettingsList;
            set
            {
                if (_modSettingsList != value)
                {
                    _modSettingsList = value;
                    OnPropertyChanged(nameof(ModSettingsList));
                }
            }
        }
        [DataSourceProperty]
        public SettingsVM? SelectedMod
        {
            get => _selectedMod;
            set
            {
                if (_selectedMod != value)
                {
                    _selectedMod?.PresetsSelector?.SetOnChangeAction(null);
                    _selectedMod = value;

                    OnPropertyChanged(nameof(SelectedMod));
                    OnPropertyChanged(nameof(SelectedDisplayName));
                    OnPropertyChanged(nameof(SomethingSelected));

                    if (_selectedMod?.PresetsSelector is not null)
                    {
                        PresetsSelector.SetOnChangeAction(null);
                        OnPropertyChanged(nameof(PresetsSelector));
                        PresetsSelector.ItemList = _selectedMod.PresetsSelector.ItemList;
                        PresetsSelector.SelectedIndex = _selectedMod.PresetsSelector.SelectedIndex;
                        PresetsSelector.HasSingleItem = _selectedMod.PresetsSelector.HasSingleItem;
                        _selectedMod.PresetsSelector.SetOnChangeAction(OnModPresetsSelectorChange);
                        PresetsSelector.SetOnChangeAction(OnPresetsSelectorChange);

                        OnPropertyChanged(nameof(IsPresetsSelectorVisible));
                    }
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
                if (_hintText != value)
                {
                    _hintText = value;
                    OnPropertyChanged(nameof(HintText));
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
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    if (SelectedMod?.SettingPropertyGroups.Count > 0)
                    {
                        foreach (var group in SelectedMod.SettingPropertyGroups)
                            group.NotifySearchChanged();
                    }
                }
            }
        }
        [DataSourceProperty]
        public MCMSelectorVM<DropdownSelectorItemVM<PresetKey>> PresetsSelector { get; } = new(Enumerable.Empty<PresetKey>(), -1, null);
        [DataSourceProperty]
        public bool IsPresetsSelectorVisible => SelectedMod is not null;

        public ModOptionsVM()
        {
            _logger = GenericServiceProvider.GetService<ILogger<ModOptionsVM>>() ?? NullLogger<ModOptionsVM>.Instance;

            Name = new TextObject("{=ModOptionsVM_Name}Mod Options").ToString();
            DoneButtonText = new TextObject("{=WiNRdfsm}Done").ToString();
            CancelButtonText = new TextObject("{=3CpNUnVl}Cancel").ToString();
            ModsText = new TextObject("{=ModOptionsPageView_Mods}Mods").ToString();
            SearchText = string.Empty;

            InitializeModSettings();
            RefreshValues();
        }

        private void InitializeModSettings()
        {
            ModSettingsList = new MBBindingList<SettingsVM>();
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

                        var settingsVM = BaseSettingsProvider.Instance!.SettingsDefinitions
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
                                        Colors.Red);
                                    return null;
                                }
                            })
                            .Where(vm => vm is not null);

                        foreach (var viewModel in settingsVM)
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
                        Colors.Red);
                }
            }, SynchronizationContext.Current);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            foreach (var viewModel in ModSettingsList)
                viewModel.RefreshValues();

            OnPropertyChanged(nameof(SelectedMod));

            if (SelectedMod is not null)
            {
                PresetsSelector.SetOnChangeAction(null);
                PresetsSelector.SelectedIndex = SelectedMod?.PresetsSelector?.SelectedIndex ?? -1;
                PresetsSelector.SetOnChangeAction(OnPresetsSelectorChange);
            }
        }

        private void OnPresetsSelectorChange(MCMSelectorVM<DropdownSelectorItemVM<PresetKey>> selector)
        {
            var presetKey = selector.SelectedItem.OriginalItem;
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
                    SelectedMod!.ChangePreset(presetKey.Id);
                    var selectedMod = SelectedMod;
                    ExecuteSelect(null);
                    ExecuteSelect(selectedMod);
                },
                () =>
                {
                    PresetsSelector.SetOnChangeAction(null);
                    PresetsSelector.SelectedIndex = SelectedMod?.PresetsSelector?.SelectedIndex ?? -1;
                    PresetsSelector.SetOnChangeAction(OnPresetsSelectorChange);
                }));
        }
        private void OnModPresetsSelectorChange(MCMSelectorVM<DropdownSelectorItemVM<PresetKey>> selector)
        {
            PresetsSelector.SetOnChangeAction(null);
            PresetsSelector.SelectedIndex = selector.SelectedIndex;
            PresetsSelector.SetOnChangeAction(OnPresetsSelectorChange);
        }

        public void ExecuteClose()
        {
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

            base.OnFinalize();
        }
    }
}
