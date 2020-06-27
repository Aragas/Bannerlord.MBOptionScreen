using MCM.Abstractions.Settings.Definitions;

using System;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Attributes
{
    public abstract class BaseSettingPropertyAttribute : Attribute, IPropertyDefinitionBase
    {
        private string _hintText = "";

        /// <summary>
        /// The display name of the setting in the settings menu.
        /// </summary>
        public string DisplayName { get; }
        /// <summary>
        /// Require restart of the game if the value is changed.
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Require restart of the game if the value is changed.
        /// </summary>
        public bool RequireRestart { get; set; }
        /// <summary>
        /// The hint text that is displayed at the bottom of the screen when the user hovers over the setting in the settings menu.
        /// </summary>
        public string HintText { get => _hintText; set => _hintText = new TextObject(value).ToString(); }

        protected BaseSettingPropertyAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
        {
            DisplayName = new TextObject(displayName).ToString();
            Order = order;
            RequireRestart = requireRestart;
            HintText = hintText;
        }
    }
}