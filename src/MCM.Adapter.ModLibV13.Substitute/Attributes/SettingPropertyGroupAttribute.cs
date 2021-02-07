using System;

namespace ModLib.Definitions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SettingPropertyGroupAttribute : Attribute
    {
        public const string DefaultGroupName = "Misc";

        public string GroupName { get; }
        public bool IsMainToggle { get; set; }

        public SettingPropertyGroupAttribute(string groupName, bool isMainToggle = false)
        {
            GroupName = groupName;
            IsMainToggle = isMainToggle;
        }

        public static SettingPropertyGroupAttribute Default => new(DefaultGroupName, false);
    }
}
