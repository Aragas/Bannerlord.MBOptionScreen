using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingPropertyAttribute : Attribute
    {
        /// <summary>
        /// The display name of the setting in the settings menu.
        /// </summary>
        public string DisplayName { get; } = "";
        /// <summary>
        /// Default value of the property.
        /// </summary>
        public string DefaultStringValue { get; } = "";
        /// <summary>
        /// The minimum value the setting can be set to. Used by the slider control.
        /// </summary>
        public float MinValue { get; } = 0f;
        /// <summary>
        /// The maximum value the setting can be set to. Used by the slider control.
        /// </summary>
        public float MaxValue { get; } = 0f;
        /// <summary>
        /// The absolute minimum value that this setting can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.
        /// </summary>
        public float EditableMinValue { get; } = 0f;
        /// <summary>
        /// The absolute maximum value that this setting can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.
        /// </summary>
        public float EditableMaxValue { get; } = 0f;
        /// <summary>
        /// Require restart of the game if the value is changed.
        /// </summary>
        public bool RequireRestart { get; } = true;
        /// <summary>
        /// The hint text that is displayed at the bottom of the screen when the user hovers over the setting in the settings menu.
        /// </summary>
        public string HintText { get; } = "";

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="defaultStringValue">Default value of the property.</param>
        /// <param name="minValue">The minimum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="editableMinValue">The absolute minimum float value that this property can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.</param>
        /// <param name="editableMaxValue">The absolute maximum float value that this property can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.</param>
        /// <param name="requireRestart">Optional: Will force to ask the user to restart the game if is set to true.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, string defaultStringValue, float minValue, float maxValue, float editableMinValue, float editableMaxValue, bool requireRestart = true, string hintText = "")
        {
            DisplayName = displayName;
            DefaultStringValue = defaultStringValue;
            MinValue = minValue;
            MaxValue = maxValue;
            EditableMinValue = editableMinValue;
            EditableMaxValue = editableMaxValue;
            RequireRestart = requireRestart;
            HintText = hintText;
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="editableMinValue">The absolute minimum float value that this property can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.</param>
        /// <param name="editableMaxValue">The absolute maximum float value that this property can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.</param>
        /// <param name="requireRestart">Optional: Will force to ask the user to restart the game if is set to true.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, float editableMinValue, float editableMaxValue, bool requireRestart = true, string hintText = "")
        {
            DisplayName = displayName;
            MinValue = minValue;
            MaxValue = maxValue;
            EditableMinValue = editableMinValue;
            EditableMaxValue = editableMaxValue;
            RequireRestart = requireRestart;
            HintText = hintText;
        }


        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="defaultStringValue">Default value of the property.</param>
        /// <param name="requireRestart">Optional: Will force to ask the user to restart the game if is set to true.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, string defaultStringValue, bool requireRestart = true, string hintText = "")
        {
            DisplayName = displayName;
            DefaultStringValue = defaultStringValue;
            RequireRestart = requireRestart;
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
        /// <param name="requireRestart">Optional: Will force to ask the user to restart the game if is set to true.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, int editableMinValue, int editableMaxValue, bool requireRestart = true, string hintText = "") :
            this(displayName, (float)minValue, (float)maxValue, (float)editableMinValue, (float)editableMaxValue, requireRestart, hintText)
        {
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum float value that this property can be set to. This is used for the slider control.</param>
        /// <param name="requireRestart">Optional: Will force to ask the user to restart the game if is set to true.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, float minValue, float maxValue, bool requireRestart = true, string hintText = "") :
            this(displayName, minValue, maxValue, minValue, maxValue, requireRestart, hintText)
        {
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="minValue">The minimum int value that this property can be set to. This is used for the slider control.</param>
        /// <param name="maxValue">The maximum int value that this property can be set to. This is used for the slider control.</param>
        /// <param name="requireRestart">Optional: Will force to ask the user to restart the game if is set to true.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, int minValue, int maxValue, bool requireRestart = true, string hintText = "") :
            this(displayName, minValue, maxValue, minValue, maxValue, requireRestart, hintText)
        {
        }

        /// <summary>
        /// Tells the Settings system that this property should be used for the settings menu.
        /// </summary>
        /// <param name="displayName">The name to be displayed in the settings menu for this property.</param>
        /// <param name="requireRestart">Optional: Will force to ask the user to restart the game if is set to true.</param>
        /// <param name="hintText">Optional: Hint text that is displayed at the bottom of the screen when the user hovers the mouse over this property. Leave blank to have no hint.</param>
        public SettingPropertyAttribute(string displayName, bool requireRestart = true, string hintText = "") :
            this(displayName, 0f, 0f, requireRestart, hintText)
        {
        }

    }
}
