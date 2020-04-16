using MBOptionScreen.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen.Settings
{
    public abstract class AttributeSettings<T> : SettingsBase<T> where T : SettingsBase, new()
    {
        protected override char SubGroupDelimiter => '/';

        public override List<SettingPropertyGroupDefinition> GetSettingPropertyGroups()
        {
            var groups = new List<SettingPropertyGroupDefinition>();

            var propList = (from p in GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                let propAttr = p.GetCustomAttribute<SettingPropertyAttribute>(true)
                let groupAttr = p.GetCustomAttribute<SettingPropertyGroupAttribute>(true)
                where propAttr != null
                let groupAttrToAdd = groupAttr ?? SettingPropertyGroupAttribute.Default
                select new SettingPropertyDefinition(propAttr, groupAttrToAdd, p, Id)).ToList();

            //Loop through each property
            foreach (var settingProp in propList)
            {
                //First check that the setting property is set up properly.
                CheckIsValid(settingProp);
                //Find the group that the setting property should belong to. This is the default group if no group is specifically set with the SettingPropertyGroup attribute.
                var group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }

            //If there is more than one group in the list, remove the misc group so that it can be added to the bottom of the list after sorting.
            var miscGroup = GetGroupFor(SettingPropertyGroupDefinition.DefaultGroupName, groups);
            if (miscGroup != null && groups.Count > 1)
                groups.Remove(miscGroup);
            else
                miscGroup = null;

            if (miscGroup != null)
                groups.Add(miscGroup);

            return groups;
        }
    }
}