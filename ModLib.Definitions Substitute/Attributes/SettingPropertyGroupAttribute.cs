using System;

namespace ModLib.Definitions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingPropertyGroupAttribute : Attribute
    {
        public const string DefaultGroupName = "Misc";

        public string GroupName { get; private set; }
        public bool IsMainToggle { get; set; }

        public SettingPropertyGroupAttribute(string groupName, bool isMainToggle = false)
        {
            GroupName = groupName;
            IsMainToggle = isMainToggle;
        }

        public static SettingPropertyGroupAttribute Default => new SettingPropertyGroupAttribute(DefaultGroupName, false);
    }
}
