using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.SettingsProvider;
using MCM.UI.Actions;
using MCM.UI.GUI.GauntletUI;

using System;
using System.ComponentModel;
using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal class SettingPropertyVM :
        ViewModel,
        ISettingPropertyStringValue,
        ISettingPropertyIntValue,
        ISettingPropertyFloatValue
    {
        private SelectorVM<SelectorItemVM> GetSelector(object dropdown)
        {
            var selectorProperty = dropdown.GetType().GetProperty("Selector");
            if (selectorProperty == null)
                return new SelectorVM<SelectorItemVM>(0, _ => { });
            return (SelectorVM<SelectorItemVM>)selectorProperty.GetValue(dropdown);
        }

        protected ModOptionsVM MainView => SettingsVM.MainView;
        protected SettingsVM SettingsVM { get; }
        public UndoRedoStack URS => SettingsVM.URS;

        public SettingsPropertyDefinition SettingPropertyDefinition { get; }
        public PropertyInfo Property => SettingPropertyDefinition.Property;
        public SettingsBase SettingsInstance => BaseSettingsProvider.Instance.GetSettings(SettingPropertyDefinition.SettingsId);
        public SettingType SettingType => SettingPropertyDefinition.SettingType;
        public SettingPropertyGroupVM Group { get; set; }
        public string HintText { get; }
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
        public string Name => SettingPropertyDefinition.DisplayName.ToString();

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
            get => SettingType == SettingType.Float ? (float) Property.GetValue(SettingsInstance) : 0f;
            set
            {
                if (SettingType == SettingType.Float && FloatValue != value)
                {
                    URS.Do(new SetValueTypeAction<float>(new PropertyRef(Property, SettingsInstance), value));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public int IntValue
        {
            get => SettingType == SettingType.Int ? (int) Property.GetValue(SettingsInstance) : 0;
            set
            {
                if (SettingType == SettingType.Int && IntValue != value)
                {
                    URS.Do(new SetValueTypeAction<int>(new PropertyRef(Property, SettingsInstance), value));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public bool BoolValue
        {
            get => SettingType == SettingType.Bool && (bool) Property.GetValue(SettingsInstance);
            set
            {
                if (SettingType == SettingType.Bool && BoolValue != value)
                {
                    URS.Do(new SetValueTypeAction<bool>(new PropertyRef(Property, SettingsInstance), value));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public string StringValue
        {
            get => SettingType == SettingType.String ? (string) Property.GetValue(SettingsInstance) : "";
            set
            {
                if (SettingType == SettingType.String && StringValue != value)
                {
                    URS.Do(new SetStringAction(new PropertyRef(Property, SettingsInstance), value));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> DropdownValue
        {
            get => SettingType == SettingType.Dropdown ? GetSelector(Property.GetValue(SettingsInstance)) : new SelectorVM<SelectorItemVM>(0, null);
            set
            {
                if (SettingType == SettingType.Dropdown && DropdownValue != value)
                {
                    DropdownValue.PropertyChanged -= OnDropdownPropertyChanged;
                    value.PropertyChanged += OnDropdownPropertyChanged;
                    URS.Do(new SetDropdownAction(new PropertyRef(Property, SettingsInstance), value));
                    OnPropertyChanged(nameof(DropdownValue));
                }
            }
        }
        [DataSourceProperty]
        public float MaxValue => SettingPropertyDefinition.MaxValue;
        [DataSourceProperty]
        public float MinValue => SettingPropertyDefinition.MinValue;
        [DataSourceProperty]
        public string? ValueString => SettingType switch
        {
            SettingType.Int => string.IsNullOrWhiteSpace(ValueFormat) ? ((int) Property.GetValue(SettingsInstance)).ToString("0") : ((int) Property.GetValue(SettingsInstance)).ToString(ValueFormat),
            SettingType.Float => string.IsNullOrWhiteSpace(ValueFormat) ? ((float) Property.GetValue(SettingsInstance)).ToString("0.00") : ((float) Property.GetValue(SettingsInstance)).ToString(ValueFormat),
            SettingType.String => (string) Property.GetValue(SettingsInstance),
            SettingType.Dropdown => DropdownValue?.SelectedItem?.StringItem ?? "",
            _ => ""
        };
        [DataSourceProperty]
        public Action OnHoverAction => OnHover;
        [DataSourceProperty]
        public Action OnHoverEndAction => OnHoverEnd;

        public SettingPropertyVM(SettingsPropertyDefinition definition, SettingsVM settingsVM)
        {
            SettingsVM = settingsVM;
            SettingPropertyDefinition = definition;

            HintText = SettingPropertyDefinition.HintText.Length > 0 ? $"{Name}: {SettingPropertyDefinition.HintText}" : "";

            if (SettingType == SettingType.Dropdown)
                DropdownValue.PropertyChanged += OnDropdownPropertyChanged;


            RefreshValues();
        }
        public override void OnFinalize()
        {
            if (SettingType == SettingType.Dropdown)
            {
                DropdownValue.PropertyChanged -= OnDropdownPropertyChanged;
            }

            base.OnFinalize();
        }
        private void OnDropdownPropertyChanged(object obj, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(SelectorVM<SelectorItemVM>.SelectedIndex))
            URS.DoWithoutDo(new SetDropdownIndexAction(new PropertyRef(Property, SettingsInstance), (SelectorVM<SelectorItemVM>) obj));
        }

        public void OnHover()
        {
            if (MainView != null)
                MainView.HintText = HintText;
        }

        public void OnHoverEnd()
        {
            if (MainView != null)
                MainView.HintText = "";
        }

        private void OnValueClick()
        {
            ScreenManager.PushScreen(new EditValueGauntletScreen(this));
        }

        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();
    }
}