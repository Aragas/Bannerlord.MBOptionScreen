using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Helpers;

using BUTR.DependencyInjection;

using ComparerExtensions;

using MCM.Abstractions.Settings.Providers;
using MCM.UI.Extensions;
using MCM.UI.Utils;
using MCM.Utils;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

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
        public bool ChangesMade => ModSettingsList.Any(x => x.URS.ChangesMade);
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
        public string SelectedDisplayName => SelectedMod is null ? TextObjectHelper.Create("{=ModOptionsVM_NotSpecified}Mod Name not Specified.")?.ToString() ?? string.Empty : SelectedMod.DisplayName;
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
        public SelectorVM<SelectorItemVM> PresetsSelector { get; } = new(Enumerable.Empty<string>(), -1, null);
        [DataSourceProperty]
        public bool IsPresetsSelectorVisible => SelectedMod is not null;

        public ModOptionsVM()
        {
            _logger = GenericServiceProvider.GetService<ILogger<ModOptionsVM>>() ?? NullLogger<ModOptionsVM>.Instance;

            Name = TextObjectHelper.Create("{=ModOptionsVM_Name}Mod Options")?.ToString() ?? string.Empty;
            DoneButtonText = TextObjectHelper.Create("{=WiNRdfsm}Done")?.ToString() ?? string.Empty;
            CancelButtonText = TextObjectHelper.Create("{=3CpNUnVl}Cancel")?.ToString() ?? string.Empty;
            ModsText = TextObjectHelper.Create("{=ModOptionsPageView_Mods}Mods")?.ToString() ?? string.Empty;
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

                        var settingsVM = BaseSettingsProvider.Instance!.CreateModSettingsDefinitions
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
                                        TextObjectHelper.Create($"{{=HNduGf7H5a}}There was an error while parsing settings from '{s.SettingsId}'! Please contact the MCM developers and the mod developer!")?.ToString(),
                                        Colors.Red));
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
                        TextObjectHelper.Create("{=JLKaTyJcyu}There was a major error while building the settings list! Please contact the MCM developers!")?.ToString(),
                        Colors.Red));
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

        private void OnPresetsSelectorChange(SelectorVM<SelectorItemVM> selector)
        {
            var data = InquiryDataUtils.Create(
                TextObjectHelper.Create("{=ModOptionsVM_ChangeToPreset}Change to preset '{PRESET}'", new Dictionary<string, TextObject?>
                {
                    {"PRESET", TextObjectHelper.Create(selector.SelectedItem.StringItem)}
                })?.ToString(),
                TextObjectHelper.Create("{=ModOptionsVM_Discard}Are you sure you wish to discard the current settings for {NAME} to '{ITEM}'?", new Dictionary<string, TextObject?>
                {
                    {"NAME", TextObjectHelper.Create(SelectedMod!.DisplayName)},
                    {"ITEM", TextObjectHelper.Create(selector.SelectedItem.StringItem)}
                })?.ToString(),
                true, true, TextObjectHelper.Create("{=aeouhelq}Yes")?.ToString(),
                TextObjectHelper.Create("{=8OkPHu4f}No")?.ToString(),
                () =>
                {
                    SelectedMod!.ChangePreset(PresetsSelector.SelectedItem.StringItem);
                    var selectedMod = SelectedMod;
                    ExecuteSelect(null);
                    ExecuteSelect(selectedMod);
                },
                () =>
                {
                    PresetsSelector.SetOnChangeAction(null);
                    PresetsSelector.SelectedIndex = SelectedMod.PresetsSelector?.SelectedIndex ?? -1;
                    PresetsSelector.SetOnChangeAction(OnPresetsSelectorChange);
                });
            InformationManagerUtils.ShowInquiry(data);
        }
        private void OnModPresetsSelectorChange(SelectorVM<SelectorItemVM> selector)
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
                var data = InquiryDataUtils.Create(TextObjectHelper.Create("{=ModOptionsVM_RestartTitle}Game Needs to Restart")?.ToString(),
                    TextObjectHelper.Create("{=ModOptionsVM_RestartDesc}The game needs to be restarted to apply mod settings changes. Do you want to close the game now?")?.ToString(),
                    true, true, TextObjectHelper.Create("{=aeouhelq}Yes")?.ToString(), TextObjectHelper.Create("{=3CpNUnVl}Cancel")?.ToString(),
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
                    }, () => { });
                InformationManagerUtils.ShowInquiry(data);
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