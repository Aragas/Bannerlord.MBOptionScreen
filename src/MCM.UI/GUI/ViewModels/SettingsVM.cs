using Bannerlord.ModuleManager;

using ComparerExtensions;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.UI.Actions;
using MCM.UI.Dropdown;
using MCM.UI.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class SettingsVM : ViewModel
    {
        private string _displayName;
        private bool _isSelected;
        private Action<SettingsVM> _executeSelect = default!;
        private MBBindingList<SettingsPropertyGroupVM> _settingPropertyGroups = default!;
        private readonly IDictionary<PresetKey, BaseSettings>? _cachedPresets;

        public ModOptionsVM MainView { get; }
        public UndoRedoStack URS { get; } = new();

        public SettingsDefinition SettingsDefinition { get; }
        public BaseSettings SettingsInstance => BaseSettingsProvider.Instance!.GetSettings(SettingsDefinition.SettingsId)!;

        public MCMSelectorVM<DropdownSelectorItemVM<PresetKey>>? PresetsSelector { get; }

        [DataSourceProperty]
        public int UIVersion => SettingsInstance.UIVersion;
        [DataSourceProperty]
        public string DisplayName { get => _displayName; private set => SetField(ref _displayName, value, nameof(DisplayName)); }
        [DataSourceProperty]
        public bool IsSelected { get => _isSelected; set => SetField(ref _isSelected, value, nameof(IsSelected)); }
        [DataSourceProperty]
        public MBBindingList<SettingsPropertyGroupVM> SettingPropertyGroups { get => _settingPropertyGroups; set => SetField(ref _settingPropertyGroups, value, nameof(SettingPropertyGroups)); }

        public SettingsVM(SettingsDefinition definition, ModOptionsVM mainView)
        {
            SettingsDefinition = definition;
            MainView = mainView;

            // new TaskFactory().StartNew(() =>
            // {
            _cachedPresets = SettingsInstance.GetBuiltInPresets().Concat(SettingsInstance.GetExternalPresets()).ToDictionary(preset => new PresetKey(preset), preset => preset.LoadPreset());

            var presets = new List<PresetKey> { new("custom", "{=SettingsVM_Custom}Custom") }.Concat(_cachedPresets.Keys);
            PresetsSelector = new MCMSelectorVM<DropdownSelectorItemVM<PresetKey>>(presets, -1, null);
            PresetsSelector.ItemList[0].CanBeSelected = false;

            RecalculateIndex();
            // });

            // Can easily backfire as I do not hold the reference
            if (SettingsInstance is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += Settings_PropertyChanged;

            SettingPropertyGroups = new MBBindingList<SettingsPropertyGroupVM>();
            foreach (var settingPropertyGroup in SettingsInstance.GetSettingPropertyGroups().Select(d => new SettingsPropertyGroupVM(d, this)))
                SettingPropertyGroups.Add(settingPropertyGroup);

            RefreshValues();
        }

        private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BaseSettings.SaveTriggered)
                return;

            RefreshValues();
        }
        public void RecalculateIndex()
        {
            if (_cachedPresets is null || PresetsSelector is null)
                return;

            var settings = SettingsInstance;

            var index = 1;
            foreach (var preset in _cachedPresets.Values)
            {
                if (SettingsUtils.Equals(settings, preset))
                {
                    PresetsSelector.SelectedIndex = index;
                    return;
                }

                index++;
            }

            PresetsSelector.SelectedIndex = 0;
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            DisplayName = SettingsInstance.DisplayName;

            foreach (var group in SettingPropertyGroups)
                group.RefreshValues();

            SettingPropertyGroups.Sort(KeyComparer<SettingsPropertyGroupVM>
                .OrderByDescending(x => x.SettingPropertyGroupDefinition.GroupName == SettingsPropertyGroupDefinition.DefaultGroupName)
                .ThenByDescending(x => x.SettingPropertyGroupDefinition.Order)
                .ThenByDescending(x => new TextObject(x.SettingPropertyGroupDefinition.GroupName).ToString(), new AlphanumComparatorFast()));
        }

        public void AddSelectCommand(Action<SettingsVM> command) => _executeSelect = command;

        public void ExecuteSelect() => _executeSelect.Invoke(this);

        public bool RestartRequired() => SettingPropertyGroups
            .SelectMany(x => SettingsUtils.GetAllSettingPropertyDefinitions(x.SettingPropertyGroupDefinition))
            .Any(p => p.RequireRestart && URS.RefChanged(p.PropertyReference));

        public void ChangePreset(string presetId)
        {
            if (_cachedPresets is null)
                return;

            if (_cachedPresets.TryGetValue(new PresetKey(presetId, string.Empty), out var preset))
                UISettingsUtils.OverrideValues(URS, SettingsInstance, preset);

            RecalculateIndex();
        }
        public void ChangePresetValue(string presetId, string valueId)
        {
            if (_cachedPresets is null)
                return;

            if (_cachedPresets.TryGetValue(new PresetKey(presetId, string.Empty), out var preset))
            {
                var current = SettingsInstance.GetAllSettingPropertyDefinitions().FirstOrDefault(spd => spd.Id == valueId);
                var @new = preset.GetAllSettingPropertyDefinitions().FirstOrDefault(spd => spd.Id == valueId);

                if (current is not null && @new is not null)
                    UISettingsUtils.OverrideValues(URS, current, @new);
            }

            RecalculateIndex();
        }
        public void ResetSettings()
        {
            ChangePreset(BaseSettings.DefaultPresetId);
        }
        public void SaveSettings()
        {
            BaseSettingsProvider.Instance!.SaveSettings(SettingsInstance);
        }
        public void ResetSettingsValue(string valueId)
        {
            ChangePresetValue(BaseSettings.DefaultPresetId, valueId);
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