using MBOptionScreen.Settings;

using System;

namespace MBOptionScreen.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyFloatingIntegerAttribute : BaseSettingPropertyAttribute, IPropertyDefinitionFloatingInteger
    {
        /// <summary>
        /// The minimum value the setting can be set to. Used by the slider control.
        /// </summary>
        public float MinValue { get; } = 0f;
        /// <summary>
        /// The maximum value the setting can be set to. Used by the slider control.
        /// </summary>
        public float MaxValue { get; } = 0f;
        /// <summary>
        /// The format in which the slider's value will be displayed in.
        /// </summary>
        public string ValueFormat { get; } = "0.00";

        public SettingPropertyFloatingIntegerAttribute(string displayName, float minValue, float maxValue, string valueFormat = "0.00") : base(displayName)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            ValueFormat = valueFormat;
        }
    }
}