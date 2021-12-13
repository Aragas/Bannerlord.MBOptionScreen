using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Providers;
using MCM.Extensions;
using MCM.UI.Actions;
using MCM.UI.Utils;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class SettingsVM : ViewModel
    {
        private bool _isSelected;
        private Action<SettingsVM> _executeSelect = default!;
        private MBBindingList<SettingsPropertyGroupVM> _settingPropertyGroups = default!;
        private readonly IDictionary<string, BaseSettings>? _cachedPresets;

        public ModOptionsVM MainView { get; }
        public UndoRedoStack URS { get; } = new();

        public SettingsDefinition SettingsDefinition { get; }
        public BaseSettings SettingsInstance => BaseSettingsProvider.Instance!.GetSettings(SettingsDefinition.SettingsId)!;

        public SelectorVM<SelectorItemVM>? PresetsSelector { get; }

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

            // new TaskFactory().StartNew(() =>
            // {
            _cachedPresets = SettingsInstance.GetAvailablePresets().ToDictionary(pair => pair.Key, pair => pair.Value());

            PresetsSelector = new SelectorVM<SelectorItemVM>(new List<string> { TextObjectHelper.Create("{=SettingsVM_Custom}Custom")?.ToString() ?? "ERROR" }.Concat(_cachedPresets.Keys.Select(x => TextObjectHelper.Create(x)?.ToString())), -1, null);
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

            foreach (var group in SettingPropertyGroups)
                group.RefreshValues();
            OnPropertyChanged(nameof(UIVersion));
            OnPropertyChanged(nameof(IsSelected));
            OnPropertyChanged(nameof(DisplayName));
            OnPropertyChanged(nameof(SettingPropertyGroups));
        }

        public void AddSelectCommand(Action<SettingsVM> command) => _executeSelect = command;

        public void ExecuteSelect() => _executeSelect.Invoke(this);

        public bool RestartRequired() => SettingPropertyGroups
            .SelectMany(x => SettingsUtils.GetAllSettingPropertyDefinitions(x.SettingPropertyGroupDefinition))
            .Any(p => p.RequireRestart && URS.RefChanged(p.PropertyReference));

        public void ChangePreset(string presetName)
        {
            if (_cachedPresets is null)
                return;

            var settings = SettingsInstance;

            if (_cachedPresets.TryGetValue(presetName, out var preset))
                UISettingsUtils.OverrideValues(URS, settings, preset);

            RecalculateIndex();
        }
        public void ChangePresetValue(string presetName, string valueId)
        {
            if (_cachedPresets is null)
                return;

            var settings = SettingsInstance;

            if (_cachedPresets.TryGetValue(presetName, out var preset))
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
            ChangePreset(TextObjectHelper.Create("{=BaseSettings_Default}Default")?.ToString() ?? string.Empty);
        }
        public void SaveSettings()
        {
            BaseSettingsProvider.Instance!.SaveSettings(SettingsInstance);
        }
        public void ResetSettingsValue(string valueId)
        {
            ChangePresetValue(TextObjectHelper.Create("{=BaseSettings_Default}Default")?.ToString() ?? string.Empty, valueId);
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