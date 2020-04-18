using System.Collections.Generic;

namespace MBOptionScreen.Settings
{
    public class SettingPropertyGroupDefinition
    {
        public const string DefaultGroupName = "Misc";

        public string GroupName { get; }
        public string GroupNameOverride { get; }
        public string DisplayGroupName => string.IsNullOrWhiteSpace(GroupNameOverride) ? GroupName : GroupNameOverride;
        public List<SettingPropertyGroupDefinition> SubGroups { get; } = new List<SettingPropertyGroupDefinition>();
        public List<SettingPropertyDefinition> SettingProperties { get; } = new List<SettingPropertyDefinition>();

        public SettingPropertyGroupDefinition(string groupName, string groupNameOverride = "")
        {
            GroupName = groupName;
            GroupNameOverride = groupNameOverride;
        }

        public void Add(SettingPropertyDefinition settingProp)
        {
            SettingProperties.Add(settingProp);
        }
    }
}