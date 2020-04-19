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
        /// <summary>
        /// The format in which the slider's value will be displayed in.
        /// </summary>
        public string ValueFormat { get; } = "";

        public SettingPropertyIntegerAttribute(string displayName, int minValue, int maxValue, int order = -1, bool requireRestart = true, string hintText = "", string valueFormat = "0")
            : base(displayName, order, requireRestart, hintText)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            ValueFormat = valueFormat;
        }
    }
}