using MBOptionScreen.Actions;
using MBOptionScreen.ExtensionMethods;
using MBOptionScreen.Settings;

using System.Linq;

using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MBOptionScreen.GUI.v1a.ViewModels
{
    public class ModSettingsScreenVM : ViewModel
    {
        private string _titleLabel;
        private string _cancelButtonText;
        private string _doneButtonText;
        private ModSettingsVM _selectedMod;
        private MBBindingList<ModSettingsVM> _modSettingsList = new MBBindingList<ModSettingsVM>();
        private string _hintText;
        private string _searchText = "";

        [DataSourceProperty]
        public string TitleLabel
        {
            get => _titleLabel;
            set
            {
                _titleLabel = value;
                OnPropertyChanged(nameof(TitleLabel));
            }
        }
        [DataSourceProperty]
        public bool ChangesMade => ModSettingsList.Any((x) => x.URS.ChangesMade());

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
            get => _cancelButtonText; set
            {
                _cancelButtonText = value;
                OnPropertyChanged(nameof(CancelButtonText));
            }
        }
        [DataSourceProperty]
        public MBBindingList<ModSettingsVM> ModSettingsList
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
        public ModSettingsVM SelectedMod
        {
            get => _selectedMod;
            set
            {
                if (_selectedMod != value)
                {
                    _selectedMod = value;
                    OnPropertyChanged(nameof(SelectedMod));
                    OnPropertyChanged(nameof(SelectedModName));
                    OnPropertyChanged(nameof(SomethingSelected));
                }
            }
        }
        [DataSourceProperty]
        public string SelectedModName => SelectedMod == null ? "Mod Name not Specified" : SelectedMod.ModName;
        [DataSourceProperty]
        public bool SomethingSelected => SelectedMod != null;
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
                    OnPropertyChanged();
                    if (SelectedMod?.SettingPropertyGroups.Count > 0)
                    {
                        foreach (var group in SelectedMod.SettingPropertyGroups)
                            group.NotifySearchChanged();
                    }
                }
            }
        }

        public ModSettingsScreenVM()
        {
            TitleLabel = "Mod Options";
            DoneButtonText = new TextObject("{=WiNRdfsm}Done", null).ToString();
            CancelButtonText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
            SearchText = "";

            ModSettingsList = new MBBindingList<ModSettingsVM>();
            foreach (var viewModel in SettingsDatabase.ModSettingsVMs.Select(s => new ModSettingsVM(s, this)))
            {
                viewModel.AddSelectCommand(ExecuteSelect);
                ModSettingsList.Add(viewModel);
                viewModel.RefreshValues();
            }

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            foreach (var viewModel in ModSettingsList)
                viewModel.RefreshValues();

            OnPropertyChanged(nameof(SelectedMod));
        }

        public void ExecuteClose()
        {
            foreach (var viewModel in ModSettingsList)
            {
                viewModel.URS.UndoAll();
                viewModel.URS.ClearStack();
            }
        }

        public bool ExecuteCancel()
        {
            ScreenManager.PopScreen();
            foreach (var viewModel in ModSettingsList)
            {
                viewModel.URS.UndoAll();
                viewModel.URS.ClearStack();
            }
            return true;
        }
        private void ExecuteDone()
        {
            //Save the changes to file.
            if (!ModSettingsList.Any(x => x.URS.ChangesMade()))
            {
                ScreenManager.PopScreen();
                return;
            }

            InformationManager.ShowInquiry(new InquiryData("Game Needs to Restart",
                "The game needs to be restarted to apply mods settings changes. Do you want to close the game now?",
                true, true, "Yes", "No",
                () =>
                {
                    ModSettingsList.Where((x) => x.URS.ChangesMade())
                        .Do((x) => SettingsDatabase.SaveSettings(x.SettingsInstance))
                        .Do((x) => x.URS.ClearStack());

                    Utilities.QuitGame();
                }, () => { }));
        }

        private void ExecuteRevert()
        {
            if (SelectedMod != null)
            {
                InformationManager.ShowInquiry(new InquiryData("Revert mod settings to defaults",
                    $"Are you sure you wish to revert all settings for {SelectedMod.ModName} to their default values?",
                    true, true, "Yes", "No",
                    () =>
                    {
                        SelectedMod.URS.Do(new ComplexAction<(ModSettingsVM VM, SettingsBase SettingsInstance)>((SelectedMod, SelectedMod.SettingsInstance),
                            tuple =>
                            {
                                //Do action
                                tuple.SettingsInstance = SettingsDatabase.ResetSettingsInstance(SelectedMod.SettingsInstance);
                                tuple.VM.RefreshValues();
                                ExecuteSelect(null);
                                ExecuteSelect(tuple.VM);
                            },
                            tuple =>
                            {
                                //Undo action
                                SettingsDatabase.OverrideSettingsWithId(tuple.SettingsInstance, tuple.SettingsInstance.ID);
                                tuple.VM.SettingsInstance = tuple.SettingsInstance;
                                tuple.VM.RefreshValues();
                                if (SelectedMod == tuple.VM)
                                {
                                    ExecuteSelect(null);
                                    ExecuteSelect(tuple.VM);
                                }
                            }));

                    }, null));
            }
        }

        public void ExecuteSelect(ModSettingsVM viewModel)
        {
            if (SelectedMod != viewModel)
            {
                if (SelectedMod != null)
                    SelectedMod.IsSelected = false;

                SelectedMod = viewModel;

                if (SelectedMod != null)
                {
                    SelectedMod.IsSelected = true;
                }
            }
        }
    }
}