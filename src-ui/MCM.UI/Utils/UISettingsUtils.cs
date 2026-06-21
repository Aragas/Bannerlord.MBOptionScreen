using Bannerlord.ModuleManager;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Common;
using MCM.UI.Actions;
using MCM.UI.GUI.ViewModels;

using Microsoft.Extensions.Logging;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Localization;

namespace MCM.UI.Utils
{
    internal static class UISettingsUtils
    {
        // Shared, stateless comparator for alphanumeric (natural) name ordering.
        private static readonly AlphanumComparatorFast AlphanumComparator = new();

        // The final tie-break on the unique Id/GroupNameRaw makes these comparers a *total* order.
        // Without it, items that share an Order (and resolve to the same display name) compare equal,
        // and the unstable MBBindingList.Sort reshuffles them on every refresh - so groups/properties
        // would flip order each time the menu was reopened or Reset was pressed.
        public static readonly IComparer<SettingsPropertyVM> SettingsPropertyVMComparer = Comparer<SettingsPropertyVM>.Create((x, y) =>
        {
            var xDef = x.SettingPropertyDefinition;
            var yDef = y.SettingPropertyDefinition;

            var byOrder = xDef.Order.CompareTo(yDef.Order);
            if (byOrder != 0) return byOrder;

            var byName = AlphanumComparator.Compare(ResolveName(xDef.DisplayName), ResolveName(yDef.DisplayName));
            if (byName != 0) return byName;

            return string.CompareOrdinal(xDef.Id, yDef.Id);
        });

        public static readonly IComparer<SettingsPropertyGroupVM> SettingsPropertyGroupVMComparer = Comparer<SettingsPropertyGroupVM>.Create((x, y) =>
        {
            var xDef = x.SettingPropertyGroupDefinition;
            var yDef = y.SettingPropertyGroupDefinition;

            // Default group first (true sorts before false).
            var byDefault = IsDefaultGroup(yDef).CompareTo(IsDefaultGroup(xDef));
            if (byDefault != 0) return byDefault;

            var byOrder = xDef.Order.CompareTo(yDef.Order);
            if (byOrder != 0) return byOrder;

            var byName = AlphanumComparator.Compare(ResolveName(xDef.GroupName), ResolveName(yDef.GroupName));
            if (byName != 0) return byName;

            return string.CompareOrdinal(xDef.GroupNameRaw, yDef.GroupNameRaw);
        });

        private static bool IsDefaultGroup(SettingsPropertyGroupDefinition group) =>
            group.GroupNameRaw == SettingsPropertyGroupDefinition.DefaultGroupName;

        private static string ResolveName(string name) => new TextObject(name).ToString();

        /// <summary>
        /// Mimics the same method in SettingsUtils, but it registers every action in URS
        /// </summary>
        /// <param name="urs"></param>
        /// <param name="current"></param>
        /// <param name="new"></param>
        public static void OverrideValues(UndoRedoStack urs, BaseSettings current, BaseSettings @new)
        {
            var currentDict = SettingPropertyDefinitionCache.GetSettingPropertyGroups(current).ToDictionary(x => x.GroupNameRaw, x => x);

            foreach (var nspg in SettingPropertyDefinitionCache.GetAllSettingPropertyGroupDefinitions(@new))
            {
                if (currentDict.TryGetValue(nspg.GroupNameRaw, out var spg))
                    OverrideValues(urs, spg, nspg);
                else
                    MCMUISubModule.Logger.LogWarning("{NewId}::{GroupName} was not found on, {CurrentId}", @new.Id, nspg.GroupNameRaw, current.Id);
            }
        }
        public static void OverrideValues(UndoRedoStack urs, SettingsPropertyGroupDefinition current, SettingsPropertyGroupDefinition @new)
        {
            var currentSubGroups = current.SubGroups.ToDictionary(x => x.GroupNameRaw, x => x);
            var currentSettingProperties = current.SettingProperties.ToDictionary(x => x.DisplayName, x => x);

            foreach (var nspg in @new.SubGroups)
            {
                if (currentSubGroups.TryGetValue(nspg.GroupNameRaw, out var spg))
                    OverrideValues(urs, spg, nspg);
                else
                    MCMUISubModule.Logger.LogWarning("{NewId}::{GroupName} was not found on, {CurrentId}", @new.GroupNameRaw, nspg.GroupNameRaw, current.GroupNameRaw);
            }
            foreach (var nsp in @new.SettingProperties)
            {
                if (currentSettingProperties.TryGetValue(nsp.DisplayName, out var sp))
                    OverrideValues(urs, sp, nsp);
                else
                    MCMUISubModule.Logger.LogWarning("{NewId}::{GroupName} was not found on, {CurrentId}", @new.GroupNameRaw, nsp.DisplayName, current.GroupNameRaw);
            }
        }
        public static void OverrideValues(UndoRedoStack urs, ISettingsPropertyDefinition current, ISettingsPropertyDefinition @new)
        {
            if (SettingsUtils.Equals(current, @new))
                return;

            switch (current.SettingType)
            {
                case SettingType.Bool when @new.PropertyReference.Value is bool val:
                    urs.Do(new SetValueTypeAction<bool>(current.PropertyReference, val));
                    break;
                case SettingType.Int when @new.PropertyReference.Value is int val:
                    urs.Do(new SetValueTypeAction<int>(current.PropertyReference, val));
                    break;
                case SettingType.Float when @new.PropertyReference.Value is float val:
                    urs.Do(new SetValueTypeAction<float>(current.PropertyReference, val));
                    break;
                case SettingType.String when @new.PropertyReference.Value is string val:
                    urs.Do(new SetStringAction(current.PropertyReference, val));
                    break;
                case SettingType.Dropdown when @new.PropertyReference.Value is { } val:
                    urs.Do(new SetSelectedIndexAction(current.PropertyReference, val));
                    break;
                case SettingType.Button when @new.PropertyReference.Value is Action val:
                    break;
            }
        }

        public static IEnumerable<object> GetDropdownValues(IRef @ref) => @ref.Value switch
        {
            IEnumerable<object> enumerableObj => enumerableObj,
            IEnumerable enumerable => enumerable.Cast<object>(),
            _ => []
        };
    }
}