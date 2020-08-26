﻿using HarmonyLib;

using MCM.Abstractions;
using MCM.Abstractions.Common;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Models;
using MCM.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Utils
{
    public static class SettingsUtils
    {
        public static void CheckIsValid(ISettingsPropertyDefinition prop, object settings)
        {
            // TODO:
            if (prop.PropertyReference is PropertyRef propertyRef)
            {
                if (!propertyRef.PropertyInfo.CanRead)
                    throw new Exception($"Property {propertyRef.PropertyInfo.Name} in {settings.GetType().FullName} must have a getter.");
                if (prop.SettingType != SettingType.Dropdown && !propertyRef.PropertyInfo.CanWrite)
                    throw new Exception($"Property {propertyRef.PropertyInfo.Name} in {settings.GetType().FullName} must have a setter.");

                if (prop.SettingType == SettingType.Float || prop.SettingType == SettingType.Int)
                {
                    if (prop.MinValue == prop.MaxValue)
                        throw new Exception($"Property {propertyRef.PropertyInfo.Name} in {settings.GetType().FullName} is a numeric type but the MinValue and MaxValue are the same.");
                }

                if (prop.SettingType != SettingType.Bool)
                {
                    if (prop.IsToggle)
                        throw new Exception($"Property {propertyRef.PropertyInfo.Name} in {settings.GetType().FullName} is marked as the main toggle for the group but is a numeric type. The main toggle must be a boolean type.");
                }
            }
        }

        public static void ResetSettings(BaseSettings settings)
        {
            GlobalSettings newSettings;
            if (settings is IWrapper wrapper)
            {
                newSettings = (GlobalSettings) Activator.CreateInstance(wrapper.GetType(), Activator.CreateInstance(wrapper.Object.GetType()));
            }
            else
                newSettings = (GlobalSettings) Activator.CreateInstance(settings.GetType());
            OverrideSettings(settings, newSettings);
        }
        public static void OverrideSettings(BaseSettings settings, BaseSettings overrideSettings)
        {
            OverrideValues(settings, overrideSettings);
        }


        public static bool Equals(BaseSettings settings1, BaseSettings settings2)
        {
            var set1 = settings1.GetAllSettingPropertyDefinitions().ToList();
            var set2 = settings2.GetAllSettingPropertyDefinitions().ToList();

            foreach (var settingsPropertyDefinition1 in set1)
            {
                var settingsPropertyDefinition2 = set2.Find(x =>
                    x.DisplayName == settingsPropertyDefinition1.DisplayName &&
                    x.GroupName == settingsPropertyDefinition1.GroupName);

                if (!Equals(settingsPropertyDefinition1, settingsPropertyDefinition2))
                    return false;
            }

            return true;
        }
        public static bool Equals(ISettingsPropertyDefinition currentDefinition, ISettingsPropertyDefinition newDefinition)
        {
            // TODO:
            switch (currentDefinition.SettingType)
            {
                case SettingType.Bool:
                case SettingType.Int:
                case SettingType.Float:
                case SettingType.String:
                {
                    var original = currentDefinition.PropertyReference.Value;
                    var @new = newDefinition.PropertyReference.Value;
                    return original.Equals(@new);
                }
                case SettingType.Dropdown:
                {
                    var original = new SelectedIndexWrapper(currentDefinition.PropertyReference.Value);
                    var @new = new SelectedIndexWrapper(newDefinition.PropertyReference.Value);
                    return original.SelectedIndex.Equals(@new.SelectedIndex);
                }
                default:
                {
                    return false;
                }
            }
        }

        public static void OverrideValues(BaseSettings current, BaseSettings @new)
        {
            foreach (var newSettingPropertyGroup in @new.GetSettingPropertyGroups())
            {
                var settingPropertyGroup = current.GetSettingPropertyGroups()
                    .Find(x => x.GroupName == newSettingPropertyGroup.GroupName);
                OverrideValues(settingPropertyGroup, newSettingPropertyGroup);
            }
        }
        public static void OverrideValues(SettingsPropertyGroupDefinition current, SettingsPropertyGroupDefinition @new)
        {
            foreach (var newSettingPropertyGroup in @new.SubGroups)
            {
                var settingPropertyGroup = current.SubGroups
                    .FirstOrDefault(x => x.GroupName == newSettingPropertyGroup.GroupName);
                OverrideValues(settingPropertyGroup, newSettingPropertyGroup);
            }
            foreach (var newSettingProperty in @new.SettingProperties)
            {
                var settingProperty = current.SettingProperties
                    .FirstOrDefault(x => x.DisplayName == newSettingProperty.DisplayName);
                OverrideValues(settingProperty, newSettingProperty);
            }
        }
        public static void OverrideValues(ISettingsPropertyDefinition current, ISettingsPropertyDefinition @new)
        {
            if (Equals(current, @new))
                return;

            current.PropertyReference.Value = @new.PropertyReference.Value;
        }


        public static bool IsForGenericDropdown(Type type)
        {
            var implementsList = type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
            var hasSelectedIndex = AccessTools.Property(type, "SelectedIndex") != null;
            return implementsList && hasSelectedIndex;
        }

        public static bool IsForTextDropdown(Type type) =>
            IsForGenericDropdown(type) && type.IsGenericType && AccessTools.Property(type.GenericTypeArguments[0], "IsSelected") == null;

        public static bool IsForCheckboxDropdown(Type type) =>
            IsForGenericDropdown(type) && type.IsGenericType && AccessTools.Property(type.GenericTypeArguments[0], "IsSelected") != null;

        public static bool IsForTextDropdown(object obj) => IsForTextDropdown(obj.GetType());
        public static bool IsForCheckboxDropdown(object obj) => IsForCheckboxDropdown(obj.GetType());

        public static object GetSelector(object dropdown)
        {
            var selectorProperty = AccessTools.Property(dropdown.GetType(), "Selector");
            return selectorProperty != null
                ? selectorProperty.GetValue(dropdown)
                : MCMSelectorVM<MCMSelectorItemVM>.Empty;
        }

        public static IEnumerable<ISettingsPropertyDefinition> GetAllSettingPropertyDefinitions(SettingsPropertyGroupDefinition settingPropertyGroup1)
        {
            foreach (var settingProperty in settingPropertyGroup1.SettingProperties)
                yield return settingProperty;

            foreach (var settingPropertyGroup in settingPropertyGroup1.SubGroups)
            foreach (var settingProperty in GetAllSettingPropertyDefinitions(settingPropertyGroup))
            {
                yield return settingProperty;
            }
        }
        public static IEnumerable<SettingsPropertyGroupDefinition> GetAllSettingPropertyGroupDefinitions(SettingsPropertyGroupDefinition settingPropertyGroup1)
        {
            yield return settingPropertyGroup1;

            foreach (var settingPropertyGroup in settingPropertyGroup1.SubGroups)
                yield return settingPropertyGroup;
        }

        public static List<SettingsPropertyGroupDefinition> GetSettingsPropertyGroups(char subGroupDelimiter, IEnumerable<ISettingsPropertyDefinition> settingsPropertyDefinitions)
        {
            var groups = new List<SettingsPropertyGroupDefinition>();
            foreach (var settingProp in settingsPropertyDefinitions)
            {
                var group = GetGroupFor(subGroupDelimiter, settingProp, groups);
                group.Add(settingProp);
            }
            return groups;
        }
        public static SettingsPropertyGroupDefinition GetGroupFor(char subGroupDelimiter, ISettingsPropertyDefinition sp, ICollection<SettingsPropertyGroupDefinition> rootCollection)
        {
            SettingsPropertyGroupDefinition? group;
            // Check if the intended group is a sub group
            if (sp.GroupName.Contains(subGroupDelimiter))
            {
                // Intended group is a sub group. Must find it. First get the top group.
                var topGroupName = GetTopGroupName(subGroupDelimiter, sp.GroupName, out var truncatedGroupName);
                var topGroup = rootCollection.GetGroup(topGroupName);
                if (topGroup == null)
                {
                    // Order will not be passed to the subgroup
                    topGroup = new SettingsPropertyGroupDefinition(sp.GroupName, topGroupName);
                    rootCollection.Add(topGroup);
                }
                // Find the sub group
                group = GetGroupForRecursive(subGroupDelimiter, truncatedGroupName, topGroup, sp);
            }
            else
            {
                // Group is not a subgroup, can find it in the main list of groups.
                group = rootCollection.GetGroup(sp.GroupName);
                if (group == null)
                {
                    group = new SettingsPropertyGroupDefinition(sp.GroupName, order: sp.GroupOrder);
                    rootCollection.Add(group);
                }
            }
            return group;
        }
        public static SettingsPropertyGroupDefinition GetGroupForRecursive(char subGroupDelimiter, string groupName, SettingsPropertyGroupDefinition sgp, ISettingsPropertyDefinition sp)
        {
            while (true)
            {
                if (groupName.Contains(subGroupDelimiter))
                {
                    // Need to go deeper
                    var topGroupName = GetTopGroupName(subGroupDelimiter, groupName, out var truncatedGroupName);
                    var topGroup = sgp.GetGroupFor(topGroupName);
                    if (topGroup == null)
                    {
                        // Order will not be passed to the subgroup
                        topGroup = new SettingsPropertyGroupDefinition(sp.GroupName, topGroupName);
                        sgp.Add(topGroup);
                    }

                    groupName = truncatedGroupName;
                    sgp = topGroup;
                }
                else
                {
                    // Reached the bottom level, can return the final group.
                    var group = sgp.GetGroup(groupName);
                    if (group == null)
                    {
                        group = new SettingsPropertyGroupDefinition(sp.GroupName, groupName, sp.GroupOrder);
                        sgp.Add(group);
                    }

                    return group;
                }
            }
        }
        public static string GetTopGroupName(char subGroupDelimiter, string groupName, out string truncatedGroupName)
        {
            var index = groupName.IndexOf(subGroupDelimiter);
            var topGroupName = groupName.Substring(0, index);

            truncatedGroupName = groupName.Remove(0, index + 1);
            return topGroupName;
        }
    }
}