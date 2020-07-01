using MCM.Abstractions.Settings.Definitions;

using System;

namespace MCM.Abstractions.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyIntegerAttribute : BaseSettingPropertyAttribute,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionWithCustomFormatter
    {
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal MaxValue { get; }
        /// <inheritdoc/>
        public string ValueFormat { get; }
        /// <inheritdoc/>
        public Type? CustomFormatter { get; set; }

        public SettingPropertyIntegerAttribute(string displayName, int minValue, int maxValue, string valueFormat = "0") : base(displayName)
        {
            MinValue = (decimal) minValue;
            MaxValue = (decimal) maxValue;
            ValueFormat = valueFormat;
        }
    }
}