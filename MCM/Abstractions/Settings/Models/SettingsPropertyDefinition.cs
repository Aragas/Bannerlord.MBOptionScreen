using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Utils;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Models
{
    public sealed class SettingsPropertyDefinition : ISettingsPropertyDefinition
    {
        public IRef PropertyReference { get; }
        public SettingType SettingType { get; }
        public string DisplayName { get; } = "";
        public int Order { get; } = -1;
        public bool RequireRestart { get; } = true;
        public string HintText { get; } = "";
        public decimal MaxValue { get; }
        public decimal MinValue { get; }
        public decimal EditableMinValue { get; }
        public decimal EditableMaxValue { get; }
        public int SelectedIndex { get; }
        public string ValueFormat { get; } = "";
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public SettingsPropertyDefinition(IPropertyDefinitionBase propertyDefinition, IPropertyGroupDefinition propertyGroupDefinition, IRef propertyReference)
            : this(new []{ propertyDefinition }, propertyGroupDefinition, propertyReference) { }
        public SettingsPropertyDefinition(IEnumerable<IPropertyDefinitionBase> propertyDefinitions, IPropertyGroupDefinition propertyGroupDefinition, IRef propertyReference)
        {
            GroupName = propertyGroupDefinition.GroupName;
            IsMainToggle = propertyGroupDefinition.IsMainToggle;
            GroupOrder = propertyGroupDefinition.GroupOrder;

            PropertyReference = propertyReference;

            if (PropertyReference.Type == typeof(bool))
                SettingType = SettingType.Bool;
            else if (PropertyReference.Type == typeof(int))
                SettingType = SettingType.Int;
            else if (PropertyReference.Type == typeof(float))
                SettingType = SettingType.Float;
            else if (PropertyReference.Type == typeof(string))
                SettingType = SettingType.String;
            else if (SettingsUtils.IsDropdown(PropertyReference.Type))
                SettingType = SettingType.Dropdown;

            foreach (var propertyDefinition in propertyDefinitions)
            {
                if (propertyDefinition is IPropertyDefinitionBase propertyBase)
                {
                    DisplayName = new TextObject(propertyBase.DisplayName).ToString();
                    Order = propertyBase.Order;
                    RequireRestart = propertyBase.RequireRestart;
                    HintText = new TextObject(propertyBase.HintText).ToString();
                }
                if (propertyDefinition is SettingPropertyAttribute settingPropertyAttribute) // v1
                {
                    MinValue = settingPropertyAttribute.MinValue;
                    MaxValue = settingPropertyAttribute.MaxValue;
                    EditableMinValue = settingPropertyAttribute.MinValue;
                    EditableMaxValue = settingPropertyAttribute.MaxValue;
                }
                if (propertyDefinition is IPropertyDefinitionBool propertyDefinitionBool) // v2
                {

                }
                if (propertyDefinition is IPropertyDefinitionWithMinMax propertyDefinitionWithMinMax)
                {
                    MinValue = propertyDefinitionWithMinMax.MinValue;
                    MaxValue = propertyDefinitionWithMinMax.MaxValue;
                    EditableMinValue = propertyDefinitionWithMinMax.MinValue;
                    EditableMaxValue = propertyDefinitionWithMinMax.MaxValue;
                }
                if (propertyDefinition is IPropertyDefinitionWithEditableMinMax propertyDefinitionWithEditableMinMax)
                {
                    EditableMinValue = propertyDefinitionWithEditableMinMax.EditableMinValue;
                    EditableMaxValue = propertyDefinitionWithEditableMinMax.EditableMaxValue;
                }
                if (propertyDefinition is IPropertyDefinitionWithFormat propertyDefinitionWithFormat)
                {
                    ValueFormat = propertyDefinitionWithFormat.ValueFormat;
                }
                if (propertyDefinition is IPropertyDefinitionText propertyDefinitionText)
                {

                }
                if (propertyDefinition is IPropertyDefinitionDropdown propertyDefinitionDropdown)
                {
                    SelectedIndex = propertyDefinitionDropdown.SelectedIndex;
                }
            }
        }

        public override string ToString() => $"[{GroupName}]: {DisplayName}";
    }
}