using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SettingPropertyIntegerAttribute : BaseSettingPropertyAttribute
    {
        /// <summary>
        /// The minimum value the setting can be set to. Used by the slider control.
        /// </summary>
        public int MinValue { get; } = 0;
        /// <summary>
        /// The maximum value the setting can be set to. Used by the slider control.
        /// </summary>
        public int MaxValue { get; } = 0;

        public SettingPropertyIntegerAttribute(string displayName, int minValue, int maxValue, int order = -1, bool requireRestart = true, string hintText = "")
            : base(displayName, order, requireRestart, hintText)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}