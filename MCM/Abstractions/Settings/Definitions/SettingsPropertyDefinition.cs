using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Data;
using MCM.Utils;

using System.Reflection;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Definitions
{
    public class SettingsPropertyDefinition
    {
        public string SettingsId { get; protected set; }

        public PropertyInfo Property { get; protected set; }
        public SettingType SettingType { get; protected set; }

        public TextObject DisplayName { get; protected set; } = new TextObject("");
        public int Order { get; protected set; } = -1;
        public bool RequireRestart { get; protected set; } = true;
        public TextObject HintText { get; protected set; } = new TextObject("");
        public float MaxValue { get; protected set; } = 0f;
        public float MinValue { get; protected set; } = 0f;
        public float EditableMinValue { get; protected set; } = 0f;
        public float EditableMaxValue { get; protected set; } = 0f;
        public int SelectedIndex { get; protected set; } = 0;
        public string ValueFormat { get; protected set; } = "";
        public string GroupName { get; protected set; } = SettingsPropertyGroupDefinition.DefaultGroupName;
        public bool IsMainToggle { get; protected set; } = false;
        public int GroupOrder { get; protected set; } = -1;

        protected SettingsPropertyDefinition() { }
        public SettingsPropertyDefinition(IPropertyDefinitionBase propertyDefinition, IPropertyGroupDefinition propertyGroupDefinition, PropertyInfo property, string settingsId)
        {
            GroupName = propertyGroupDefinition.GroupName;
            IsMainToggle = propertyGroupDefinition.IsMainToggle;
            GroupOrder = propertyGroupDefinition.Order;

            Property = property;
            SettingsId = settingsId;

            if (propertyDefinition is IPropertyDefinitionBase propertyBase)
            {
                DisplayName = new TextObject(propertyBase.DisplayName, null);
                Order = propertyBase.Order;
                RequireRestart = propertyBase.RequireRestart;
                HintText = new TextObject(propertyBase.HintText, null);
            }

            // v1
            if (propertyDefinition is SettingPropertyAttribute settingPropertyAttribute)
            {
                if (Property.PropertyType == typeof(bool))
                    SettingType = SettingType.Bool;
                else if (Property.PropertyType == typeof(int))
                    SettingType = SettingType.Int;
                else if (Property.PropertyType == typeof(float))
                    SettingType = SettingType.Float;
                else if (Property.PropertyType == typeof(string))
                    SettingType = SettingType.String;
                else if (ReflectionUtils.ImplementsOrImplementsEquivalent(Property.PropertyType, typeof(IDropdownProvider)))
                    SettingType = SettingType.Dropdown;

                MinValue = settingPropertyAttribute.MinValue;
                MaxValue = settingPropertyAttribute.MaxValue;
                EditableMinValue = settingPropertyAttribute.MinValue;
                EditableMaxValue = settingPropertyAttribute.MaxValue;
            }

            // v2
            if (propertyDefinition is IPropertyDefinitionBool propertyDefinitionBool)
            {
                SettingType = SettingType.Bool;
            }
            if (propertyDefinition is IPropertyDefinitionFloatingInteger propertyDefinitionFloatingInteger)
            {
                SettingType = SettingType.Float;
                MinValue = propertyDefinitionFloatingInteger.MinValue;
                MaxValue = propertyDefinitionFloatingInteger.MaxValue;
                EditableMinValue = propertyDefinitionFloatingInteger.MinValue;
                EditableMaxValue = propertyDefinitionFloatingInteger.MaxValue;
                ValueFormat = propertyDefinitionFloatingInteger.ValueFormat;
            }
            if (propertyDefinition is IPropertyDefinitionInteger propertyDefinitionInteger)
            {
                SettingType = SettingType.Int;
                MinValue = propertyDefinitionInteger.MinValue;
                MaxValue = propertyDefinitionInteger.MaxValue;
                EditableMinValue = propertyDefinitionInteger.MinValue;
                EditableMaxValue = propertyDefinitionInteger.MaxValue;
                ValueFormat = propertyDefinitionInteger.ValueFormat;
            }
            if (propertyDefinition is IPropertyDefinitionText propertyDefinitionText)
            {
                SettingType = SettingType.String;
            }
            if (propertyDefinition is IPropertyDefinitionDropdown propertyDefinitionDropdown)
            {
                SettingType = SettingType.Dropdown;
                SelectedIndex = propertyDefinitionDropdown.SelectedIndex;
            }
        }

        public override string ToString() => $"[{GroupName}]: {DisplayName}";
    }
}