using MBOptionScreen.Actions;
using MBOptionScreen.Attributes;
using MBOptionScreen.GUI.v1a.GauntletUI;
using MBOptionScreen.Settings;

using System;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1a.ViewModels
{
    public class SettingPropertyVM : ViewModel
    {
        private SelectorVM<SelectorItemVM> _selector;

        protected ModOptionsScreenVM MainView => ModSettingsView.MainView;
        protected ModSettingsVM ModSettingsView { get; }
        public UndoRedoStack URS => ModSettingsView.URS;

        public SettingPropertyDefinition SettingPropertyDefinition { get; }
        public SettingPropertyAttribute SettingAttribute => SettingPropertyDefinition.SettingAttribute;
        public PropertyInfo Property => SettingPropertyDefinition.Property;
        public SettingPropertyGroupAttribute GroupAttribute => SettingPropertyDefinition.GroupAttribute;
        public SettingsBase SettingsInstance => SettingPropertyDefinition.SettingsInstance;
        public SettingType SettingType => SettingPropertyDefinition.SettingType;
        public SettingPropertyGroupVM Group { get; set; }
        public string HintText { get; }
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
        public string Name => SettingAttribute.DisplayName;

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
                if (Group != null && GroupAttribute?.IsMainToggle == true)
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
                    URS.Do(new SetValueAction<float>(new Ref(Property, SettingsInstance), value));
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
                    URS.Do(new SetValueAction<int>(new Ref(Property, SettingsInstance), value));
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
                    URS.Do(new SetValueAction<bool>(new Ref(Property, SettingsInstance), value));
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
                    URS.Do(new SetValueAction<string>(new Ref(Property, SettingsInstance), value));
                    OnPropertyChanged(nameof(ValueString));
                }
            }
        }
        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> DropdownValue
        {
            get => _selector;
            set
            {
                if (value != _selector)
                {
                    _selector = value;
                    OnPropertyChanged(nameof(DropdownValue));
                }
            }
        }
        [DataSourceProperty]
        public float MaxValue => SettingAttribute.MaxValue;
        [DataSourceProperty]
        public float MinValue => SettingAttribute.MinValue;
        [DataSourceProperty]
        public string? ValueString => SettingType switch
        {
            SettingType.Int => ((int) Property.GetValue(SettingsInstance)).ToString("0"),
            SettingType.Float => ((float) Property.GetValue(SettingsInstance)).ToString("0.00"),
            SettingType.String => (string) Property.GetValue(SettingsInstance),
            SettingType.Dropdown => DropdownValue.SelectedItem.StringItem,
            _ => ""
        };
        [DataSourceProperty]
        public Action OnHoverAction => OnHover;
        [DataSourceProperty]
        public Action OnHoverEndAction => OnHoverEnd;

        public SettingPropertyVM(SettingPropertyDefinition definition, ModSettingsVM modSettingsView)
        {
            ModSettingsView = modSettingsView;
            SettingPropertyDefinition = definition;

            if (!string.IsNullOrWhiteSpace(SettingAttribute.HintText))
                HintText = $"{Name}: {SettingAttribute.HintText}";

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            switch (SettingType)
            {
                case SettingType.Float:
                    FloatValue = (float) Property.GetValue(SettingsInstance);
                    OnPropertyChanged(nameof(FloatValue));
                    break;
                case SettingType.Int:
                    IntValue = (int) Property.GetValue(SettingsInstance);
                    OnPropertyChanged(nameof(IntValue));
                    break;
                case SettingType.Bool:
                    BoolValue = (bool) Property.GetValue(SettingsInstance);
                    OnPropertyChanged(nameof(BoolValue));
                    break;
                case SettingType.String:
                    StringValue = (string) Property.GetValue(SettingsInstance);
                    OnPropertyChanged(nameof(StringValue));
                    break;
                case SettingType.Dropdown:
                    var dropdownValue = (Dropdown<string>) Property.GetValue(SettingsInstance);
                    DropdownValue = new SelectorVM<SelectorItemVM>(
                        dropdownValue.GetValues().AsEnumerable(),
                        dropdownValue.GetSelectedIndex(), value =>
                        {
                            dropdownValue.SelectValue(value.SelectedItem.StringItem);
                        });
                    OnPropertyChanged(nameof(DropdownValue));
                    break;
            }
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