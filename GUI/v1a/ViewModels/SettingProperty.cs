using MBOptionScreen.Actions;
using MBOptionScreen.Attributes;
using MBOptionScreen.GUI.v1a.GauntletUI;
using MBOptionScreen.Interfaces;
using MBOptionScreen.Settings;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1a.ViewModels
{
    public class SettingProperty : ViewModel
    {
        private float _floatValue = 0f;
        private int _intValue = 0;
        private bool initializing = false;

        public ModSettingsScreenVM MainView { get; private set; }
        public SettingPropertyDefinition SettingPropertyDefinition { get; }
        public SettingPropertyAttribute SettingAttribute => SettingPropertyDefinition.SettingAttribute;
        public PropertyInfo Property => SettingPropertyDefinition.Property;
        public SettingPropertyGroupAttribute GroupAttribute => SettingPropertyDefinition.GroupAttribute;
        public ISerializableFile SettingsInstance => SettingPropertyDefinition.SettingsInstance;
        public SettingType SettingType => SettingPropertyDefinition.SettingType;
        public SettingPropertyGroup Group { get; set; }
        public UndoRedoStack URS { get; private set; }
        public string HintText { get; private set; }
        public bool SatisfiesSearch
        {
            get
            {
                if (MainView == null || MainView.SearchText == "")
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
        public bool IsValueFieldVisible => !IsBoolVisible;

        [DataSourceProperty]
        public float FloatValue
        {
            get => _floatValue;
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
            get => _intValue;
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
        public float FinalizedFloatValue
        {
            get => 0;
            set
            {
                if (Property.GetValue(SettingsInstance) is float val && val != value && !initializing)
                {
                    URS.Do(new SetValueAction<float>(new Ref(Property, SettingsInstance), (float) Math.Round(value, 2, MidpointRounding.ToEven)));
                }
            }
        }
        [DataSourceProperty]
        public int FinalizedIntValue
        {
            get => 0;
            set
            {
                if (Property.GetValue(SettingsInstance) is int val && val != value && !initializing)
                {
                    URS.Do(new SetValueAction<int>(new Ref(Property, SettingsInstance), value));
                }
            }
        }
        [DataSourceProperty]
        public bool BoolValue
        {
            get => SettingType == SettingType.Bool && Property.GetValue(SettingsInstance) is bool val && val;
            set
            {
                if (SettingType == SettingType.Bool)
                {
                    if (BoolValue != value)
                    {
                        URS.Do(new SetValueAction<bool>(new Ref(Property, SettingsInstance), value));
                        //Property.SetValue(SettingsInstance, value);
                        OnPropertyChanged(nameof(BoolValue));
                    }
                }
            }
        }
        [DataSourceProperty]
        public float MaxValue => SettingAttribute.MaxValue;
        [DataSourceProperty]
        public float MinValue => SettingAttribute.MinValue;
        [DataSourceProperty]
        public string ValueString => SettingType switch
        {
            SettingType.Int => ((int) Property.GetValue(SettingsInstance)).ToString("0"),
            SettingType.Float => ((float) Property.GetValue(SettingsInstance)).ToString("0.00"),
            _ => ""
        };

        [DataSourceProperty]
        public Action OnHoverAction => OnHover;
        [DataSourceProperty]
        public Action OnHoverEndAction => OnHoverEnd;

        public SettingProperty(SettingPropertyDefinition definition, ModSettingsScreenVM mainView)
        {
            MainView = mainView;
            SettingPropertyDefinition = definition;

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            initializing = true;
            if (!string.IsNullOrWhiteSpace(SettingAttribute.HintText))
                HintText = $"{Name}: {SettingAttribute.HintText}";

            if (SettingType == SettingType.Float)
                FloatValue = (float)Property.GetValue(SettingsInstance);
            else if (SettingType == SettingType.Int)
                IntValue = (int)Property.GetValue(SettingsInstance);
            initializing = false;
        }

        internal void AssignUndoRedoStack(UndoRedoStack urs)
        {
            URS = urs;
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
    }
}