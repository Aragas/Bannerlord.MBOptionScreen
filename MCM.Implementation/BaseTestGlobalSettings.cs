#if DEBUG
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Models;
using MCM.Implementation.Settings.Properties;
using MCM.Utils;

using System.Collections.Generic;

namespace MCM.Implementation
{
    public abstract class BaseTestGlobalSettings<T> : AttributeGlobalSettings<T> where T : GlobalSettings, new()
    {
        public override string FolderName => "Testing";

        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups()
        {
            //var Discoverer = new AttributeSettingsPropertyDiscoverer();
            var groups = new List<SettingsPropertyGroupDefinition>();
            if (Discoverer == null)
                return groups;

            foreach (var settingProp in Discoverer.GetProperties(this))
            {
                var group = SettingsUtils.GetGroupFor(SubGroupDelimiter, settingProp, groups);
                group.Add(settingProp);
            }
            return groups;
        }
    }
}
#endif