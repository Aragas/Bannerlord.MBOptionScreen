using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Models;
using MCM.Implementation.Settings.Properties;

using System.Collections.Generic;

namespace MCM.Implementation
{
    public abstract class BaseTestGlobalSettings<T> : AttributeGlobalSettings<T> where T : GlobalSettings, new()
    {
        public override string FolderName => "Testing";

        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups()
        {
            var groups = new List<SettingsPropertyGroupDefinition>();
            foreach (var settingProp in new MCMSettingsPropertyDiscoverer().GetProperties(this, Id))
            {
                var group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }
            return groups;
        }
    }
}