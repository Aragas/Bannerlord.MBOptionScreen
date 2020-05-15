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
    internal class SettingsPropertyVM : ViewModel
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
        public bool HasEditableText => SettingType == SettingType.Int || SettingType == SettingType.Float;
        [DataSourceProperty]
        public bool IsSettingVisible
        {
            get
            {
                if (Group != null && SettingPropertyDefinition.IsMainToggle)
                    return false;
                else if (Group?.GroupToggle == false)
                    return false;
                else if (Group?.IsExpanded == false)
                    return false;
                else if (!SatisfiesSearch)
                    return false;
                else
                    return true;
            }
        }
        [DataSourceProperty]
        public float FloatValue
        {
            get => SettingType == SettingType.Float ? (float) PropertyReference.Value : 0f;
            set
            {
                if (SettingType == SettingType.Float && FloatValue != value)
                {
                    URS.Do(new SetValueTypeAction<float>(PropertyReference, value));
                    OnPropertyChanged(nameof(FloatValue));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public int IntValue
        {
            get => SettingType == SettingType.Int ? (int) PropertyReference.Value : 0;
            set
            {
                if (SettingType == SettingType.Int && IntValue != value)
                {
                    URS.Do(new SetValueTypeAction<int>(PropertyReference, value));
                    OnPropertyChanged(nameof(IntValue));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public bool BoolValue
        {
            get => SettingType == SettingType.Bool && (bool) PropertyReference.Value;
            set
            {
                if (SettingType == SettingType.Bool && BoolValue != value)
                {
                    URS.Do(new SetValueTypeAction<bool>(PropertyReference, value));
                    OnPropertyChanged(nameof(BoolValue));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public string StringValue
        {
            get => SettingType == SettingType.String ? (string) PropertyReference.Value : "";
            set
            {
                if (SettingType == SettingType.String && StringValue != value)
                {
                    URS.Do(new SetStringAction(PropertyReference, value));
                    OnPropertyChanged(nameof(StringValue));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> DropdownValue
        {
            get => SettingType == SettingType.Dropdown ? SettingsUtils.GetSelector(PropertyReference.Value) : new SelectorVM<SelectorItemVM>(0, null);
            set
            {
                if (SettingType == SettingType.Dropdown && DropdownValue != value)
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
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public float MaxValue => (float) SettingPropertyDefinition.MaxValue;
        [DataSourceProperty]
        public float MinValue => (float) SettingPropertyDefinition.MinValue;
        [DataSourceProperty]
        public string? ValueString => SettingType switch
        {
            SettingType.Int => string.IsNullOrWhiteSpace(ValueFormat)
                ? ((int) PropertyReference.Value).ToString("0")
                : ((int) PropertyReference.Value).ToString(ValueFormat),
            SettingType.Float => string.IsNullOrWhiteSpace(ValueFormat)
                ? ((float) PropertyReference.Value).ToString("0.00")
                : ((float) PropertyReference.Value).ToString(ValueFormat),
            SettingType.String => (string) PropertyReference.Value,
            SettingType.Dropdown => DropdownValue?.SelectedItem?.StringItem ?? "",
            _ => ""
        };
        [DataSourceProperty]
        public Action OnHoverAction => OnHover;
        [DataSourceProperty]
        public Action OnHoverEndAction => OnHoverEnd;

        public SettingsPropertyVM(ISettingsPropertyDefinition definition, SettingsVM settingsVM)
        {
            SettingsVM = settingsVM;
            SettingPropertyDefinition = definition;

            PropertyReference.PropertyChanged += PropertyReference_OnPropertyChanged;

            if (SettingType == SettingType.Dropdown)
                DropdownValue.PropertyChanged += DropdownValue_PropertyChanged;

            RefreshValues();
        }
        public override void OnFinalize()
        {
            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (SettingType == SettingType.Dropdown)
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
            OnPropertyChanged(nameof(ValueString));
        }


        public void OnResetStart()
        {
            PropertyReference.PropertyChanged -= PropertyReference_OnPropertyChanged;

            if (SettingType == SettingType.Dropdown)
                DropdownValue.PropertyChanged -= DropdownValue_PropertyChanged;
        }
        public void OnResetEnd()
        {
            PropertyReference.PropertyChanged += PropertyReference_OnPropertyChanged;

            if (SettingType == SettingType.Dropdown)
                DropdownValue.PropertyChanged += DropdownValue_PropertyChanged;

            RefreshValues();
        }
        public void OnHover() { if (MainView != null) MainView.HintText = HintText; }
        public void OnHoverEnd() { if (MainView != null) MainView.HintText = ""; }
        private void OnValueClick() => ScreenManager.PushScreen(new EditValueGauntletScreen(this));

        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();
    }
}