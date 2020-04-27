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

            foreach (var property in properties)
            {
                var stack = URS.UndoStack.Where(s => s.Context != null && s.Context._propInfo == property.Property).ToList();
                if (stack.Count == 0)
                    continue;
                else
                {
                    var firstChange = stack.First();
                    var lastChange = stack.Last();
                    var originalValue = firstChange.Original;
                    var currentValue = lastChange.Value;
                    if (!originalValue.Equals(currentValue))
                        return true;
                }
            }

            return false;
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


        public override void OnFinalize()
        {
            foreach (var settingPropertyGroup in SettingPropertyGroups)
                settingPropertyGroup.OnFinalize();

            base.OnFinalize();
        }
    }
}