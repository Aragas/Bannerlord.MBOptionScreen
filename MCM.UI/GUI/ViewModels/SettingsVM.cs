using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.SettingsProvider;
using MCM.UI.Actions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal class SettingsVM : ViewModel
    {
        private bool _isSelected;
        private Action<SettingsVM> _executeSelect = default!;
        private MBBindingList<SettingPropertyGroupVM> _settingPropertyGroups = default!;

        public ModOptionsVM MainView { get; }
        public UndoRedoStack URS { get; } = new UndoRedoStack();

        public SettingsDefinition SettingsDefinition { get; }
        public BaseSettings SettingsInstance => BaseSettingsProvider.Instance.GetSettings(SettingsDefinition.SettingsId)!;

        /// <summary>
        /// XSLT?
        /// </summary>
        [DataSourceProperty]
        public int UIVersion => SettingsInstance.UIVersion;
        [DataSourceProperty]
        public string DisplayName => SettingsInstance.DisplayName;
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

        public SettingsVM(SettingsDefinition definition, ModOptionsVM mainView)
        {
            SettingsDefinition = definition;
            MainView = mainView;

            // Can easily backfire as I do not hold the reference
            if (SettingsInstance is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += Settings_PropertyChanged;

            SettingPropertyGroups = new MBBindingList<SettingPropertyGroupVM>();
            foreach (var settingPropertyGroup in SettingsInstance.GetSettingPropertyGroups().Select(d => new SettingPropertyGroupVM(d, this)))
                SettingPropertyGroups.Add(settingPropertyGroup);

            RefreshValues();
        }
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            foreach (var group in SettingPropertyGroups)
                group.RefreshValues();
            OnPropertyChanged(nameof(UIVersion));
            OnPropertyChanged(nameof(IsSelected));
            OnPropertyChanged(nameof(DisplayName));
            OnPropertyChanged(nameof(SettingPropertyGroups));
        }

        public void AddSelectCommand(Action<SettingsVM> command)
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
                var stack = URS.UndoStack.Where(s => s.Context != null && s.Context is PropertyRef propertyRef && propertyRef.PropertyInfo == property.Property).ToList();
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
        private static IEnumerable<SettingsPropertyDefinition> GetAllSettingPropertyDefinitions(SettingPropertyGroupVM settingPropertyGroup1)
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

            // Can easily backfire as I do not hold the reference
            if (SettingsInstance is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged -= Settings_PropertyChanged;

            base.OnFinalize();
        }
    }
}