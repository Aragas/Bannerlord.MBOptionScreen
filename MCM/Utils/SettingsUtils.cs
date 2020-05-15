using HarmonyLib;

using MCM.Abstractions;
using MCM.Abstractions.Data;
using MCM.Abstractions.ExtensionMethods;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.Utils
{
    public static class SettingsUtils
    {
        public static IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object)
        {
            var settingPropertyDefinitionsDiscoverers = DI.GetBaseImplementations<ISettingsPropertyDiscoverer>();
            return settingPropertyDefinitionsDiscoverers.SelectMany(d => d.GetProperties(@object));
        }

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
                    if (prop.IsMainToggle)
                        throw new Exception($"Property {propertyRef.PropertyInfo.Name} in {settings.GetType().FullName} is marked as the main toggle for the group but is a numeric type. The main toggle must be a boolean type.");
                }
            }
        }

        public static object? UnwrapSettings(object? settings)
        {
            while (settings != null && ReflectionUtils.ImplementsOrImplementsEquivalent(settings.GetType(), typeof(BaseGlobalSettingsWrapper)))
            {
                var prop = AccessTools.Property(settings.GetType(), "_object") ??
                           AccessTools.Property(settings.GetType(), nameof(IWrapper.Object));
                settings = prop?.GetValue(settings);
            }
            return settings;
        }
        public static GlobalSettings? WrapSettings(object? settingsObj) => settingsObj is { } settings
            ? settings is GlobalSettings settingsBase ? settingsBase : BaseGlobalSettingsWrapper.Create(settings)
            : null;

        // TODO:
        public static bool PropertyIsSetting(IEnumerable<Attribute> attributes) => attributes.Any(a =>
                ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "ModLib.Attributes.SettingPropertyAttribute") ||
                ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Attributes.BaseSettingPropertyAttribute") ||
                ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MCM.Abstractions.Settings.Definitions.IPropertyDefinitionBase") ||
                ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionBase)));

        public static void ResetSettings(BaseSettings settings)
        {
            var newSettings = settings is IWrapper wrapper
                    ? BaseGlobalSettingsWrapper.Create(Activator.CreateInstance(wrapper.Object.GetType()))
                    : (GlobalSettings) Activator.CreateInstance(settings.GetType());
            OverrideSettings(settings, newSettings);
        }
        public static void OverrideSettings(BaseSettings settings, BaseSettings overrideSettings)
        {
            OverrideValues(settings, overrideSettings);
            /*
            if (settings is IWrapper wrapper && overrideSettings is IWrapper overrideWrapper)
                CopyProperties(wrapper.Object, overrideWrapper.Object);
            else
                CopyProperties(settings, overrideSettings);
            */
        }


        public static bool Equals(BaseSettings settings1, BaseSettings settings2)
        {
            var set1 = settings1.GetSettingPropertyGroups().SelectMany(GetAllSettingPropertyDefinitions).ToList();
            var set2 = settings2.GetSettingPropertyGroups().SelectMany(GetAllSettingPropertyDefinitions).ToList();

            foreach (var settingsPropertyDefinition1 in set1)
            {
                var settingsPropertyDefinition2 = set2.FirstOrDefault(x =>
                    x.DisplayName == settingsPropertyDefinition1.DisplayName &&
                    x.GroupName == settingsPropertyDefinition1.GroupName);

                if (!Equals(settingsPropertyDefinition1, settingsPropertyDefinition2))
                    return false;
            }

            return true;
        }
        private static IEnumerable<ISettingsPropertyDefinition> GetAllSettingPropertyDefinitions(SettingsPropertyGroupDefinition settingPropertyGroup1)
        {
            foreach (var settingProperty in settingPropertyGroup1.SettingProperties)
                yield return settingProperty;

            foreach (var settingPropertyGroup in settingPropertyGroup1.SubGroups)
            foreach (var settingProperty in GetAllSettingPropertyDefinitions(settingPropertyGroup))
            {
                yield return settingProperty;
            }
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
                        var original = GetSelector(currentDefinition.PropertyReference.Value);
                        var @new = GetSelector(newDefinition.PropertyReference.Value);
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
                    .FirstOrDefault(x => x.DisplayGroupName.ToString() == newSettingPropertyGroup.DisplayGroupName.ToString());
                OverrideValues(settingPropertyGroup, newSettingPropertyGroup);
            }
        }
        public static void OverrideValues(SettingsPropertyGroupDefinition current, SettingsPropertyGroupDefinition @new)
        {
            foreach (var newSettingPropertyGroup in @new.SubGroups)
            {
                var settingPropertyGroup = current.SubGroups
                    .FirstOrDefault(x => x.DisplayGroupName.ToString() == newSettingPropertyGroup.DisplayGroupName.ToString());
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


        public static bool IsDropdown(Type type)
        {
            return ReflectionUtils.ImplementsOrImplementsEquivalent(type, typeof(IDropdownProvider)) ||
                   ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MBOptionScreen.Data.IDropdownProvider");
        }
        public static SelectorVM<SelectorItemVM> GetSelector(object dropdown)
        {
            var selectorProperty = AccessTools.Property(dropdown.GetType(), "Selector");
            return selectorProperty == null
                ? new SelectorVM<SelectorItemVM>(0, _ => { })
                : (SelectorVM<SelectorItemVM>) selectorProperty.GetValue(dropdown);
        }


        public static SettingsPropertyGroupDefinition GetGroupFor(char subGroupDelimiter, ISettingsPropertyDefinition sp, ICollection<SettingsPropertyGroupDefinition> rootCollection)
        {
            SettingsPropertyGroupDefinition? group;
            //Check if the intended group is a sub group
            if (sp.GroupName.Contains(subGroupDelimiter))
            {
                //Intended group is a sub group. Must find it. First get the top group.
                var topGroupName = GetTopGroupName(subGroupDelimiter, sp.GroupName, out var truncatedGroupName);
                var topGroup = rootCollection.GetGroup(topGroupName);
                if (topGroup == null)
                {
                    // Order will not be passed to the subgroup
                    topGroup = new SettingsPropertyGroupDefinition(sp.GroupName, topGroupName);
                    rootCollection.Add(topGroup);
                }
                //Find the sub group
                group = GetGroupForRecursive(subGroupDelimiter, truncatedGroupName, topGroup, sp);
            }
            else
            {
                //Group is not a subgroup, can find it in the main list of groups.
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
                    //Need to go deeper
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
                    //Reached the bottom level, can return the final group.
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