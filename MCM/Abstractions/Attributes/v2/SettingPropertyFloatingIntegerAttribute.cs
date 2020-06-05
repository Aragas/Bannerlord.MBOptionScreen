using MCM.Abstractions.Settings.Definitions;

using System;

namespace MCM.Abstractions.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyFloatingIntegerAttribute : BaseSettingPropertyAttribute,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionWithCustomFormatter
    {
        /// <summary>
        /// The minimum value the setting can be set to. Used by the slider control.
        /// </summary>
        public decimal MinValue { get; }
        /// <summary>
        /// The maximum value the setting can be set to. Used by the slider control.
        /// </summary>
        public decimal MaxValue { get; }
        /// <summary>
        /// The format in which the slider's value will be displayed in.
        /// </summary>
        public string ValueFormat { get; }

        public Type? CustomFormatter { get; set; }

        public SettingPropertyFloatingIntegerAttribute(string displayName, float minValue, float maxValue, string valueFormat = "0.00") : base(displayName)
        {
            MinValue = (decimal) minValue;
            MaxValue = (decimal) maxValue;
            ValueFormat = valueFormat;
        }
    }
}