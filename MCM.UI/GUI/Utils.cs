using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;
using MCM.UI.Actions;
using MCM.Utils;

using System.Linq;

namespace MCM.UI.GUI
{
    internal static class Utils
    {
        /// <summary>
        /// Mimics the same method in SettingsUtils, but it registers every action in URS
        /// </summary>
        /// <param name="urs"></param>
        /// <param name="current"></param>
        /// <param name="new"></param>
        public static void OverrideValues(UndoRedoStack urs, BaseSettings current, BaseSettings @new)
        {
            foreach (var newSettingPropertyGroup in @new.GetSettingPropertyGroups())
            {
                var settingPropertyGroup = current.GetSettingPropertyGroups()
                    .FirstOrDefault(x => x.DisplayGroupName.ToString() == newSettingPropertyGroup.DisplayGroupName.ToString());
                OverrideValues(urs, settingPropertyGroup, newSettingPropertyGroup);
            }
        }
        public static void OverrideValues(UndoRedoStack urs, SettingsPropertyGroupDefinition current, SettingsPropertyGroupDefinition @new)
        {
            foreach (var newSettingPropertyGroup in @new.SubGroups)
            {
                var settingPropertyGroup = current.SubGroups
                    .FirstOrDefault(x => x.DisplayGroupName.ToString() == newSettingPropertyGroup.DisplayGroupName.ToString());
                OverrideValues(urs, settingPropertyGroup, newSettingPropertyGroup);
            }
            foreach (var newSettingProperty in @new.SettingProperties)
            {
                var settingProperty = current.SettingProperties
                    .FirstOrDefault(x => x.DisplayName == newSettingProperty.DisplayName);
                OverrideValues(urs, settingProperty, newSettingProperty);
            }
        }
        public static void OverrideValues(UndoRedoStack urs, ISettingsPropertyDefinition current, ISettingsPropertyDefinition @new)
        {
            if (SettingsUtils.Equals(current, @new))
                return;

            switch (current.SettingType)
            {
                case SettingType.Bool:
                    urs.Do(new SetValueTypeAction<bool>(current.PropertyReference, (bool) @new.PropertyReference.Value));
                    break;
                case SettingType.Int:
                    urs.Do(new SetValueTypeAction<int>(current.PropertyReference, (int) @new.PropertyReference.Value));
                    break;
                case SettingType.Float:
                    urs.Do(new SetValueTypeAction<float>(current.PropertyReference, (float) @new.PropertyReference.Value));
                    break;
                case SettingType.String:
                    urs.Do(new SetStringAction(current.PropertyReference, (string) @new.PropertyReference.Value));
                    break;
                case SettingType.Dropdown:
                    urs.Do(new SetDropdownIndexAction(current.PropertyReference, SettingsUtils.GetSelector(@new.PropertyReference.Value)));
                    break;
            }
        }
    }
}