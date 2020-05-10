﻿using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Models;

using System;

namespace MCM.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SettingPropertyGroupAttribute : Attribute, IPropertyGroupDefinition
    {
        /// <summary>
        /// The default group used for settings that don't have a group explicitly set.
        /// </summary>
        public static IPropertyGroupDefinition Default => new SettingPropertyGroupAttribute(SettingsPropertyGroupDefinition.DefaultGroupName);

        /// <summary>
        /// The name of the settings group. Includes SubGroup notation if present.
        /// </summary>
        public string GroupName { get; }
        /// <summary>
        /// If true, the boolean setting property that this attribute is attached to will be set as the main toggle switch for the entire group. It will not appear in the settings menu, but rather cause a toggle button to appear next to the group's display name in the settings menu.
        /// </summary>
        public bool IsMainToggle { get; set; }
        /// <summary>
        ///
        /// </summary>
        public int GroupOrder { get; set; }

        /// <summary>
        /// Tells the settings menu that this setting property should be in a group. All settings will automatically be grouped together if they have a SettingPropertyGroupAttribute with the same GroupName.
        /// </summary>
        /// <param name="groupName">The name of the setting group. Groups can set to be sub groups by separating the parent group's name and this group's name with a '/'. For example, "Group 1/Group 2" will name this group "Group 2" and place it as a sub group in "Group 1".</param>
        public SettingPropertyGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}