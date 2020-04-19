using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MBOptionScreen.Settings
{
    public class SettingPropertyGroupDefinition
    {
        public const string DefaultGroupName = "Misc";

        private TextObject _groupName;
        private TextObject _groupNameOverride;

        public string GroupName { get; }
        public TextObject DisplayGroupName => _groupNameOverride.Length > 0 ? _groupNameOverride : _groupName;
        public List<SettingPropertyGroupDefinition> SubGroups { get; } = new List<SettingPropertyGroupDefinition>();
        public List<SettingPropertyDefinition> SettingProperties { get; } = new List<SettingPropertyDefinition>();

        public SettingPropertyGroupDefinition(string groupName, string groupNameOverride = "")
        {
            _groupName = new TextObject(groupName, null);
            _groupNameOverride = new TextObject(groupNameOverride, null);
            GroupName = string.IsNullOrWhiteSpace(groupNameOverride) ? groupName : groupNameOverride;
        }

        public void Add(SettingPropertyDefinition settingProp)
        {
            SettingProperties.Add(settingProp);
        }
    }
}