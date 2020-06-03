using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Models;
using MCM.UI.Actions;
using MCM.UI.GUI.GauntletUI;
using MCM.Utils;

using System;
using System.ComponentModel;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class SettingsPropertyVM : ViewModel
    {
        public ModOptionsVM MainView => SettingsVM.MainView;
        public SettingsVM SettingsVM { get; }
        public SettingsPropertyGroupVM Group { get; set; } = default!;

        public UndoRedoStack URS => SettingsVM.URS;

        public ISettingsPropertyDefinition SettingPropertyDefinition { get; }
        public IRef PropertyReference => SettingPropertyDefinition.PropertyReference;
        public SettingType SettingType => SettingPropertyDefinition.SettingType;
        public string HintText => SettingPropertyDefinition.HintText.Length > 0 ? $"{Name}: {SettingPropertyDefinition.HintText}" : "";
        public string ValueFormat => SettingPropertyDefinition.ValueFormat;
        public IFormatProvider? ValueFormatProvider { get; }
        
        public bool SatisfiesSearch
        {
            get
            {
                if (MainView == null || string.IsNullOrEmpty(MainView.SearchText))
                    return true;

                return Name.IndexOf(MainView.SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        [DataSourceProperty]
        public string Name => SettingPropertyDefinition.DisplayName;

        [DataSourceProperty]
        public bool IsIntVisible => SettingType == SettingType.Int;
        [DataSourceProperty]
        public bool IsFloatVisible => SettingType == SettingType.Float;
        [DataSourceProperty]
        public bool IsBoolVisible => SettingType == SettingType.Bool;
        [DataSourceProperty]
        public bool IsStringVisible => SettingType == SettingType.String;
        [DataSourceProperty]
        public bool IsDropdownVisible => SettingType == SettingType.Dropdown;
        [DataSourceProperty]
        public bool IsEnabled => Group?.GroupToggle != false;
        [DataSourceProperty]
        public bool HasEditableText => IsIntVisible || IsFloatVisible;
        [DataSourceProperty]
        public bool IsSettingVisible
        {
            get
            {
                if (Group != null && SettingPropertyDefinition.IsMainToggle)
                    return false;
                if (Group?.GroupToggle == false)
                    return false;
                if (Group?.IsExpanded == false)
                    return false;
                if (!SatisfiesSearch)
                    return false;
                return true;
            }
        }
        [DataSourceProperty]
        public float FloatValue
        {
            get => IsFloatVisible ? (float) PropertyReference.Value : 0f;
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
            get => IsIntVisible ? (int) PropertyReference.Value : 0;
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
            get => IsBoolVisible && (bool) PropertyReference.Value;
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
            get => IsStringVisible ? (string) PropertyReference.Value : "";
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
        public SelectorVM<SelectorItemVM> DropdownValue
        {
            get => IsDropdownVisible ? SettingsUtils.GetSelector(PropertyReference.Value) : new SelectorVM<SelectorItemVM>(0, null);
            set
            {
                if (IsDropdownVisible && DropdownValue != value)
                {
                    URS.Do(new ComplexReferenceTypeAction<SelectorVM<SelectorItemVM>>(PropertyReference, selector =>
                    {
                        selector.ItemList = DropdownValue.ItemList;
                        selector.SelectedIndex = DropdownValue.SelectedIndex;
                    }, selector =>
                    {
                        selector.ItemList = DropdownValue.ItemList;
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
        public string? TextBoxValue => SettingType switch
        {
            SettingType.Int => string.IsNullOrWhiteSpace(ValueFormat)
                ? string.Format(ValueFormatProvider, "{0}", ((int) PropertyReference.Value).ToString("0"))
                : string.Format(ValueFormatProvider, "{0}", ((int) PropertyReference.Value).ToString(ValueFormat)),
            SettingType.Float => string.IsNullOrWhiteSpace(ValueFormat)
                ? string.Format(ValueFormatProvider, "{0}", ((float) PropertyReference.Value).ToString("0:00"))
                : string.Format(ValueFormatProvider, "{0}", ((float) PropertyReference.Value).ToString(ValueFormat)),
            _ => ""
        };

        public SettingsPropertyVM(ISettingsPropertyDefinition definition, SettingsVM settingsVM)
        {
            SettingsVM = settingsVM;
            SettingPropertyDefinition = definition;
            ValueFormatProvider = SettingPropertyDefinition.CustomFormatter!= null
                ? Activator.CreateInstance(SettingPropertyDefinition.CustomFormatter) as IFormatProvider
                : null;

            PropertyReference.PropertyChanged += PropertyReference_OnPropertyChanged;

            if (IsDropdownVisible)
                DropdownValue.PropertyChanged += DropdownValue_PropertyChanged;

            RefreshValues();
        }
        public override void OnFinalize()
        {
            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (IsDropdownVisible)
                DropdownValue.PropertyChanged -= DropdownValue_PropertyChanged;

            base.OnFinalize();
        }
        private void PropertyReference_OnPropertyChanged(object obj, PropertyChangedEventArgs args)
        {
            RefreshValues();
            SettingsVM.RecalculateIndex();
            //SettingsVM.RefreshValues();
        }
        private void DropdownValue_PropertyChanged(object obj, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(SelectorVM<SelectorItemVM>.SelectedIndex))
            {
                URS.Do(new SetDropdownIndexAction(PropertyReference, (SelectorVM<SelectorItemVM>) obj));
                SettingsVM.RecalculateIndex();
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
            }
            OnPropertyChanged(nameof(TextBoxValue));
        }


        public void OnResetStart()
        {
            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (IsDropdownVisible)
                DropdownValue.PropertyChanged -= DropdownValue_PropertyChanged;
        }
        public void OnResetEnd()
        {
            PropertyReference.PropertyChanged += PropertyReference_OnPropertyChanged;

            if (IsDropdownVisible)
                DropdownValue.PropertyChanged += DropdownValue_PropertyChanged;

            RefreshValues();
        }
        private void OnHover() { if (MainView != null) MainView.HintText = HintText; }
        private void OnHoverEnd() { if (MainView != null) MainView.HintText = ""; }
        private void OnValueClick() => ScreenManager.PushScreen(new EditValueGauntletScreen(this));

        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();
    }
}