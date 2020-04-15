using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace ModLib.GUI.ViewModels
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
                OnPropertyChanged();
            }
        }
        [DataSourceProperty]
        public bool ChangesMade
        {
            get
            {
                return ModSettingsList.Any((x) => x.URS.ChangesMade());
            }
        }
        [DataSourceProperty]
        public string DoneButtonText
        {
            get => _doneButtonText;
            set
            {
                _doneButtonText = value;
                OnPropertyChanged();
            }
        }
        [DataSourceProperty]
        public string CancelButtonText
        {
            get => _cancelButtonText; set
            {
                _cancelButtonText = value;
                OnPropertyChanged();
            }
        }
        [DataSourceProperty]
        public MBBindingList<ModSettingsVM> ModSettingsList
        {
            get => _modSettingsList;
            set
            {
                if (!(_modSettingsList == value))
                {
                    _modSettingsList = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                    OnPropertyChanged("SelectedModName");
                    OnPropertyChanged("SomethingSelected");
                }
            }
        }
        [DataSourceProperty]
        public string SelectedModName => SelectedMod == null ? "Mod Name Goes Here" : SelectedMod.ModName;
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
                    OnPropertyChanged();
                    OnPropertyChanged("IsHintVisible");
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
                    if (SelectedMod != null && SelectedMod.SettingPropertyGroups.Count > 0)
                    {
                        foreach (var group in SelectedMod.SettingPropertyGroups)
                            group.NotifySearchChanged();
                    }
                }
            }
        }

        public ModSettingsScreenVM()
        {
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            TitleLabel = "Mod Options";
            DoneButtonText = new TextObject("{=WiNRdfsm}Done", null).ToString();
            CancelButtonText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();

            ModSettingsList.Clear();
            foreach (var msvm in SettingsDatabase.ModSettingsVMs)
            {
                msvm.AddSelectCommand(ExecuteSelect);
                ModSettingsList.Add(msvm);
                msvm.RefreshValues();
                msvm.SetParent(this);
            }
            OnPropertyChanged("SelectedMod");
        }

        public bool ExecuteCancel()
        {
            //Revert any changes
            ScreenManager.PopScreen();
            foreach (var msvm in ModSettingsList)
            {
                msvm.URS.UndoAll();
                msvm.URS.ClearStack();
            }
            AssignParent(true);
            ExecuteSelect(null);
            return true;
        }

        private void ExecuteDone()
        {
            //Save the changes to file.
            if (ModSettingsList.Any((x) => x.URS.ChangesMade()))
            {
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
            else
                ScreenManager.PopScreen();
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
                        SelectedMod.URS.Do(new ComplexAction<KeyValuePair<ModSettingsVM, SettingsBase>>(new KeyValuePair<ModSettingsVM, SettingsBase>(SelectedMod, SelectedMod.SettingsInstance),
                            (KeyValuePair<ModSettingsVM, SettingsBase> kvp) =>
                            {
                                //Do action
                                SettingsBase newObj = SettingsDatabase.ResetSettingsInstance(SelectedMod.SettingsInstance);
                                kvp.Key.SettingsInstance = newObj;
                                kvp.Key.RefreshValues();
                                ExecuteSelect(null);
                                ExecuteSelect(kvp.Key);
                            },
                            (KeyValuePair<ModSettingsVM, SettingsBase> kvp) =>
                            {
                                //Undo action
                                SettingsDatabase.OverrideSettingsWithID(kvp.Value, kvp.Value.ID);
                                kvp.Key.SettingsInstance = kvp.Value;
                                kvp.Key.RefreshValues();
                                if (SelectedMod == kvp.Key)
                                {
                                    ExecuteSelect(null);
                                    ExecuteSelect(kvp.Key);
                                }
                            }));

                    }, null));
            }
        }

        public void AssignParent(bool remove = false)
        {
            foreach (var msvm in ModSettingsList)
                msvm.SetParent(remove ? null : this);
        }

        public void ExecuteSelect(ModSettingsVM msvm)
        {
            if (SelectedMod != msvm)
            {
                if (SelectedMod != null)
                    SelectedMod.IsSelected = false;

                SelectedMod = msvm;
                SearchText = "";

                if (SelectedMod != null)
                {
                    SelectedMod.IsSelected = true;
                }
            }
        }
    }
}
