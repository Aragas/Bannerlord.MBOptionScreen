using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;
using MCM.Extensions;
using MCM.UI.Actions;
using MCM.Utils;

using Microsoft.Extensions.Logging;

using System;
using System.Linq;

namespace MCM.UI.Utils
{
    internal static class UISettingsUtils
    {
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
                    MCMUISubModule.Logger.LogWarning("{NewId}::{GroupName} was not found on, {CurrentId}", @new.DisplayGroupName, nspg.GroupName, current.DisplayGroupName);
            }
            foreach (var nsp in @new.SettingProperties)
            {
                if (currentSettingProperties.TryGetValue(nsp.DisplayName, out var sp))
                    OverrideValues(urs, sp, nsp);
                else
                    MCMUISubModule.Logger.LogWarning("{NewId}::{GroupName} was not found on, {CurrentId}", @new.DisplayGroupName, nsp.DisplayName, current.DisplayGroupName);
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
    }
}