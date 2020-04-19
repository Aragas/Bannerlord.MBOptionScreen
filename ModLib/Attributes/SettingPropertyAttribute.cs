using System;

namespace ModLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingPropertyAttribute : Attribute
    {
        /// <summary>
        /// The display name of the setting in the settings menu.
        /// </summary>
        public string DisplayName { get; private set; } = "";
        /// <summary>
        /// The minimum value the setting can be set to. Used by the slider control.
        /// </summary>
        public float MinValue { get; private set; } = 0f;
        /// <summary>
        /// The maximum value the setting can be set to. Used by the slider control.
        /// </summary>
        public float MaxValue { get; private set; } = 0f;
        /// <summary>
        /// The absolute minimum value that this setting can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.
        /// </summary>
        public float EditableMinValue { get; private set; } = 0f;
        /// <summary>
        /// The absolute maximum value that this setting can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.
        /// </summary>
        public float EditableMaxValue { get; private set; } = 0f;
        /// <summary>
        /// The hint text that is displayed at the bottom of the screen when the user hovers over the setting in the settings menu.
        /// </summary>
        public string HintText { get; private set; } = "";

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="editableMinValue">The absolute minimum float value that this property can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.</param>
        /// <param name="editableMaxValue">The absolute maximum float value that this property can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, float editableMinValue, float editableMaxValue, string hintText = "")
        {
            DisplayName = displayName;
            MinValue = minValue;
            MaxValue = maxValue;
            EditableMinValue = editableMinValue;
            EditableMaxValue = editableMaxValue;
            HintText = hintText;
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum int value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum int value that this property can be set to. This is used for the slider control.</param>
        /// <param name="editableMinValue">The absolute minimum int value that this property can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.</param>
        /// <param name="editableMaxValue">The absolute maximum int value that this property can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, int editableMinValue, int editableMaxValue, string hintText = "") :
            this(displayName, (float)minValue, (float)maxValue, (float)editableMinValue, (float)editableMaxValue, hintText)
        {
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, string hintText = "") :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText)
        {
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum int value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum int value that this property can be set to. This is used for the slider control.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, string hintText = "") :
            this(displayName, minValue, maxValue, minValue, maxValue, hintText)
        {
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="tooltip">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, string tooltip = "") : this(displayName, 0f, 0f, tooltip)
        {
        }
    }
}
