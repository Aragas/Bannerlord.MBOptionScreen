using System;

namespace MBOptionScreen.Attributes
{
    public abstract class BaseSettingPropertyAttribute : Attribute
    {
        /// <summary>
        /// The display name of the setting in the settings menu.
        /// </summary>
        public string DisplayName { get; } = "";
        /// <summary>
        /// Require restart of the game if the value is changed.
        /// </summary>
        public int Order { get; } = -1;
        /// <summary>
        /// Require restart of the game if the value is changed.
        /// </summary>
        public bool RequireRestart { get; } = true;
        /// <summary>
        /// The hint text that is displayed at the bottom of the screen when the user hovers over the setting in the settings menu.
        /// </summary>
        public string HintText { get; } = "";

        protected BaseSettingPropertyAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
        {
            DisplayName = displayName;
            Order = order;
            RequireRestart = requireRestart;
            HintText = hintText;
        }
    }
}