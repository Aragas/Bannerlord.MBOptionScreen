using MCM.Abstractions.Settings;
using MCM.UI.Actions;
using MCM.UI.GUI.ViewModels;
using MCM.Utils;

using System.Collections.Generic;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.UI.GUI
{
    internal static class Utils
    {
        public static void OverridePropertyValues(IEnumerable<SettingsPropertyVM> settingProperties, object settings, object settingsNew)
        {
            if (settings.GetType() != settingsNew.GetType())
                return;

            foreach (var settingProperty in settingProperties)
            {
                if (Equal(settingProperty, settings, settingsNew))
                    return;

                switch (settingProperty.SettingType)
                {
                    case SettingType.Bool:
                        settingProperty.URS.Do(new SetValueTypeAction<bool>(
                            new ProxyRef(() => settingProperty.BoolValue, o => settingProperty.BoolValue = (bool) o),
                            (bool) settingProperty.Property.GetValue(settingsNew)));
                        break;
                    case SettingType.Int:
                        settingProperty.URS.Do(new SetValueTypeAction<int>(
                            new ProxyRef(() => settingProperty.IntValue, o => settingProperty.IntValue = (int) o),
                            (int) settingProperty.Property.GetValue(settingsNew)));
                        break;
                    case SettingType.Float:
                        settingProperty.URS.Do(new SetValueTypeAction<float>(
                            new ProxyRef(() => settingProperty.FloatValue, o => settingProperty.FloatValue = (float) o),
                            (float) settingProperty.Property.GetValue(settingsNew)));
                        break;
                    case SettingType.String:
                        settingProperty.URS.Do(new SetStringAction(
                            new ProxyRef(() => settingProperty.StringValue, o => settingProperty.StringValue = (string) o),
                            (string) settingProperty.Property.GetValue(settingsNew)));
                        break;
                    case SettingType.Dropdown:
                        settingProperty.URS.Do(new SetDropdownIndexAction(
                            new ProxyRef(() => settingProperty.DropdownValue,
                                o => settingProperty.DropdownValue = (SelectorVM<SelectorItemVM>) o),
                            SettingsUtils.GetSelector(settingProperty.Property.GetValue(settingsNew))));
                        break;
                }
            }
        }

        private static bool Equal(SettingsPropertyVM settingProperty, object settings, object settingsNew)
        {
            switch (settingProperty.SettingType)
            {
                case SettingType.Bool:
                case SettingType.Int:
                case SettingType.Float:
                case SettingType.String:
                {
                    var original = settingProperty.Property.GetValue(settings);
                    var @new = settingProperty.Property.GetValue(settingsNew); 
                    return original.Equals(@new);
                }
                case SettingType.Dropdown:
                {
                    var original = (SelectorVM<SelectorItemVM>) settingProperty.Property.GetValue(settings);
                    var @new = (SelectorVM<SelectorItemVM>) settingProperty.Property.GetValue(settingsNew);
                    return original.SelectedIndex.Equals(@new.SelectedIndex);
                }
                default:
                {
                    return false;
                }
            }
        }
    }
}