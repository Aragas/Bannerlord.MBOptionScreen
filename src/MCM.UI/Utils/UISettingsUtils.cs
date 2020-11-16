using MCM.Abstractions.Common;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;
using MCM.Extensions;
using MCM.UI.Actions;
using MCM.Utils;

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
            foreach (var newSettingPropertyGroup in @new.GetAllSettingPropertyGroupDefinitions())
            {
                var settingPropertyGroup = current.GetAllSettingPropertyGroupDefinitions()
                    .FirstOrDefault(x => x.GroupName == newSettingPropertyGroup.GroupName);
                if (settingPropertyGroup is not null)
                    OverrideValues(urs, settingPropertyGroup, newSettingPropertyGroup);
                // else log
            }
        }
        public static void OverrideValues(UndoRedoStack urs, SettingsPropertyGroupDefinition current, SettingsPropertyGroupDefinition @new)
        {
            foreach (var newSettingPropertyGroup in @new.SubGroups)
            {
                var settingPropertyGroup = current.SubGroups
                    .FirstOrDefault(x => x.GroupName == newSettingPropertyGroup.GroupName);
                if (settingPropertyGroup is not null)
                    OverrideValues(urs, settingPropertyGroup, newSettingPropertyGroup);
                // else log
            }
            foreach (var newSettingProperty in @new.SettingProperties)
            {
                var settingProperty = current.SettingProperties
                    .FirstOrDefault(x => x.DisplayName == newSettingProperty.DisplayName);
                if (settingProperty is not null)
                    OverrideValues(urs, settingProperty, newSettingProperty);
                // else log
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
                    urs.Do(new SetSelectedIndexAction(current.PropertyReference, new SelectedIndexWrapper(val)));
                    break;
            }
        }
    }
}