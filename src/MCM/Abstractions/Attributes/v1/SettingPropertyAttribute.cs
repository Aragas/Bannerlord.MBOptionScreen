using MCM.Abstractions.Settings.Definitions;

using System;

namespace MCM.Abstractions.Attributes.v1
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SettingPropertyAttribute : BaseSettingPropertyAttribute, IPropertyDefinitionWithMinMax
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
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        public SettingPropertyAttribute(string displayName) : base(displayName)
        {

        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum float value that this property can be set to. This is used for the slider control.</param>
        public SettingPropertyAttribute(string displayName, float minValue, float maxValue) : base(displayName)
        {
            MinValue = (decimal) minValue;
            MaxValue = (decimal) maxValue;
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum int value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum int value that this property can be set to. This is used for the slider control.</param>
        public SettingPropertyAttribute(string displayName, int minValue, int maxValue) : base(displayName)
        {
            MinValue = (decimal) minValue;
            MaxValue = (decimal) maxValue;
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum int value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum int value that this property can be set to. This is used for the slider control.</param>
        public SettingPropertyAttribute(string displayName, decimal minValue, decimal maxValue) : base(displayName)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}