using System;

namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyGroupDefinition
    {
        /// <summary>
        /// The name of the settings group. Includes SubGroup notation if present.
        /// </summary>
        string GroupName { get; }

        /// <summary>
        /// If true, the boolean setting property that this attribute is attached to will be set as the main toggle switch for the entire group. It will not appear in the settings menu, but rather cause a toggle button to appear next to the group's display name in the settings menu.
        /// </summary>
        [Obsolete("Will be removed", true)]
        bool IsMainToggle { get; }

        /// <summary>
        ///
        /// </summary>
        int GroupOrder { get; }
    }
}