using System;
using System.Reflection;
using MBOptionScreen.Attributes;
using MBOptionScreen.Interfaces;
using MBOptionScreen.Settings;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1.ViewModels
{
    public class SettingProperty : ViewModel
    {
        private float _floatValue = 0f;
        private int _intValue = 0;
        private bool initialising = false;

        public SettingPropertyDefinition SettingPropertyDefinition { get; }
        public SettingPropertyAttribute SettingAttribute => SettingPropertyDefinition.SettingAttribute;
        public PropertyInfo Property => SettingPropertyDefinition.Property;
        public SettingPropertyGroupAttribute GroupAttribute => SettingPropertyDefinition.GroupAttribute;
        public ISerializableFile SettingsInstance => SettingPropertyDefinition.SettingsInstance;
        public SettingType SettingType => SettingPropertyDefinition.SettingType;
        public SettingPropertyGroup Group { get; set; }
        public UndoRedoStack URS { get; private set; }
        public ModSettingsScreenVM ScreenVM { get; private set; }
        public string HintText { get; private set; }
        public bool SatisfiesSearch
        {
            get
            {
                if (ScreenVM == null || ScreenVM.SearchText == "")
                    return true;

                return Name.ToLower().Contains(ScreenVM.SearchText.ToLower());
            }
        }

        [DataSourceProperty]
        public string Name => SettingAttribute.DisplayName;

        [DataSourceProperty]
        public bool IsIntVisible => SettingType == SettingType.Int;
        [DataSourceProperty]
        public bool IsFloatVisible => SettingType == SettingType.Float;
        [DataSourceProperty]
        public bool IsBoolVisible { get => SettingType == SettingType.Bool; set { } }
        [DataSourceProperty]
        public bool IsEnabled
        {
            get
            {
                if (Group == null)
                    return true;
                return Group.GroupToggle;
            }
        }
        [DataSourceProperty]
        public bool IsSettingVisible
        {
            get
            {
                if (Group != null && GroupAttribute != null && GroupAttribute.IsMainToggle)
                    return false;
                else if (!Group.GroupToggle)
                    return false;
                else if (!Group.IsExpanded)
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
            get
            {
                return _floatValue;
            }
            set
            {
                if (SettingType == SettingType.Float && _floatValue != value)
                {
                    _floatValue = (float)Math.Round((double)value, 2, MidpointRounding.ToEven);
                    OnPropertyChanged();
                    OnPropertyChanged("ValueString");
                }
            }
        }
        [DataSourceProperty]
        public int IntValue
        {
            get
            {
                return _intValue;
            }
            set
            {
                if (SettingType == SettingType.Int)
                {
                    _intValue = value;
                    OnPropertyChanged();
                    OnPropertyChanged("ValueString");
                }
            }
        }
        [DataSourceProperty]
        public float FinalisedFloatValue
        {
            get => 0;
            set
            {
                if ((float)Property.GetValue(SettingsInstance) != value && !initialising)
                {
                    URS.Do(new SetValueAction<float>(new Ref(Property, SettingsInstance), (float)Math.Round((double)value, 2, MidpointRounding.ToEven)));
                }
            }
        }
        [DataSourceProperty]
        public int FinalisedIntValue
        {
            get => 0;
            set
            {
                if ((int)Property.GetValue(SettingsInstance) != value && !initialising)
                {
                    URS.Do(new SetValueAction<int>(new Ref(Property, SettingsInstance), value));
                }
            }
        }
        [DataSourceProperty]
        public bool BoolValue
        {
            get
            {
                if (SettingType == SettingType.Bool)
                    return (bool)Property.GetValue(SettingsInstance);
                else
                    return false;
            }
            set
            {
                if (SettingType == SettingType.Bool)
                {
                    if (BoolValue != value)
                    {
                        URS.Do(new SetValueAction<bool>(new Ref(Property, SettingsInstance), value));
                        OnPropertyChanged();
                    }
                }
            }
        }
        [DataSourceProperty]
        public float MaxValue => SettingAttribute.MaxValue;
        [DataSourceProperty]
        public float MinValue => SettingAttribute.MinValue;
        [DataSourceProperty]
        public string ValueString
        {
            get
            {
                if (SettingType == SettingType.Int)
                    return IntValue.ToString();
                else if (SettingType == SettingType.Float)
                    return FloatValue.ToString("0.00");
                else
                    return "";
            }
        }

        public SettingProperty(SettingPropertyDefinition definition)
        {
            SettingPropertyDefinition = definition;

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            initialising = true;
            if (!string.IsNullOrWhiteSpace(SettingAttribute.HintText))
                HintText = $"{Name}: {SettingAttribute.HintText}";

            if (SettingType == SettingType.Float)
                FloatValue = (float)Property.GetValue(SettingsInstance);
            else if (SettingType == SettingType.Int)
                IntValue = (int)Property.GetValue(SettingsInstance);
            initialising = false;
        }

        public void AssignUndoRedoStack(UndoRedoStack urs)
        {
            URS = urs;
        }

        public void SetScreenVM(ModSettingsScreenVM screenVM)
        {
            ScreenVM = screenVM;
        }

        private void OnHover()
        {
            if (ScreenVM != null)
                ScreenVM.HintText = HintText;
        }

        private void OnHoverEnd()
        {
            if (ScreenVM != null)
                ScreenVM.HintText = "";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
