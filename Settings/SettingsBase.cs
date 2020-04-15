using MBOptionScreen.ExtensionMethods;
using MBOptionScreen.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.Settings
{
    public abstract class SettingsBase<T> : SettingsBase where T : SettingsBase, new()
    {
        private static T? _instance = null;
        public static T? Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var emptySettings = new T();

                _instance = FileDatabase.Get<T>(emptySettings.Id);
                if (_instance != null)
                    return _instance;

                _instance = emptySettings;
                SettingsDatabase.SaveSettings(_instance);

                return _instance;
            }
        }
    }

    public abstract class SettingsBase : ISerializableFile, ISubFolder
    {
        public abstract string Id { get; set; }
        public abstract string ModuleFolderName { get; }
        public abstract string ModName { get; }
        public virtual string SubFolder => "";
        protected virtual char SubGroupDelimiter => '/';

        public abstract List<SettingPropertyGroupDefinition> GetSettingPropertyGroups();


        protected SettingPropertyGroupDefinition GetGroupFor(SettingPropertyDefinition sp, ICollection<SettingPropertyGroupDefinition> groupsList)
        {
            //If the setting somehow doesn't have a group attribute, throw an error.
            if (sp.GroupAttribute == null)
                throw new Exception($"SettingProperty {sp.Name} has null GroupAttribute");

            SettingPropertyGroupDefinition? group = null;
            //Check if the intended group is a sub group
            if (sp.GroupAttribute.GroupName.Contains(SubGroupDelimiter))
            {
                //Intended group is a sub group. Must find it. First get the top group.
                var topGroupName = GetTopGroupName(sp.GroupAttribute.GroupName, out var truncatedGroupName);
                var topGroup = groupsList.GetGroup(topGroupName);
                if (topGroup == null)
                {
                    topGroup = new SettingPropertyGroupDefinition(sp.GroupAttribute, topGroupName);
                    groupsList.Add(topGroup);
                }
                //Find the sub group
                group = GetGroupForRecursive(truncatedGroupName, topGroup.SubGroups, sp);
            }
            else
            {
                //Group is not a subgroup, can find it in the main list of groups.
                group = groupsList.GetGroup(sp.GroupAttribute.GroupName);
                if (group == null)
                {
                    group = new SettingPropertyGroupDefinition(sp.GroupAttribute);
                    groupsList.Add(group);
                }
            }
            return group;
        }

        protected SettingPropertyGroupDefinition? GetGroupFor(string groupName, ICollection<SettingPropertyGroupDefinition> groupsList)
        {
            return groupsList.GetGroup(groupName);
        }

        protected SettingPropertyGroupDefinition GetGroupForRecursive(string groupName, ICollection<SettingPropertyGroupDefinition> groupsList, SettingPropertyDefinition sp)
        {
            if (groupName.Contains(SubGroupDelimiter))
            {
                //Need to go deeper
                var topGroupName = GetTopGroupName(groupName, out var truncatedGroupName);
                var topGroup = GetGroupFor(topGroupName, groupsList);
                if (topGroup == null)
                {
                    topGroup = new SettingPropertyGroupDefinition(sp.GroupAttribute, topGroupName);
                    groupsList.Add(topGroup);
                }
                return GetGroupForRecursive(truncatedGroupName, topGroup.SubGroups, sp);
            }
            else
            {
                //Reached the bottom level, can return the final group.
                var group = groupsList.GetGroup(groupName);
                if (group == null)
                {
                    group = new SettingPropertyGroupDefinition(sp.GroupAttribute, groupName);
                    groupsList.Add(group);
                }
                return group;
            }
        }

        protected string GetTopGroupName(string groupName, out string truncatedGroupName)
        {
            var index = groupName.IndexOf(SubGroupDelimiter);
            var topGroupName = groupName.Substring(0, index);

            truncatedGroupName = groupName.Remove(0, index + 1);
            return topGroupName;
        }

        protected void CheckIsValid(SettingPropertyDefinition prop)
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