using MCM.Abstractions.Settings.Providers;
using MCM.Utils;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Models
{
    public class SettingsDefinition
    {
        public string SettingsId { get; }
        public string DisplayName { get; }
        public List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }

        public SettingsDefinition(string id)
        {
            SettingsId = id;

            var settings = BaseSettingsProvider.Instance.GetSettings(id);
            DisplayName = settings?.DisplayName ?? "ERROR";
            SettingPropertyGroups = settings?.GetSettingPropertyGroups() ?? new List<SettingsPropertyGroupDefinition>();
        }

        public SettingsDefinition(string id, string displayName, List<SettingsPropertyGroupDefinition> settingsPropertyGroups)
        {
            SettingsId = id;
            DisplayName = displayName;
            SettingPropertyGroups = settingsPropertyGroups;
        }

        public SettingsDefinition(string id, string displayName, List<SettingsPropertyDefinition> settingsProperties)
        {
            SettingsId = id;
            DisplayName = displayName;
            var groups = new List<SettingsPropertyGroupDefinition>();
            foreach (var settingProp in settingsProperties)
            {
                // TODO:
                //Find the group that the setting property should belong to. This is the default group if no group is specifically set with the SettingPropertyGroup attribute.
                var group = SettingsUtils.GetGroupFor('/', settingProp, groups);
                group.Add(settingProp);
            }
            SettingPropertyGroups = groups;
        }
    }
}