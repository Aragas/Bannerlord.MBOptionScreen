using MCM.Abstractions.Common;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Models;
using MCM.UI.Actions;
using MCM.UI.Data;
using MCM.Utils;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        private SelectorVMWrapper? _selectorVMWrapper;

        public ModOptionsVM MainView => SettingsVM.MainView;
        public SettingsVM SettingsVM { get; }
        public SettingsPropertyGroupVM Group { get; set; } = default!;

        public UndoRedoStack URS => SettingsVM.URS;

        public ISettingsPropertyDefinition SettingPropertyDefinition { get; }
        public IRef PropertyReference => SettingPropertyDefinition.PropertyReference;
        public SettingType SettingType => SettingPropertyDefinition.SettingType;
        public string HintText => SettingPropertyDefinition.HintText.Length > 0
            ? $"{Name}: {SettingPropertyDefinition.HintText}"
            : string.Empty;
        public string ValueFormat => SettingPropertyDefinition.ValueFormat;
        public IFormatProvider? ValueFormatProvider { get; }

        public bool SatisfiesSearch => string.IsNullOrEmpty(MainView.SearchText) ||
                                       Name.IndexOf(MainView.SearchText, StringComparison.InvariantCultureIgnoreCase) >= 0;

        [DataSourceProperty]
        public string Name => SettingPropertyDefinition.DisplayName;

        [DataSourceProperty]
        public bool IsEnabled => Group.GroupToggle;

        [DataSourceProperty]
        public bool IsSettingVisible
        {
            get
            {
                if (SettingPropertyDefinition.IsToggle)
                    return false;
                if (!Group.GroupToggle)
                    return false;
                if (!Group.IsExpanded)
                    return false;
                if (!SatisfiesSearch)
                    return false;
                return true;
            }
        }

        public bool IsSelected { get; set; }

        public SettingsPropertyVM(ISettingsPropertyDefinition definition, SettingsVM settingsVM)
        {
            SettingsVM = settingsVM;
            SettingPropertyDefinition = definition;
            ValueFormatProvider = SettingPropertyDefinition.CustomFormatter is not null
                ? Activator.CreateInstance(SettingPropertyDefinition.CustomFormatter) as IFormatProvider
                : null;

            // Moved to constructor
            IsInt = SettingType == SettingType.Int;
            IsFloat = SettingType == SettingType.Float;
            IsBool = SettingType == SettingType.Bool;
            IsString = SettingType == SettingType.String;
            IsDropdownDefault = SettingType == SettingType.Dropdown && SettingsUtils.IsForTextDropdown(PropertyReference.Value);
            IsDropdownCheckbox = SettingType == SettingType.Dropdown && SettingsUtils.IsForCheckboxDropdown(PropertyReference.Value);
            IsDropdown = IsDropdownDefault || IsDropdownCheckbox;
            IsButton = SettingType == SettingType.Button;
            IsNotNumeric = !(IsInt || IsFloat);
            NumericValueToggle = IsInt || IsFloat;
            // Moved to constructor

            PropertyReference.PropertyChanged += PropertyReference_OnPropertyChanged;

            if (IsDropdown)
            {
                DropdownValue.PropertyChanged += DropdownValue_PropertyChanged;
                DropdownValue.PropertyChangedWithValue += DropdownValue_PropertyChangedWithValue;
            }

            RefreshValues();

            if (MCMUISubModule.ResetValueToDefault is { } key)
                key.OnReleasedEvent += ResetValueToDefaultOnReleasedEvent;
        }
        public override void OnFinalize()
        {
            if (MCMUISubModule.ResetValueToDefault is { } key)
                key.OnReleasedEvent -= ResetValueToDefaultOnReleasedEvent;

            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (IsDropdown)
            {
                DropdownValue.PropertyChanged -= DropdownValue_PropertyChanged;
                DropdownValue.PropertyChangedWithValue -= DropdownValue_PropertyChangedWithValue;
            }

            base.OnFinalize();
        }
        private void PropertyReference_OnPropertyChanged(object? obj, PropertyChangedEventArgs args)
        {
            RefreshValues();
            SettingsVM.RecalculateIndex();
            //SettingsVM.RefreshValues();
        }

        private void ResetValueToDefaultOnReleasedEvent()
        {
            if (IsSelected)
            {
                SettingsVM.ResetSettingsValue(SettingPropertyDefinition.Id);
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            switch (SettingType)
            {
                case SettingType.Bool:
                    OnPropertyChanged(nameof(BoolValue));
                    break;
                case SettingType.Int:
                    OnPropertyChanged(nameof(IntValue));
                    break;
                case SettingType.Float:
                    OnPropertyChanged(nameof(FloatValue));
                    break;
                case SettingType.String:
                    OnPropertyChanged(nameof(StringValue));
                    break;
                case SettingType.Dropdown:
                    OnPropertyChanged(nameof(DropdownValue));
                    break;
                case SettingType.Button:
                    break;
            }
            OnPropertyChanged(nameof(NumericValue));
        }


        public void OnResetStart()
        {
            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (IsDropdown)
            {
                DropdownValue.PropertyChanged -= DropdownValue_PropertyChanged;
                DropdownValue.PropertyChangedWithValue -= DropdownValue_PropertyChangedWithValue;
            }
        }
        public void OnResetEnd()
        {
            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (IsDropdown)
            {
                DropdownValue.PropertyChanged -= DropdownValue_PropertyChanged;
                DropdownValue.PropertyChangedWithValue -= DropdownValue_PropertyChangedWithValue;
            }

            RefreshValues();
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("Redundancy", "RCS1213:Remove unused member declaration.", Justification = "Reflection is used.")]
        public void OnHover()
        {
            IsSelected = true;
            MainView.HintText = HintText;
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("Redundancy", "RCS1213:Remove unused member declaration.", Justification = "Reflection is used.")]
        public void OnHoverEnd()
        {
            IsSelected = false;
            MainView.HintText = string.Empty;
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("Redundancy", "RCS1213:Remove unused member declaration.", Justification = "Reflection is used.")]
        public void OnValueClick()
        {
            if (PropertyReference.Value is Action val)
            {
                val();
            }
        }

        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();
    }
}