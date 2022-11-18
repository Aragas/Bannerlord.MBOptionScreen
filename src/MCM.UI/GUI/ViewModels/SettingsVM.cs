using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.UI.Actions;
using MCM.UI.Dropdown;
using MCM.UI.Extensions;
using MCM.UI.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class SettingsVM : ViewModel
    {
        private string _displayName = string.Empty;
        private bool _isSelected = false;
        private Action<SettingsVM> _executeSelect = _ => { };
        private readonly Dictionary<PresetKey, BaseSettings> _cachedPresets = new();

        public ModOptionsVM MainView { get; }
        public UndoRedoStack URS { get; } = new();

        public SettingsDefinition SettingsDefinition { get; }
        public BaseSettings? SettingsInstance => BaseSettingsProvider.Instance?.GetSettings(SettingsDefinition.SettingsId);

        public MCMSelectorVM<MCMSelectorItemVM<PresetKey>>? PresetsSelector { get; }

        [DataSourceProperty]
        public string DisplayName { get => _displayName; private set => SetField(ref _displayName, value, nameof(DisplayName)); }
        [DataSourceProperty]
        public bool IsSelected { get => _isSelected; set => SetField(ref _isSelected, value, nameof(IsSelected)); }
        [DataSourceProperty]
        public MBBindingList<SettingsPropertyGroupVM> SettingPropertyGroups { get; } = new();

        public SettingsVM(SettingsDefinition definition, ModOptionsVM mainView)
        {
            SettingsDefinition = definition;
            MainView = mainView;

            foreach (var preset in SettingsInstance?.GetBuiltInPresets().Concat(SettingsInstance.GetExternalPresets()) ?? Enumerable.Empty<ISettingsPreset>())
                _cachedPresets.Add(new PresetKey(preset), preset.LoadPreset());

            var presets = new List<PresetKey> { new("custom", "{=SettingsVM_Custom}Custom") }.Concat(_cachedPresets.Keys);
            PresetsSelector = new MCMSelectorVM<MCMSelectorItemVM<PresetKey>>(presets, -1, null);
            PresetsSelector.ItemList[0].CanBeSelected = false;

            RecalculateIndex();

            if (SettingsInstance is not null)
                SettingPropertyGroups.AddRange(SettingPropertyDefinitionCache.GetSettingPropertyGroups(SettingsInstance).Select(x => new SettingsPropertyGroupVM(x, this)));

            RefreshValues();
        }

        public void RecalculateIndex()
        {
            if (SettingsInstance is null || PresetsSelector is null)
                return;

            var index = 1;
            foreach (var preset in _cachedPresets.Values)
            {
                if (SettingPropertyDefinitionCache.Equals(SettingsInstance, preset))
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

            DisplayName = SettingsInstance?.DisplayName ?? "ERROR";

            SettingPropertyGroups.Sort(UISettingsUtils.SettingsPropertyGroupVMComparer);
            foreach (var group in SettingPropertyGroups)
                group.RefreshValues();
        }

        public void AddSelectCommand(Action<SettingsVM> command) => _executeSelect = command;

        public void ExecuteSelect() => _executeSelect.Invoke(this);

        public bool RestartRequired() => SettingPropertyGroups
            .SelectMany(x => SettingsUtils.GetAllSettingPropertyDefinitions(x.SettingPropertyGroupDefinition))
            .Any(p => p.RequireRestart && URS.RefChanged(p.PropertyReference));

        public void ChangePreset(string presetId)
        {
            if (SettingsInstance is not null && _cachedPresets.TryGetValue(new PresetKey(presetId, string.Empty), out var preset))
                UISettingsUtils.OverrideValues(URS, SettingsInstance, preset);
        }
        public void ChangePresetValue(string presetId, string valueId)
        {
            if (SettingsInstance is not null && _cachedPresets.TryGetValue(new PresetKey(presetId, string.Empty), out var preset))
            {
                var current = SettingPropertyDefinitionCache.GetAllSettingPropertyDefinitions(SettingsInstance).FirstOrDefault(spd => spd.Id == valueId);
                var @new = SettingPropertyDefinitionCache.GetAllSettingPropertyDefinitions(preset).FirstOrDefault(spd => spd.Id == valueId);

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
            if (SettingsInstance is not null)
                BaseSettingsProvider.Instance?.SaveSettings(SettingsInstance);
        }
        public void ResetSettingsValue(string valueId)
        {
            ChangePresetValue(BaseSettings.DefaultPresetId, valueId);
        }


        public override void OnFinalize()
        {
            foreach (var settingPropertyGroup in SettingPropertyGroups)
                settingPropertyGroup.OnFinalize();

            base.OnFinalize();
        }
    }
}