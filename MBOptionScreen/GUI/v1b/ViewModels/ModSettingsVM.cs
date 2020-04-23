using MBOptionScreen.Actions;
using MBOptionScreen.Settings;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1b.ViewModels
{
    public class ModSettingsVM : ViewModel
    {
        private bool _isSelected;
        private readonly Dictionary<SettingPropertyDefinition, object> _initialValues = new Dictionary<SettingPropertyDefinition, object>();
        private Action<ModSettingsVM> _executeSelect;
        private MBBindingList<SettingPropertyGroupVM> _settingPropertyGroups;

        public ModOptionsScreenVM MainView { get; }
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
        public MBBindingList<SettingPropertyGroupVM> SettingPropertyGroups
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

        public ModSettingsVM(ModSettingsDefinition definition, ModOptionsScreenVM mainView)
        {
            ModSettingsDefinition = definition;
            MainView = mainView;

            SettingPropertyGroups = new MBBindingList<SettingPropertyGroupVM>();
            foreach (var settingPropertyGroup in SettingsInstance.GetSettingPropertyGroups().Select(d => new SettingPropertyGroupVM(d, this)))
                SettingPropertyGroups.Add(settingPropertyGroup);

            var properties = SettingPropertyGroups.SelectMany(GetAllSettingPropertyDefinitions)
                .Where(p => p.RequireRestart);
            var initialSettings = SettingsInstance;
            _initialValues = properties.ToDictionary(p => p, p => p.Property.GetValue(initialSettings));

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

        public bool RestartRequired()
        {
            var properties = SettingPropertyGroups.SelectMany(GetAllSettingPropertyDefinitions)
                .Where(p => p.RequireRestart);

            var lastSettings = SettingsInstance;
            var lastValues = properties.ToDictionary(p => p, p => p.Property.GetValue(lastSettings));

            var diff1 = lastValues.Except(_initialValues);
            var diff2 = _initialValues.Except(lastValues);
            return diff1.Concat(diff2).Any();
        }
        private static IEnumerable<SettingPropertyDefinition> GetAllSettingPropertyDefinitions(SettingPropertyGroupVM settingPropertyGroup1)
        {
            foreach (var settingProperty in settingPropertyGroup1.SettingProperties)
                yield return settingProperty.SettingPropertyDefinition;

            foreach (var settingPropertyGroup in settingPropertyGroup1.SettingPropertyGroups)
                foreach (var settingProperty in GetAllSettingPropertyDefinitions(settingPropertyGroup))
                {
                    yield return settingProperty;
                }
        }
    }
}