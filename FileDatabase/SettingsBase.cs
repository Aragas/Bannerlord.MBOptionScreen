using ModLib.Attributes;
using ModLib.GUI.ViewModels;
using ModLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;

namespace ModLib
{
    public abstract class SettingsBase : ISerialisableFile, ISubFolder
    {
        private const char SubGroupDelimiter = '/';

        /// <summary>
        /// Unique identifier used to store the settings instance in the settings database and to save to file. Make sure this is unique to your mod.
        /// </summary>
        public abstract string ID { get; set; }
        /// <summary>
        /// The folder name of your mod's 'Modules' folder. Should be identical.
        /// </summary>
        public abstract string ModuleFolderName { get; }
        /// <summary>
        /// The name of your mod. This is used in the mods list in the settings menu.
        /// </summary>
        public abstract string ModName { get; }
        /// <summary>
        /// If you want this settings file stored inside a subfolder, set this to the name of the subfolder.
        /// </summary>
        public virtual string SubFolder => "";

        internal List<SettingPropertyGroup> GetSettingPropertyGroups()
        {
            var groups = new List<SettingPropertyGroup>();
            // Find all the properties in the settings instance which have the SettingProperty attribute.
            var propList = (from p in GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            let propAttr = p.GetCustomAttribute<SettingPropertyAttribute>(true)
                            let groupAttr = p.GetCustomAttribute<SettingPropertyGroupAttribute>(true)
                            where propAttr != null
                            let groupAttrToAdd = groupAttr == null ? SettingPropertyGroupAttribute.Default : groupAttr
                            select new SettingProperty(propAttr, groupAttrToAdd, p, this)).ToList();

            //Loop through each property
            foreach (var settingProp in propList)
            {
                //First check that the setting property is set up properly.
                CheckIsValid(settingProp);
                //Find the group that the setting property should belong to. This is the default group if no group is specifically set with the SettingPropertyGroup attribute.
                SettingPropertyGroup group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }

            //If there is more than one group in the list, remove the misc group so that it can be added to the bottom of the list after sorting.
            SettingPropertyGroup miscGroup = GetGroupFor(SettingPropertyGroup.DefaultGroupName, groups);
            if (miscGroup != null && groups.Count > 1)
                groups.Remove(miscGroup);
            else
                miscGroup = null;

            //Sort the list of groups alphabetically.
            groups.Sort((x, y) => x.GroupName.CompareTo(y.GroupName));
            if (miscGroup != null)
                groups.Add(miscGroup);

            foreach (var group in groups)
                group.SetParentGroup(null);

            return groups;
        }

        private SettingPropertyGroup GetGroupFor(SettingProperty sp, ICollection<SettingPropertyGroup> groupsList)
        {
            //If the setting somehow doesn't have a group attribute, throw an error.
            if (sp.GroupAttribute == null)
                throw new Exception($"SettingProperty {sp.Name} has null GroupAttribute");

            SettingPropertyGroup group;
            //Check if the intended group is a sub group
            if (sp.GroupAttribute.GroupName.Contains(SubGroupDelimiter))
            {
                //Intended group is a sub group. Must find it. First get the top group.
                string truncatedGroupName;
                string topGroupName = GetTopGroupName(sp.GroupAttribute.GroupName, out truncatedGroupName);
                SettingPropertyGroup topGroup = groupsList.GetGroup(topGroupName);
                if (topGroup == null)
                {
                    topGroup = new SettingPropertyGroup(sp.GroupAttribute, topGroupName);
                    groupsList.Add(topGroup);
                }
                //Find the sub group
                group = GetGroupForRecursive(truncatedGroupName, topGroup.SettingPropertyGroups, sp);
            }
            else
            {
                //Group is not a subgroup, can find it in the main list of groups.
                group = groupsList.GetGroup(sp.GroupAttribute.GroupName);
                if (group == null)
                {
                    group = new SettingPropertyGroup(sp.GroupAttribute);
                    groupsList.Add(group);
                }
            }
            return group;
        }

        private SettingPropertyGroup GetGroupFor(string groupName, ICollection<SettingPropertyGroup> groupsList)
        {
            return groupsList.GetGroup(groupName);
        }

        private SettingPropertyGroup GetGroupForRecursive(string groupName, ICollection<SettingPropertyGroup> groupsList, SettingProperty sp)
        {
            if (groupName.Contains(SubGroupDelimiter))
            {
                //Need to go deeper
                string truncatedGroupName;
                string topGroupName = GetTopGroupName(groupName, out truncatedGroupName);
                SettingPropertyGroup topGroup = GetGroupFor(topGroupName, groupsList);
                if (topGroup == null)
                {
                    topGroup = new SettingPropertyGroup(sp.GroupAttribute, topGroupName);
                    groupsList.Add(topGroup);
                }
                return GetGroupForRecursive(truncatedGroupName, topGroup.SettingPropertyGroups, sp);
            }
            else
            {
                //Reached the bottom level, can return the final group.
                SettingPropertyGroup group = groupsList.GetGroup(groupName);
                if (group == null)
                {
                    group = new SettingPropertyGroup(sp.GroupAttribute, groupName);
                    groupsList.Add(group);
                }
                return group;
            }
        }

        private string GetTopGroupName(string groupName, out string truncatedGroupName)
        {
            int index = groupName.IndexOf(SubGroupDelimiter);
            string topGroupName = groupName.Substring(0, index);

            truncatedGroupName = groupName.Remove(0, index + 1);
            return topGroupName;
        }

        private void CheckIsValid(SettingProperty prop)
        {
            if (!prop.Property.CanRead)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a getter.");
            if (!prop.Property.CanWrite)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a setter.");

            if (prop.SettingType == SettingType.Int || prop.SettingType == SettingType.Float)
            {
                if (prop.MinValue == prop.MaxValue)
                    throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} is a numeric type but the MinValue and MaxValue are the same.");
                if (prop.GroupAttribute != null && prop.GroupAttribute.IsMainToggle)
                    throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} is marked as the main toggle for the group but is a numeric type. The main toggle must be a boolean type.");
            }
        }
    }
}
