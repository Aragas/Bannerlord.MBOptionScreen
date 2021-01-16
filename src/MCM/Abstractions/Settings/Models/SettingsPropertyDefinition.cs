using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;

using System;
using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Models
{
    public sealed class SettingsPropertyDefinition : ISettingsPropertyDefinition
    {
        public string Id { get; }
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
        public Type? CustomFormatter { get; }
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public SettingsPropertyDefinition(IPropertyDefinitionBase propertyDefinition, IPropertyGroupDefinition propertyGroupDefinition, IRef propertyReference, char subGroupDelimiter)
            : this(new []{ propertyDefinition }, propertyGroupDefinition, propertyReference, subGroupDelimiter) { }
        public SettingsPropertyDefinition(IEnumerable<IPropertyDefinitionBase> propertyDefinitions, IPropertyGroupDefinition propertyGroupDefinition, IRef propertyReference, char subGroupDelimiter) { }
    }
}