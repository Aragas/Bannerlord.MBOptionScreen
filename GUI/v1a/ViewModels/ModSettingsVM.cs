using MBOptionScreen.Settings;

using System;
using System.Linq;

using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1a.ViewModels
{
    public class ModSettingsVM : ViewModel
    {
        private bool _isSelected;
        private Action<ModSettingsVM> _executeSelect;
        private MBBindingList<SettingPropertyGroup> _settingPropertyGroups;

        public ModSettingsScreenVM MainView { get; }
        public UndoRedoStack URS { get; } = new UndoRedoStack();

        public ModSettingsDefinition ModSettingsDefinition { get; }
        public SettingsBase SettingsInstance => ModSettingsDefinition.SettingsInstance;

        /// <summary>
        /// XSLT?
        /// </summary>
        [DataSourceProperty]
        public int UIVersion => SettingsInstance.UIVersion;
        [DataSourceProperty]
        public string ModName => SettingsInstance.ModName;
        [DataSourceProperty]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
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
                    OnPropertyChanged(nameof(SettingPropertyGroups));
                }
            }
        }

        public ModSettingsVM(ModSettingsDefinition definition, ModSettingsScreenVM mainView)
        {
            ModSettingsDefinition = definition;
            MainView = mainView;

            SettingPropertyGroups = new MBBindingList<SettingPropertyGroup>();
            foreach (var settingPropertyGroup in SettingsInstance.GetSettingPropertyGroups().Select(d => new SettingPropertyGroup(d, this)))
                SettingPropertyGroups.Add(settingPropertyGroup);

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            foreach (var group in SettingPropertyGroups)
                group.RefreshValues();
            OnPropertyChanged(nameof(UIVersion));
            OnPropertyChanged(nameof(IsSelected));
            OnPropertyChanged(nameof(ModName));
            OnPropertyChanged(nameof(SettingPropertyGroups));
        }

        public void AddSelectCommand(Action<ModSettingsVM> command)
        {
            _executeSelect = command;
        }

        private void ExecuteSelect()
        {
            _executeSelect?.Invoke(this);
        }
    }
}