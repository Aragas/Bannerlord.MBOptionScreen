using Bannerlord.ModuleManager;

using ComparerExtensions;

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
        public static IComparer<SettingsPropertyVM> SettingsPropertyVMComparer = KeyComparer<SettingsPropertyVM>
            .OrderBy(x => x.SettingPropertyDefinition.Order)
            .ThenBy(x => new TextObject(x.SettingPropertyDefinition.DisplayName).ToString(), new AlphanumComparatorFast());

        public static IComparer<SettingsPropertyGroupVM> SettingsPropertyGroupVMComparer = KeyComparer<SettingsPropertyGroupVM>
            .OrderByDescending(x => x.SettingPropertyGroupDefinition.GroupName == SettingsPropertyGroupDefinition.DefaultGroupName)
            .ThenBy(x => x.SettingPropertyGroupDefinition.Order)
            .ThenBy(x => new TextObject(x.SettingPropertyGroupDefinition.GroupName).ToString(), new AlphanumComparatorFast());

        /// <summary>
        /// Mimics the same method in SettingsUtils, but it registers every action in URS
        /// </summary>
        /// <param name="urs"></param>
        /// <param name="current"></param>
        /// <param name="new"></param>
        public static void OverrideValues(UndoRedoStack urs, BaseSettings current, BaseSettings @new)
        {
            var currentDict = current.GetUnsortedSettingPropertyGroups().ToDictionary(x => x.GroupName, x => x);

            foreach (var nspg in @new.GetAllSettingPropertyGroupDefinitions())
            {
                if (currentDict.TryGetValue(nspg.GroupName, out var spg))
                    OverrideValues(urs, spg, nspg);
                else
                    MCMUISubModule.Logger.LogWarning("{NewId}::{GroupName} was not found on, {CurrentId}", @new.Id, nspg.GroupName, current.Id);
            }
        }
        public static void OverrideValues(UndoRedoStack urs, SettingsPropertyGroupDefinition current, SettingsPropertyGroupDefinition @new)
        {
            var currentSubGroups = current.SubGroups.ToDictionary(x => x.GroupName, x => x);
            var currentSettingProperties = current.SettingProperties.ToDictionary(x => x.DisplayName, x => x);

            foreach (var nspg in @new.SubGroups)
            {
                if (currentSubGroups.TryGetValue(nspg.GroupName, out var spg))
                    OverrideValues(urs, spg, nspg);
                else
                    MCMUISubModule.Logger.LogWarning("{NewId}::{GroupName} was not found on, {CurrentId}", @new.GroupName, nspg.GroupName, current.GroupName);
            }
            foreach (var nsp in @new.SettingProperties)
            {
                if (currentSettingProperties.TryGetValue(nsp.DisplayName, out var sp))
                    OverrideValues(urs, sp, nsp);
                else
                    MCMUISubModule.Logger.LogWarning("{NewId}::{GroupName} was not found on, {CurrentId}", @new.GroupName, nsp.DisplayName, current.GroupName);
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
            _ => Enumerable.Empty<object>()
        };
    }
}