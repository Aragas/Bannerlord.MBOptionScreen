using System;

namespace ModLib.Definitions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingPropertyAttribute : Attribute
    {
        public string DisplayName { get; private set; } = string.Empty;
        public float MinValue { get; private set; } = 0f;
        public float MaxValue { get; private set; } = 0f;
        public float EditableMinValue { get; private set; } = 0f;
        public float EditableMaxValue { get; private set; } = 0f;
        public string HintText { get; private set; } = string.Empty;

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
            this(displayName, (float)minValue, (float)maxValue, (float)editableMinValue, (float)editableMaxValue, hintText)
        {
        }

        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, string hintText = string.Empty) :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText)
        {
        }

        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, string hintText = string.Empty) :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText)
        {
        }

        public SettingPropertyAttribute(string displayName, string tooltip = string.Empty) : this(displayName, 0f, 0f, tooltip)
        {
        }
    }
}
