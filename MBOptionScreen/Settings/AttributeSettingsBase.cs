using MBOptionScreen.Utils;

using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.Settings
{
    public abstract class AttributeSettings<T> : SettingsBase<T> where T : SettingsBase, new()
    {
        protected override char SubGroupDelimiter => '/';

        public override List<SettingPropertyGroupDefinition> GetSettingPropertyGroups() => GetUnsortedSettingPropertyGroups()
            .OrderByDescending(x => x.GroupName == SettingPropertyGroupDefinition.DefaultGroupName)
            .ThenByDescending(x => x.Order)
            .ThenByDescending(x => x, new AlphanumComparatorFast())
            .ToList();

        public IEnumerable<SettingPropertyGroupDefinition> GetUnsortedSettingPropertyGroups()
        {
            var groups = new List<SettingPropertyGroupDefinition>();
            foreach (var settingProp in SettingsUtils.GetProperties(this, Id))
            {
                CheckIsValid(settingProp);

                //Find the group that the setting property should belong to. This is the default group if no group is specifically set with the SettingPropertyGroup attribute.
                var group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }
            return groups;
        }
    }
}