using MCM.Abstractions.Settings.Models;
using MCM.Utils;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings
{
    public abstract class AttributeGlobalSettings<T> : GlobalSettings<T> where T : GlobalSettings, new()
    {
        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups()
        {
            var groups = new List<SettingsPropertyGroupDefinition>();
            foreach (var settingProp in SettingsUtils.GetProperties(this, Id))
            {
                //Find the group that the setting property should belong to. This is the default group if no group is specifically set with the SettingPropertyGroup attribute.
                var group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }
            return groups;
        }
    }
}