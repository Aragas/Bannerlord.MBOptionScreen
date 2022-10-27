using System.Collections.Generic;

namespace MCM.Abstractions
{
    public class SettingsDefinition
    {
        public string SettingsId { get; }
        public string DisplayName { get; }
        public List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }

        public SettingsDefinition(string id)
        {
            SettingsId = id;

            var settings = BaseSettingsProvider.Instance?.GetSettings(id);
            DisplayName = settings?.DisplayName ?? "ERROR";
            SettingPropertyGroups = settings?.GetSettingPropertyGroups() ?? new List<SettingsPropertyGroupDefinition>();
        }

        public SettingsDefinition(string id, string displayName, List<SettingsPropertyGroupDefinition> settingsPropertyGroups)
        {
            SettingsId = id;
            DisplayName = displayName;
            SettingPropertyGroups = settingsPropertyGroups;
        }

        public SettingsDefinition(string id, string displayName, IEnumerable<SettingsPropertyDefinition> settingsProperties)
        {
            SettingsId = id;
            DisplayName = displayName;
            SettingPropertyGroups = SettingsUtils.GetSettingsPropertyGroups('/', settingsProperties);
        }
    }
}