using MBOptionScreen.Attributes;

using System.Reflection;

using TaleWorlds.Localization;

namespace MBOptionScreen.Settings
{
    public class SettingPropertyDefinition
    {
        public string SettingsId { get; }
        public SettingsBase SettingsInstance => SettingsDatabase.GetSettings(SettingsId);
        public PropertyInfo Property { get; }
        public SettingType SettingType { get; }

        public TextObject DisplayName { get; } = new TextObject("");
        public int Order { get; } = -1;
        public bool RequireRestart { get; } = true;
        public TextObject HintText { get; } = new TextObject("");
        public float MaxValue { get; } = 0f;
        public float MinValue { get; } = 0f;
        public float EditableMinValue { get; } = 0f;
        public float EditableMaxValue { get; } = 0f;
        public string GroupName { get; } = SettingPropertyGroupDefinition.DefaultGroupName;
        public bool IsMainToggle { get; } = false;
        public string ValueFormat { get; } = "";

        public SettingPropertyDefinition(BaseSettingPropertyAttribute settingAttribute, SettingPropertyGroupAttribute groupAttribute, PropertyInfo property, string settingsId)
        {
            GroupName = groupAttribute.GroupName;
            IsMainToggle = groupAttribute.IsMainToggle;

            Property = property;
            SettingsId = settingsId;

            if (settingAttribute is BaseSettingPropertyAttribute baseSettingPropertyAttribute)
            {
                DisplayName = new TextObject(baseSettingPropertyAttribute.DisplayName, null);
                Order = baseSettingPropertyAttribute.Order;
                RequireRestart = baseSettingPropertyAttribute.RequireRestart;
                HintText = new TextObject(baseSettingPropertyAttribute.HintText, null);
            }

            // v1
            if (settingAttribute is SettingPropertyAttribute settingPropertyAttribute)
            {
                if (Property.PropertyType == typeof(bool))
                    SettingType = SettingType.Bool;
                else if (Property.PropertyType == typeof(int))
                    SettingType = SettingType.Int;
                else if (Property.PropertyType == typeof(float))
                    SettingType = SettingType.Float;
                else if (Property.PropertyType == typeof(string))
                    SettingType = SettingType.String;
                else if (Property.PropertyType == typeof(Dropdown) || Property.PropertyType == typeof(Dropdown<string>))
                    SettingType = SettingType.Dropdown;
                else
                    ;

                MinValue = settingPropertyAttribute.MinValue;
                MaxValue = settingPropertyAttribute.MaxValue;
                EditableMinValue = settingPropertyAttribute.EditableMinValue;
                EditableMaxValue = settingPropertyAttribute.EditableMaxValue;
            }

            // v2
            if (settingAttribute is SettingPropertyBoolAttribute settingPropertyBoolAttribute)
            {
                SettingType = SettingType.Bool;
            }
            if (settingAttribute is SettingPropertyFloatingIntegerAttribute settingPropertyFloatingIntegerAttribute)
            {
                SettingType = SettingType.Float;
                MinValue = settingPropertyFloatingIntegerAttribute.MinValue;
                MaxValue = settingPropertyFloatingIntegerAttribute.MaxValue;
                EditableMinValue = settingPropertyFloatingIntegerAttribute.MinValue;
                EditableMaxValue = settingPropertyFloatingIntegerAttribute.MaxValue;
                ValueFormat = settingPropertyFloatingIntegerAttribute.ValueFormat;
            }
            if (settingAttribute is SettingPropertyIntegerAttribute settingPropertyIntegerAttribute)
            {
                SettingType = SettingType.Int;
                MinValue = settingPropertyIntegerAttribute.MinValue;
                MaxValue = settingPropertyIntegerAttribute.MaxValue;
                EditableMinValue = settingPropertyIntegerAttribute.MinValue;
                EditableMaxValue = settingPropertyIntegerAttribute.MaxValue;
                ValueFormat = settingPropertyIntegerAttribute.ValueFormat;
            }
            if (settingAttribute is SettingPropertyTextAttribute settingPropertyTextAttribute)
            {
                SettingType = SettingType.String;
            }
            if (settingAttribute is SettingPropertyDropdownAttribute settingPropertyDropdownAttribute)
            {
                SettingType = SettingType.Dropdown;
            }
        }
    }
}