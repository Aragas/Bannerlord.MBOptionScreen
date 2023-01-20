using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Base;
using MCM.Abstractions.Wrapper;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class SettingsUtils
    {
        public static void CheckIsValid(ISettingsPropertyDefinition prop, object? settings)
        {
            if (settings is null)
                throw new Exception($"Settings is null.");

            // TODO:
            if (prop.PropertyReference is PropertyRef propertyRef)
            {
                if (!propertyRef.PropertyInfo.CanRead)
                    throw new Exception($"Property {propertyRef.PropertyInfo.Name} in {settings.GetType().FullName} must have a getter.");
                if (prop.SettingType != SettingType.Dropdown && !propertyRef.PropertyInfo.CanWrite)
                    throw new Exception($"Property {propertyRef.PropertyInfo.Name} in {settings.GetType().FullName} must have a setter.");

                if (prop.SettingType is SettingType.Float or SettingType.Int)
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
            if (settings is IWrapper wrapper && Activator.CreateInstance(wrapper.Object?.GetType()) is { } copy && Activator.CreateInstance(wrapper.GetType(), copy) is BaseSettings copyWrapped)
                OverrideSettings(settings, copyWrapped);
            else if (Activator.CreateInstance(settings.GetType()) is BaseSettings copySettings)
                OverrideSettings(settings, copySettings);
        }

        public static void OverrideSettings(BaseSettings settings, BaseSettings overrideSettings)
        {
            OverrideValues(settings, overrideSettings);
        }


        public static bool Equals(BaseSettings settings1, BaseSettings settings2)
        {
            var setDict1 = settings1.GetAllSettingPropertyDefinitions().ToDictionary(x => (x.DisplayName, x.GroupName), x => x);
            var setDict2 = settings2.GetAllSettingPropertyDefinitions().ToDictionary(x => (x.DisplayName, x.GroupName), x => x);

            if (setDict1.Count != setDict2.Count)
                return false;

            foreach (var kv in setDict1)
            {
                if (!setDict2.TryGetValue(kv.Key, out var spd2) || !Equals(kv.Value, spd2))
                    return false;
            }

            return true;
        }
        public static bool Equals(ISettingsPropertyDefinition? currentDefinition, ISettingsPropertyDefinition? newDefinition)
        {
            if (currentDefinition is null || newDefinition is null)
                return false;

            // TODO:
            switch (currentDefinition.SettingType)
            {
                case SettingType.Bool:
                case SettingType.Int:
                case SettingType.Float:
                case SettingType.String:
                case SettingType.Button:
                {
                    if (currentDefinition.PropertyReference.Value is null || newDefinition.PropertyReference.Value is null)
                        return false;

                    var original = currentDefinition.PropertyReference.Value;
                    var @new = newDefinition.PropertyReference.Value;
                    return original.Equals(@new);
                }
                case SettingType.Dropdown:
                {
                    if (currentDefinition.PropertyReference.Value is null || newDefinition.PropertyReference.Value is null)
                        return false;

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
            var currentDict = current.GetUnsortedSettingPropertyGroups().ToDictionary(x => x.GroupNameRaw, x => x);

            foreach (var nspg in @new.GetUnsortedSettingPropertyGroups())
            {
                if (currentDict.TryGetValue(nspg.GroupNameRaw, out var spg))
                    OverrideValues(spg, nspg);
                else
                {
                    var logger = GenericServiceProvider.GetService<IBUTRLogger<SettingsUtils>>();
                    logger?.LogWarning($"{@new.Id}::{nspg.GroupNameRaw} was not found on, {current.Id}");
                }
            }
        }
        public static void OverrideValues(SettingsPropertyGroupDefinition current, SettingsPropertyGroupDefinition @new)
        {
            var currentSubGroups = current.SubGroups.ToDictionary(x => x.GroupNameRaw, x => x);
            var currentSettingProperties = current.SettingProperties.ToDictionary(x => x.DisplayName, x => x);

            foreach (var nspg in @new.SubGroups)
            {
                if (currentSubGroups.TryGetValue(nspg.GroupNameRaw, out var spg))
                    OverrideValues(spg, nspg);
                else
                {
                    var logger = GenericServiceProvider.GetService<IBUTRLogger<SettingsUtils>>();
                    logger?.LogWarning($"{@new.GroupName}::{nspg.GroupNameRaw} was not found on, {current.GroupNameRaw}");
                }
            }
            foreach (var nsp in @new.SettingProperties)
            {
                if (currentSettingProperties.TryGetValue(nsp.DisplayName, out var sp))
                    OverrideValues(sp, nsp);
                else
                {
                    var logger = GenericServiceProvider.GetService<IBUTRLogger<SettingsUtils>>();
                    logger?.LogWarning($"{@new.GroupNameRaw}::{nsp.DisplayName} was not found on, {current.GroupNameRaw}");
                }
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
            var hasSelectedIndex = AccessTools2.Property(type, "SelectedIndex", logErrorInTrace: false) is not null;
            return implementsList && hasSelectedIndex;
        }

        public static bool IsForTextDropdown(Type type) =>
            IsForGenericDropdown(type) && type.IsGenericType && AccessTools2.Property(type.GenericTypeArguments[0], "IsSelected", logErrorInTrace: false) is null;

        public static bool IsForCheckboxDropdown(Type type) =>
            IsForGenericDropdown(type) && type.IsGenericType && AccessTools2.Property(type.GenericTypeArguments[0], "IsSelected", logErrorInTrace: false) is not null;

        public static bool IsForTextDropdown(object? obj) => obj is not null && IsForTextDropdown(obj.GetType());
        public static bool IsForCheckboxDropdown(object? obj) => obj is not null && IsForCheckboxDropdown(obj.GetType());

        public static IEnumerable<ISettingsPropertyDefinition> GetAllSettingPropertyDefinitions(SettingsPropertyGroupDefinition settingPropertyGroup1)
        {
            foreach (var settingProperty in settingPropertyGroup1.SettingProperties)
            {
                yield return settingProperty;
            }

            foreach (var settingPropertyGroup in settingPropertyGroup1.SubGroups)
            {
                foreach (var settingProperty in GetAllSettingPropertyDefinitions(settingPropertyGroup))
                {
                    yield return settingProperty;
                }
            }
        }
        public static IEnumerable<SettingsPropertyGroupDefinition> GetAllSettingPropertyGroupDefinitions(SettingsPropertyGroupDefinition settingPropertyGroup)
        {
            yield return settingPropertyGroup;

            foreach (var settingPropertyGroup1 in settingPropertyGroup.SubGroups)
                yield return settingPropertyGroup1;
        }
        public static List<SettingsPropertyGroupDefinition> GetSettingsPropertyGroups(char subGroupDelimiter, IEnumerable<ISettingsPropertyDefinition> settingsPropertyDefinitions)
        {
            var groups = new List<SettingsPropertyGroupDefinition>();
            foreach (var settingsPropertyDefinition in settingsPropertyDefinitions)
            {
                var group = GetGroupFor(subGroupDelimiter, settingsPropertyDefinition, groups);
                group.Add(settingsPropertyDefinition);
            }
            return groups;
        }
        public static SettingsPropertyGroupDefinition GetGroupFor(char subGroupDelimiter, ISettingsPropertyDefinition sp, ICollection<SettingsPropertyGroupDefinition> rootCollection)
        {
            SettingsPropertyGroupDefinition? group;
            var groupName = sp.GroupName;
            // Check if the intended group is a sub group
            if (groupName.Contains(subGroupDelimiter))
            {
                // Intended group is a sub group. Must find it. First get the top group.
                var topGroupName = GetTopGroupName(subGroupDelimiter, groupName, out var truncatedGroupName);
                var topGroup = rootCollection.GetGroupFromName(topGroupName);
                if (topGroup is null)
                {
                    // Order will not be passed to the subgroup
                    topGroup = new SettingsPropertyGroupDefinition(topGroupName).SetSubGroupDelimiter(subGroupDelimiter);
                    rootCollection.Add(topGroup);
                }
                // Find the sub group
                group = GetGroupForRecursive(subGroupDelimiter, truncatedGroupName, topGroup, sp);
            }
            else
            {
                // Group is not a subgroup, can find it in the main list of groups.
                group = rootCollection.GetGroupFromName(groupName);
                if (group is null)
                {
                    group = new SettingsPropertyGroupDefinition(groupName, order: sp.GroupOrder).SetSubGroupDelimiter(subGroupDelimiter);
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
                    var topGroup = sgp.GetGroup(topGroupName);
                    if (topGroup is null)
                    {
                        // Order will not be passed to the subgroup
                        topGroup = new SettingsPropertyGroupDefinition(topGroupName).SetSubGroupDelimiter(subGroupDelimiter);
                        sgp.Add(topGroup);
                    }

                    groupName = truncatedGroupName;
                    sgp = topGroup;
                }
                else
                {
                    // Reached the bottom level, can return the final group.
                    var group = sgp.GetGroup(groupName);
                    if (group is null)
                    {
                        group = new SettingsPropertyGroupDefinition(groupName, sp.GroupOrder).SetSubGroupDelimiter(subGroupDelimiter);
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

        public static IEnumerable<IPropertyDefinitionBase> GetPropertyDefinitionWrappers(object property) => GetPropertyDefinitionWrappers(new[] { property });

        public static IEnumerable<IPropertyDefinitionBase> GetPropertyDefinitionWrappers(IReadOnlyCollection<object> properties)
        {
            object? propAttr;

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionBool);
            if (propAttr is not null)
                yield return new PropertyDefinitionBoolWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionDropdown);
            if (propAttr is not null)
                yield return new PropertyDefinitionDropdownWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionGroupToggle);
            if (propAttr is not null)
                yield return new PropertyDefinitionGroupToggleWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionText);
            if (propAttr is not null)
                yield return new PropertyDefinitionTextWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionWithActionFormat);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithActionFormatWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionWithCustomFormatter);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithCustomFormatterWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionWithEditableMinMax);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithEditableMinMaxWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionWithFormat);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithFormatWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionWithId);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithIdWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionWithMinMax);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithMinMaxWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => a is IPropertyDefinitionButton);
            if (propAttr is not null)
                yield return new PropertyDefinitionButtonWrapper(propAttr);
        }
    }
}