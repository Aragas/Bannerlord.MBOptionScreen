using MCM.Abstractions;
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
        private MBBindingList<SettingsPropertyGroupVM> _settingPropertyGroups = default!;

        public ModOptionsVM MainView { get; }
        public UndoRedoStack URS { get; } = new UndoRedoStack();

        public SettingsDefinition SettingsDefinition { get; }
        public BaseSettings SettingsInstance => BaseSettingsProvider.Instance.GetSettings(SettingsDefinition.SettingsId)!;

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
        public MBBindingList<SettingsPropertyGroupVM> SettingPropertyGroups
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

            SettingPropertyGroups = new MBBindingList<SettingsPropertyGroupVM>();
            foreach (var settingPropertyGroup in SettingsInstance.GetSettingPropertyGroups().Select(d => new SettingsPropertyGroupVM(d, this)))
                SettingPropertyGroups.Add(settingPropertyGroup);

            RefreshValues();
        }
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BaseSettings.SaveTriggered)
                return;

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
                var stack = URS.UndoStack.Where(s => s.Context is PropertyRef propertyRef && propertyRef.PropertyInfo == property.Property).ToList();
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
        private static IEnumerable<ISettingsPropertyDefinition> GetAllSettingPropertyDefinitions(SettingsPropertyGroupVM settingPropertyGroup1)
        {
            foreach (var settingProperty in settingPropertyGroup1.SettingProperties)
                yield return settingProperty.SettingPropertyDefinition;

            foreach (var settingPropertyGroup in settingPropertyGroup1.SettingPropertyGroups)
            foreach (var settingProperty in GetAllSettingPropertyDefinitions(settingPropertyGroup))
            {
                yield return settingProperty;
            }
        }

        public void ResetSettings()
        {
            var settings = SettingsInstance;
            var newSettings = settings is IWrapper wrapper
                ? BaseGlobalSettingsWrapper.Create(Activator.CreateInstance(wrapper.Object.GetType()))
                : (GlobalSettings) Activator.CreateInstance(settings.GetType());

            OverrideSettings(newSettings);
        }
        public void OverrideSettings(BaseSettings newSettings)
        {
            var settings = SettingsInstance;
            if (settings is IWrapper wrapper && newSettings is IWrapper newWrapper)
                Utils.OverridePropertyValues(SettingPropertyGroups.SelectMany(GetAllSettingPropertyVMs), wrapper.Object, newWrapper.Object);
            else
                Utils.OverridePropertyValues(SettingPropertyGroups.SelectMany(GetAllSettingPropertyVMs), settings, newSettings);
        }
        private static IEnumerable<SettingsPropertyVM> GetAllSettingPropertyVMs(SettingsPropertyGroupVM settingPropertyGroup1)
        {
            foreach (var settingProperty in settingPropertyGroup1.SettingProperties)
                yield return settingProperty;

            foreach (var settingPropertyGroup in settingPropertyGroup1.SettingPropertyGroups)
            foreach (var settingProperty in GetAllSettingPropertyVMs(settingPropertyGroup))
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