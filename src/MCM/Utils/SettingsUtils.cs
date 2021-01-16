using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.Utils
{
    public static class SettingsUtils
    {
        public static void CheckIsValid(ISettingsPropertyDefinition prop, object settings) { }

        public static object? UnwrapSettings(object? settings) => null;
        public static GlobalSettings? WrapSettings(object? settingsObj) => null;

        public static void ResetSettings(BaseSettings settings) { }
        public static void OverrideSettings(BaseSettings settings, BaseSettings overrideSettings) { }


        public static bool Equals(BaseSettings settings1, BaseSettings settings2) => false;
        public static bool Equals(ISettingsPropertyDefinition currentDefinition, ISettingsPropertyDefinition newDefinition) => false;

        public static void OverrideValues(BaseSettings current, BaseSettings @new) { }
        public static void OverrideValues(SettingsPropertyGroupDefinition current, SettingsPropertyGroupDefinition @new) { }
        public static void OverrideValues(ISettingsPropertyDefinition current, ISettingsPropertyDefinition @new) { }


        public static bool IsDropdown(Type type) => false;
        public static SelectorVM<SelectorItemVM> GetSelector(object dropdown) => null!;

        public static IEnumerable<ISettingsPropertyDefinition> GetAllSettingPropertyDefinitions(SettingsPropertyGroupDefinition settingPropertyGroup1)
            => Enumerable.Empty<ISettingsPropertyDefinition>();
        public static IEnumerable<SettingsPropertyGroupDefinition> GetAllSettingPropertyGroupDefinitions(SettingsPropertyGroupDefinition settingPropertyGroup1)
            => Enumerable.Empty<SettingsPropertyGroupDefinition>();

        public static List<SettingsPropertyGroupDefinition> GetSettingsPropertyGroups(char subGroupDelimiter, IEnumerable<ISettingsPropertyDefinition> settingsPropertyDefinitions)
            => new();
        public static SettingsPropertyGroupDefinition GetGroupFor(char subGroupDelimiter, ISettingsPropertyDefinition sp, ICollection<SettingsPropertyGroupDefinition> rootCollection)
            => null!;
        public static SettingsPropertyGroupDefinition GetGroupForRecursive(char subGroupDelimiter, string groupName, SettingsPropertyGroupDefinition sgp, ISettingsPropertyDefinition sp)
            => null!;
        public static string GetTopGroupName(char subGroupDelimiter, string groupName, out string truncatedGroupName)
        {
            truncatedGroupName = null!;
            return null!;
        }
    }
}