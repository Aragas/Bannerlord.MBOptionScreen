using System;

namespace ModLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingPropertyAttribute : Attribute
    {
        public string DisplayName { get; private set; } = "";
        public float MinValue { get; private set; } = 0f;
        public float MaxValue { get; private set; } = 0f;
        public float EditableMinValue { get; private set; } = 0f;
        public float EditableMaxValue { get; private set; } = 0f;
        public string HintText { get; private set; } = "";

        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, float editableMinValue, float editableMaxValue, string hintText = "")
        {
            DisplayName = displayName;
            MinValue = minValue;
            MaxValue = maxValue;
            EditableMinValue = editableMinValue;
            EditableMaxValue = editableMaxValue;
            HintText = hintText;
        }

        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, string hintText = "") :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText)
        {
        }

        public SettingPropertyAttribute(string displayName, string tooltip = "") : this(displayName, 0f, 0f, tooltip)
        {

        }
    }
}
