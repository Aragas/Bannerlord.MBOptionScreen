using MBOptionScreen.ExtensionMethods;

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

                var settings = SettingsDatabase.GetSettings(new T().Id);
                //if (settings is AttributeSettingsWrapper settingsWrapper)
                //    return _instance = settingsWrapper._object as T;

                return _instance = settings as T;
            }
        }
    }

    public abstract class SettingsBase
    {
        public abstract string Id { get; set; }
        public abstract string ModuleFolderName { get; }
        public abstract string ModName { get; }
        public virtual int UIVersion => 1;
        public virtual string SubFolder => "";
        protected virtual char SubGroupDelimiter => '/';
        public virtual string Format => "json";

        public abstract List<SettingPropertyGroupDefinition> GetSettingPropertyGroups();


        protected SettingPropertyGroupDefinition GetGroupFor(SettingPropertyDefinition sp, ICollection<SettingPropertyGroupDefinition> rootCollection)
        {
            SettingPropertyGroupDefinition? group;
            //Check if the intended group is a sub group
            if (sp.GroupName.Contains(SubGroupDelimiter))
            {
                //Intended group is a sub group. Must find it. First get the top group.
                var topGroupName = GetTopGroupName(sp.GroupName, out var truncatedGroupName);
                var topGroup = rootCollection.GetGroup(topGroupName);
                if (topGroup == null)
                {
                    // Order will not be passed to the subgroup
                    topGroup = new SettingPropertyGroupDefinition(sp.GroupName, topGroupName);
                    rootCollection.Add(topGroup);
                }
                //Find the sub group
                group = GetGroupForRecursive(truncatedGroupName, topGroup, sp);
            }
            else
            {
                //Group is not a subgroup, can find it in the main list of groups.
                group = rootCollection.GetGroup(sp.GroupName);
                if (group == null)
                {
                    group = new SettingPropertyGroupDefinition(sp.GroupName, order: sp.GroupOrder);
                    rootCollection.Add(group);
                }
            }
            return group;
        }

        protected SettingPropertyGroupDefinition GetGroupForRecursive(string groupName, SettingPropertyGroupDefinition sgp, SettingPropertyDefinition sp)
        {
            if (groupName.Contains(SubGroupDelimiter))
            {
                //Need to go deeper
                var topGroupName = GetTopGroupName(groupName, out var truncatedGroupName);
                var topGroup = sgp.GetGroupFor(topGroupName);
                if (topGroup == null)
                {
                    // Order will not be passed to the subgroup
                    topGroup = new SettingPropertyGroupDefinition(sp.GroupName, topGroupName);
                    sgp.Add(topGroup);
                }
                return GetGroupForRecursive(truncatedGroupName, topGroup, sp);
            }
            else
            {
                //Reached the bottom level, can return the final group.
                var group = sgp.GetGroup(groupName);
                if (group == null)
                {
                    group = new SettingPropertyGroupDefinition(sp.GroupName, groupName, sp.GroupOrder);
                    sgp.Add(group);
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

        protected static void CheckIsValid(SettingPropertyDefinition prop)
        {
            if (!prop.Property.CanRead)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a getter.");
            if (prop.SettingType != SettingType.Dropdown && !prop.Property.CanWrite)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a setter.");

            if (prop.SettingType == SettingType.Int || prop.SettingType == SettingType.Float)
            {
                if (prop.MinValue == prop.MaxValue)
                    throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} is a numeric type but the MinValue and MaxValue are the same.");
                if (prop.IsMainToggle)
                    throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} is marked as the main toggle for the group but is a numeric type. The main toggle must be a boolean type.");
            }
        }
    }
}