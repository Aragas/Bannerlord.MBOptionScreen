using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Common;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Models;
using MCM.UI.Actions;
using MCM.UI.Data;
using MCM.UI.GUI.GauntletUI;
using MCM.Utils;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class SettingsPropertyVM : ViewModel
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
        public bool IsIntVisible { get; }
        [DataSourceProperty]
        public bool IsFloatVisible { get; }
        [DataSourceProperty]
        public bool IsBoolVisible { get; }
        [DataSourceProperty]
        public bool IsStringVisible { get; }
        [DataSourceProperty]
        public bool IsDropdownVisible { get; }
        [DataSourceProperty]
        public bool IsDropdownDefaultVisible { get; }
        [DataSourceProperty]
        public bool IsDropdownCheckboxVisible { get; }
        [DataSourceProperty]
        public bool IsButtonVisible { get; }
        [DataSourceProperty]
        public bool IsEnabled => Group.GroupToggle;
        [DataSourceProperty]
        public bool HasEditableText { get; }
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
        [DataSourceProperty]
        public float FloatValue
        {
            get => IsFloatVisible ? PropertyReference.Value is float val ? val : float.MinValue : 0f;
            set
            {
                if (IsFloatVisible && FloatValue != value)
                {
                    URS.Do(new SetValueTypeAction<float>(PropertyReference, value));
                    OnPropertyChanged(nameof(FloatValue));
                    OnPropertyChanged(nameof(TextBoxValue));
                }
            }
        }
        [DataSourceProperty]
        public int IntValue
        {
            get => IsIntVisible ? PropertyReference.Value is int val ? val : int.MinValue : 0;
            set
            {
                if (IsIntVisible && IntValue != value)
                {
                    URS.Do(new SetValueTypeAction<int>(PropertyReference, value));
                    OnPropertyChanged(nameof(IntValue));
                    OnPropertyChanged(nameof(TextBoxValue));
                }
            }
        }
        [DataSourceProperty]
        public bool BoolValue
        {
            get => IsBoolVisible && PropertyReference.Value is bool val ? val : false;
            set
            {
                if (IsBoolVisible && BoolValue != value)
                {
                    URS.Do(new SetValueTypeAction<bool>(PropertyReference, value));
                    OnPropertyChanged(nameof(BoolValue));
                }
            }
        }
        [DataSourceProperty]
        public string StringValue
        {
            get => IsStringVisible ? PropertyReference.Value is string val ? val : "ERROR" : string.Empty;
            set
            {
                if (IsStringVisible && StringValue != value)
                {
                    URS.Do(new SetStringAction(PropertyReference, value));
                    OnPropertyChanged(nameof(StringValue));
                }
            }
        }
        [DataSourceProperty]
        public SelectorVMWrapper DropdownValue
        {
            get => _selectorVMWrapper ??= new SelectorVMWrapper(IsDropdownVisible && PropertyReference.Value is { } val
                    ? SettingsUtils.GetSelector(val)
                    : MCMSelectorVM<MCMSelectorItemVM>.Empty);
            set
            {
                if (IsDropdownVisible && DropdownValue != value)
                {
                    // TODO
                    URS.Do(new ComplexReferenceTypeAction<SelectedIndexWrapper>(PropertyReference, selector =>
                    {
                        //selector.ItemList = DropdownValue.ItemList;
                        if (selector is not null)
                            selector.SelectedIndex = DropdownValue.SelectedIndex;
                    }, selector =>
                    {
                        //selector.ItemList = DropdownValue.ItemList;
                        if (selector is not null)
                            selector.SelectedIndex = DropdownValue.SelectedIndex;
                    }));
                    OnPropertyChanged(nameof(DropdownValue));
                }
            }
        }

        [DataSourceProperty]
        public float MaxValue => (float) SettingPropertyDefinition.MaxValue;
        [DataSourceProperty]
        public float MinValue => (float) SettingPropertyDefinition.MinValue;
        [DataSourceProperty]
        public string ButtonContent => SettingPropertyDefinition.Content;
        [DataSourceProperty]
        public string TextBoxValue => SettingType switch
        {
            SettingType.Int when PropertyReference.Value is int val => string.IsNullOrWhiteSpace(ValueFormat)
                ? string.Format(ValueFormatProvider, "{0}", val.ToString("0"))
                : string.Format(ValueFormatProvider, "{0}", val.ToString(TextObjectHelper.Create(ValueFormat)?.ToString())),
            SettingType.Float when PropertyReference.Value is float val => string.IsNullOrWhiteSpace(ValueFormat)
                ? string.Format(ValueFormatProvider, "{0}", val.ToString("0.00"))
                : string.Format(ValueFormatProvider, "{0}", val.ToString(TextObjectHelper.Create(ValueFormat)?.ToString())),
            _ => string.Empty
        };

        public bool IsSelected { get; set; }

        public SettingsPropertyVM(ISettingsPropertyDefinition definition, SettingsVM settingsVM)
        {
            SettingsVM = settingsVM;
            SettingPropertyDefinition = definition;
            ValueFormatProvider = SettingPropertyDefinition.CustomFormatter is not null
                ? Activator.CreateInstance(SettingPropertyDefinition.CustomFormatter) as IFormatProvider
                : null;

            // Moved to constructor
            IsIntVisible = SettingType == SettingType.Int;
            IsFloatVisible = SettingType == SettingType.Float;
            IsBoolVisible = SettingType == SettingType.Bool;
            IsStringVisible = SettingType == SettingType.String;
            IsDropdownDefaultVisible = SettingType == SettingType.Dropdown && SettingsUtils.IsForTextDropdown(PropertyReference.Value);
            IsDropdownCheckboxVisible = SettingType == SettingType.Dropdown && SettingsUtils.IsForCheckboxDropdown(PropertyReference.Value);
            IsDropdownVisible = IsDropdownDefaultVisible || IsDropdownCheckboxVisible;
            IsButtonVisible = SettingType == SettingType.Button;
            HasEditableText = IsIntVisible || IsFloatVisible;
            // Moved to constructor

            PropertyReference.PropertyChanged += PropertyReference_OnPropertyChanged;

            if (IsDropdownVisible)
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

            if (IsDropdownVisible)
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
        private void DropdownValue_PropertyChanged(object? obj, PropertyChangedEventArgs args)
        {
            if (obj is not null && args.PropertyName == "SelectedIndex")
            {
                URS.Do(new SetSelectedIndexAction(PropertyReference, new SelectedIndexWrapper(obj)));
                SettingsVM.RecalculateIndex();
            }
        }
        private void DropdownValue_PropertyChangedWithValue(object obj, PropertyChangedWithValueEventArgs args)
        {
            if (args.PropertyName == "SelectedIndex")
            {
                URS.Do(new SetSelectedIndexAction(PropertyReference, new SelectedIndexWrapper(obj)));
                SettingsVM.RecalculateIndex();
            }
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
            OnPropertyChanged(nameof(TextBoxValue));
        }


        public void OnResetStart()
        {
            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (IsDropdownVisible)
            {
                DropdownValue.PropertyChanged -= DropdownValue_PropertyChanged;
                DropdownValue.PropertyChangedWithValue -= DropdownValue_PropertyChangedWithValue;
            }
        }
        public void OnResetEnd()
        {
            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (IsDropdownVisible)
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
        public void OnValueClick2() => ScreenManager.PushScreen(new EditValueGauntletScreen(this));

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
