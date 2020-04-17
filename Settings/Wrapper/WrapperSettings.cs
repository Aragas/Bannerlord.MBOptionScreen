using MBOptionScreen.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen.Settings.Wrapper
{
    /// <summary>
    /// It is unsafe to cast an instance of SettingsBase to out SettingsBase, so use a wrapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class WrapperSettings : SettingsBase
    {
        internal readonly object _object;
        private PropertyInfo IdProperty { get; }
        private PropertyInfo ModuleFolderNameProperty { get; }
        private PropertyInfo ModNameProperty { get; }
        private PropertyInfo UIVersionProperty { get; }
        private PropertyInfo SubFolderProperty { get; }
        private PropertyInfo SubGroupDelimiterProperty { get; }


        public override string Id { get => (string) IdProperty.GetValue(_object); set => IdProperty.SetValue(_object, value); }
        public override string ModuleFolderName => (string) ModuleFolderNameProperty.GetValue(_object);
        public override string ModName => (string) ModNameProperty.GetValue(_object);
        public override int UIVersion => UIVersionProperty?.GetValue(_object) as int? ?? 1;
        public override string SubFolder => SubFolderProperty?.GetValue(_object) as string ?? "";
        protected override char SubGroupDelimiter => SubGroupDelimiterProperty?.GetValue(_object) as char? ?? '/';

        public WrapperSettings(object @object)
        {
            _object = @object;
            var type = @object.GetType();
            IdProperty = type.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public) ??
                         type.GetProperty("ID", BindingFlags.Instance | BindingFlags.Public);
            ModuleFolderNameProperty = type.GetProperty("ModuleFolderName", BindingFlags.Instance | BindingFlags.Public);
            ModNameProperty = type.GetProperty("ModName", BindingFlags.Instance | BindingFlags.Public);
            UIVersionProperty = type.GetProperty("UIVersion", BindingFlags.Instance | BindingFlags.Public);
            SubFolderProperty = type.GetProperty("SubFolder", BindingFlags.Instance | BindingFlags.Public);
            SubGroupDelimiterProperty = type.GetProperty("SubGroupDelimiter", BindingFlags.Instance | BindingFlags.Public);
        }

        public override List<SettingPropertyGroupDefinition> GetSettingPropertyGroups()
        {
            var groups = new List<SettingPropertyGroupDefinition>();

            var propList = from propertyInfo in _object.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                let propAttr = propertyInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType().FullName == "ModLib.Attributes.SettingPropertyAttribute")
                where propAttr != null
                let groupAttr = propertyInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType().FullName == "ModLib.Attributes.SettingPropertyGroupAttribute")
                let pAttr = new SettingPropertyAttributeWrapper(propAttr)
                let gAttr = groupAttr != null
                    ? (SettingPropertyGroupAttribute) new SettingPropertyGroupAttributeWrapper(groupAttr)
                    : SettingPropertyGroupAttribute.Default
                select new SettingPropertyDefinition(pAttr, gAttr, new ProxyPropertyInfo(_object, propertyInfo), Id);

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

            //Sort the list of groups alphabetically.
            groups.Sort((x, y) => string.Compare(x.GroupName, y.GroupName, StringComparison.Ordinal));
            if (miscGroup != null)
                groups.Add(miscGroup);

            return groups;
        }
    }
}