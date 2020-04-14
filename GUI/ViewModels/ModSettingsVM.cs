using System;
using TaleWorlds.Library;

namespace ModLib.GUI.ViewModels
{
    public class ModSettingsVM : ViewModel
    {
        private bool _isSelected;
        private Action<ModSettingsVM> _executeSelect = null;
        private MBBindingList<SettingPropertyGroup> _settingPropertyGroups;
        public ModSettingsScreenVM Parent { get; private set; } = null;

        public SettingsBase SettingsInstance { get; internal set; }
        public UndoRedoStack URS { get; } = new UndoRedoStack();

        [DataSourceProperty]
        public string ModName => SettingsInstance.ModName;
        [DataSourceProperty]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        [DataSourceProperty]
        public MBBindingList<SettingPropertyGroup> SettingPropertyGroups
        {
            get => _settingPropertyGroups;
            set
            {
                if (_settingPropertyGroups != value)
                {
                    _settingPropertyGroups = value;
                    OnPropertyChanged();
                }
            }
        }

        public ModSettingsVM(SettingsBase settingsInstance)
        {
            SettingsInstance = settingsInstance;

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            SettingPropertyGroups = new MBBindingList<SettingPropertyGroup>();
            SettingPropertyGroups.AddRange(SettingsInstance.GetSettingPropertyGroups());

            foreach (var settingGroup in SettingPropertyGroups)
                settingGroup.AssignUndoRedoStack(URS);

            foreach (var group in SettingPropertyGroups)
                group.RefreshValues();
            OnPropertyChanged("IsSelected");
            OnPropertyChanged("ModName");
            OnPropertyChanged("SettingPropertyGroups");
        }

        public void AddSelectCommand(Action<ModSettingsVM> command)
        {
            _executeSelect = command;
        }

        public void SetParent(ModSettingsScreenVM parent)
        {
            Parent = parent;
            foreach (var group in SettingPropertyGroups)
                group.SetScreenVM(Parent);
        }

        private void ExecuteSelect()
        {
            _executeSelect?.Invoke(this);
        }

    }
}
