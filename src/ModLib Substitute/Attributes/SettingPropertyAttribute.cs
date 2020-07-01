using System;

namespace ModLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SettingPropertyAttribute : Attribute
    {
        public string DisplayName { get; } = string.Empty;
        public float MinValue { get; } = 0f;
        public float MaxValue { get; } = 0f;
        public float EditableMinValue { get; } = 0f;
        public float EditableMaxValue { get; } = 0f;
        public string HintText { get; } = string.Empty;

        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, float editableMinValue, float editableMaxValue, string hintText = string.Empty)
        {
            DisplayName = displayName;
            MinValue = minValue;
            MaxValue = maxValue;
            EditableMinValue = editableMinValue;
            EditableMaxValue = editableMaxValue;
            HintText = hintText;
        }

        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, int editableMinValue, int editableMaxValue, string hintText = string.Empty) :
            this(displayName, (float) minValue, (float) maxValue, (float) editableMinValue, (float) editableMaxValue, hintText) { }

        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, string hintText = string.Empty) :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText) { }

        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, string hintText = string.Empty) :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText) { }

        public SettingPropertyAttribute(string displayName, string tooltip = string.Empty) :
            this(displayName, 0f, 0f, tooltip) { }
    }
}
