using System;

namespace ModLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SettingPropertyAttribute : Attribute
    {
        public string DisplayName { get; }
        public float MinValue { get; }
        public float MaxValue { get; }
        public float EditableMinValue { get; }
        public float EditableMaxValue { get; }
        public string HintText { get; }

        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, float editableMinValue, float editableMaxValue, string hintText = "")
        {
            DisplayName = displayName;
            MinValue = minValue;
            MaxValue = maxValue;
            EditableMinValue = editableMinValue;
            EditableMaxValue = editableMaxValue;
            HintText = hintText;
        }

        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, int editableMinValue, int editableMaxValue, string hintText = "") :
            this(displayName, (float) minValue, (float) maxValue, (float) editableMinValue, (float) editableMaxValue, hintText) { }

        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, string hintText = "") :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText) { }

        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, string hintText = "") :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText) { }

        public SettingPropertyAttribute(string displayName, string tooltip = "") :
            this(displayName, 0f, 0f, tooltip) { }
    }
}
