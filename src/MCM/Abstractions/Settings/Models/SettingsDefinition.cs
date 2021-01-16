using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Models
{
    public class SettingsDefinition
    {
        public string SettingsId { get; }
        public string DisplayName { get; }
        public List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }

        public SettingsDefinition(string id) { }
        public SettingsDefinition(string id, string displayName, List<SettingsPropertyGroupDefinition> settingsPropertyGroups) { }
        public SettingsDefinition(string id, string displayName, IEnumerable<SettingsPropertyDefinition> settingsProperties) { }
    }
}