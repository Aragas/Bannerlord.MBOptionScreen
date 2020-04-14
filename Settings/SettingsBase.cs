using ModLib;
using ModLib.Attributes;
using ModLib.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen
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

                _instance = FileDatabase.Get<T>(emptySettings.ID);
                if (_instance != null)
                    return _instance;

                _instance = emptySettings;
                SettingsDatabase.SaveSettings(_instance);

                return _instance;
            }
        }
    }
}

namespace ModLib
{
    public abstract class SettingsBase : ISerialisableFile, ISubFolder
    {
        public abstract string ID { get; set; }
        public abstract string ModuleFolderName { get; }
        public abstract string ModName { get; }
        public virtual string SubFolder => "";

        internal List<SettingPropertyGroupDefinition> GetSettingPropertyGroups()
        {
            var groups = new List<SettingPropertyGroupDefinition>();

            var propList = (from p in GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            let propAttr = p.GetCustomAttribute<SettingPropertyAttribute>(true)
                            let groupAttr = p.GetCustomAttribute<SettingPropertyGroupAttribute>(true)
                            where propAttr != null
                            let groupAttrToAdd = groupAttr ?? SettingPropertyGroupAttribute.Default
                            select new SettingPropertyDefinition(propAttr, groupAttrToAdd, p, this)).ToList();

            foreach (var settingProp in propList)
            {
                CheckIsValid(settingProp);
                var group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }

            var noneGroup = GetGroupFor(groups, SettingPropertyGroupDefinition.DefaultGroupName);
            if (noneGroup != null && groups.Count > 1)
                groups.Remove(noneGroup);
            else
                noneGroup = null;

            groups.Sort((x, y) => x.GroupName.CompareTo(y.GroupName));
            if (noneGroup != null)
                groups.Add(noneGroup);

            return groups;
        }

        private SettingPropertyGroupDefinition GetGroupFor(SettingPropertyDefinition sp, List<SettingPropertyGroupDefinition> groupsList)
        {
            if (sp.GroupAttribute == null)
                throw new Exception($"SettingProperty {sp.Name} has null GroupAttribute");

            var group = groupsList.FirstOrDefault(x => x.GroupName == sp.GroupAttribute.GroupName);
            if (group == null)
            {
                group = new SettingPropertyGroupDefinition(sp.GroupAttribute);
                groupsList.Add(group);
            }
            return group;
        }

        private SettingPropertyGroupDefinition GetGroupFor(List<SettingPropertyGroupDefinition> groupsList, string groupName)
        {
            return groupsList.FirstOrDefault(x => x.GroupName == groupName);
        }

        private void CheckIsValid(SettingPropertyDefinition prop)
        {
            if (!prop.Property.CanRead)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a getter.");
            if (!prop.Property.CanWrite)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a setter.");
            if (prop.SettingType == SettingType.Int || prop.SettingType == SettingType.Float)
            {
                if (prop.MinValue == prop.MaxValue)
                    throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} is a numeric type but the MinValue and MaxValue are the same.");
            }
        }
    }
}
