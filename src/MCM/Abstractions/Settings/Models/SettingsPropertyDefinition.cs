using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Models
{
    public sealed class SettingsPropertyDefinition : ISettingsPropertyDefinition
    {
        /// <inheritdoc/>
        public string Id { get; } = "";
        /// <inheritdoc/>
        public IRef PropertyReference { get; }
        /// <inheritdoc/>
        public SettingType SettingType { get; }
        /// <inheritdoc/>
        public string DisplayName { get; } = "";
        /// <inheritdoc/>
        public int Order { get; } = -1;
        /// <inheritdoc/>
        public bool RequireRestart { get; } = true;
        /// <inheritdoc/>
        public string HintText { get; } = "";
        /// <inheritdoc/>
        public decimal MaxValue { get; }
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal EditableMinValue { get; }
        /// <inheritdoc/>
        public decimal EditableMaxValue { get; }
        /// <inheritdoc/>
        public int SelectedIndex { get; }
        /// <inheritdoc/>
        public string ValueFormat { get; } = "";
        /// <inheritdoc/>
        public Type? CustomFormatter { get; }
        /// <inheritdoc/>
        public string GroupName { get; }
        /// <inheritdoc/>
        [Obsolete("Will be removed", true)]
        public bool IsMainToggle { get; }
        /// <inheritdoc/>
        public bool IsToggle { get; }
        /// <inheritdoc/>
        public int GroupOrder { get; }
        /// <inheritdoc/>

        public SettingsPropertyDefinition(IPropertyDefinitionBase propertyDefinition, IPropertyGroupDefinition propertyGroupDefinition, IRef propertyReference, char subGroupDelimiter)
            : this(new []{ propertyDefinition }, propertyGroupDefinition, propertyReference, subGroupDelimiter) { }
        public SettingsPropertyDefinition(IEnumerable<IPropertyDefinitionBase> propertyDefinitions, IPropertyGroupDefinition propertyGroupDefinition, IRef propertyReference, char subGroupDelimiter)
        {
            var groups = propertyGroupDefinition.GroupName.Split(subGroupDelimiter);
            GroupName = string.Join(subGroupDelimiter.ToString(), groups.Select(x => new TextObject(x).ToString()));
            GroupOrder = propertyGroupDefinition.GroupOrder;

            PropertyReference = propertyReference;

            if (PropertyReference is PropertyRef propertyRef)
                Id = propertyRef.PropertyInfo.Name;

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
                if (propertyDefinition is IPropertyDefinitionWithCustomFormatter propertyDefinitionWithCustomFormatter)
                {
                    CustomFormatter = propertyDefinitionWithCustomFormatter.CustomFormatter;
                }
                if (propertyDefinition is IPropertyDefinitionWithActionFormat propertyDefinitionWithActionFormat)
                {
                    // TODO:
                }
                if (propertyDefinition is IPropertyDefinitionText propertyDefinitionText)
                {

                }
                if (propertyDefinition is IPropertyDefinitionDropdown propertyDefinitionDropdown)
                {
                    SelectedIndex = propertyDefinitionDropdown.SelectedIndex;
                }
                if (propertyDefinition is IPropertyDefinitionWithId propertyDefinitionWithId)
                {
                    Id = propertyDefinitionWithId.Id;
                }
                if (propertyDefinition is IPropertyDefinitionGroupToggle propertyDefinitionGroupToggle)
                {
                    IsToggle = propertyDefinitionGroupToggle.IsToggle;
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"[{GroupName}]: {DisplayName}";
    }
}