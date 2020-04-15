using MBOptionScreen.Attributes;

using System.Collections.Generic;

namespace MBOptionScreen.Settings
{
    public class SettingPropertyGroupDefinition
    {
        public const string DefaultGroupName = "Misc";

        public SettingPropertyGroupAttribute Attribute { get; set; /* Check set */ }
        public string GroupName => string.IsNullOrWhiteSpace(GroupNameOverride) ? Attribute.GroupName : GroupNameOverride;
        public string GroupNameOverride { get; }
        public List<SettingPropertyDefinition> SettingProperties { get; } = new List<SettingPropertyDefinition>();

        public SettingPropertyGroupDefinition(SettingPropertyGroupAttribute attribute, string groupNameOverride = "")
        {
            Attribute = attribute;
            GroupNameOverride = groupNameOverride;
        }

        public void Add(SettingPropertyDefinition settingProp)
        {
            SettingProperties.Add(settingProp);
        }
    }
}