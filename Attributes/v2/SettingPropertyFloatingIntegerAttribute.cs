using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal class SettingPropertyFloatingIntegerAttribute : BaseSettingPropertyAttribute
    {
        /// <summary>
        /// The minimum value the setting can be set to. Used by the slider control.
        /// </summary>
        public float MinValue { get; } = 0f;
        /// <summary>
        /// The maximum value the setting can be set to. Used by the slider control.
        /// </summary>
        public float MaxValue { get; } = 0f;

        public SettingPropertyFloatingIntegerAttribute(string displayName, float minValue, float maxValue, int order = -1, bool requireRestart = true, string hintText = "")
            : base(displayName, order, requireRestart, hintText)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}