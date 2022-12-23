using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Wrapper;
using MCM.Common;

using System;
using System.Collections.Generic;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class SettingsPropertyDefinition : ISettingsPropertyDefinition
    {
        /// <inheritdoc/>
        public string Id { get; } = string.Empty;
        /// <inheritdoc/>
        public IRef PropertyReference { get; }
        /// <inheritdoc/>
        public SettingType SettingType { get; }
        /// <inheritdoc/>
        public string DisplayName { get; } = string.Empty;
        /// <inheritdoc/>
        public int Order { get; } = -1;
        /// <inheritdoc/>
        public bool RequireRestart { get; } = true;
        /// <inheritdoc/>
        public string HintText { get; } = string.Empty;
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
        public string ValueFormat { get; } = string.Empty;
        /// <inheritdoc/>
        public Type? CustomFormatter { get; }
        /// <inheritdoc/>
        public string GroupName { get; }
        /// <inheritdoc/>
        public bool IsToggle { get; }
        /// <inheritdoc/>
        public int GroupOrder { get; }
        private char SubGroupDelimiter { get; }
        /// <inheritdoc/>
        public string Content { get; } = string.Empty;

        public SettingsPropertyDefinition(IPropertyDefinitionBase propertyDefinition, IPropertyGroupDefinition propertyGroupDefinition, IRef propertyReference, char subGroupDelimiter)
            : this(new[] { propertyDefinition }, propertyGroupDefinition, propertyReference, subGroupDelimiter) { }
        public SettingsPropertyDefinition(IEnumerable<IPropertyDefinitionBase> propertyDefinitions, IPropertyGroupDefinition propertyGroupDefinition, IRef propertyReference, char subGroupDelimiter)
        {
            SubGroupDelimiter = subGroupDelimiter;

            var groups = propertyGroupDefinition.GroupName.Split(SubGroupDelimiter);
            GroupName = string.Join(SubGroupDelimiter.ToString(), groups);
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
            else if (SettingsUtils.IsForGenericDropdown(PropertyReference.Type))
                SettingType = SettingType.Dropdown;
            else if (PropertyReference.Type == typeof(Action))
                SettingType = SettingType.Button;

            foreach (var propertyDefinition in propertyDefinitions)
            {
                if (propertyDefinition is { } propertyBase)
                {
                    DisplayName = propertyBase.DisplayName;
                    Order = propertyBase.Order;
                    RequireRestart = propertyBase.RequireRestart;
                    HintText = propertyBase.HintText;
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
                    // Do nothing
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
                    // Do nothing
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
                if (propertyDefinition is IPropertyDefinitionButton propertyDefinitionButton)
                {
                    Content = propertyDefinitionButton.Content;
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"[{GroupName}]: {DisplayName}";

        public SettingsPropertyDefinition Clone(bool keepRefs = true)
        {
            var localPropValue = PropertyReference.Value;
            var value = keepRefs ? PropertyReference : new StorageRef(localPropValue is ICloneable cloneable ? cloneable.Clone() : localPropValue);
            return new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(this), new PropertyGroupDefinitionWrapper(this), value, SubGroupDelimiter);
        }
    }
}