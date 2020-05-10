using HarmonyLib;

using MCM.Abstractions;
using MCM.Abstractions.Data;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.Utils
{
    public static class SettingsUtils
    {
        public static IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object, string id)
        {
            var settingPropertyDefinitionsDiscoverers = DI.GetImplementations<ISettingsPropertyDiscoverer, SettingsPropertyDiscovererWrapper>(ApplicationVersionUtils.GameVersion());
            return settingPropertyDefinitionsDiscoverers.SelectMany(d => d.GetProperties(@object, id));
        }

        public static void CheckIsValid(ISettingsPropertyDefinition prop, object settings)
        {
            if (!prop.Property.CanRead)
                throw new Exception($"Property {prop.Property.Name} in {settings.GetType().FullName} must have a getter.");
            if (prop.SettingType != SettingType.Dropdown && !prop.Property.CanWrite)
                throw new Exception($"Property {prop.Property.Name} in {settings.GetType().FullName} must have a setter.");

            if (prop.SettingType == SettingType.Float || prop.SettingType == SettingType.Int)
            {
                if (prop.MinValue == prop.MaxValue)
                    throw new Exception($"Property {prop.Property.Name} in {settings.GetType().FullName} is a numeric type but the MinValue and MaxValue are the same.");
            }

            if (prop.SettingType != SettingType.Bool)
            {
                if (prop.IsMainToggle)
                    throw new Exception($"Property {prop.Property.Name} in {settings.GetType().FullName} is marked as the main toggle for the group but is a numeric type. The main toggle must be a boolean type.");
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

        public static void CopyProperties(object settings, object settingsNew)
        {
            if (settings.GetType() != settingsNew.GetType())
                return;

            foreach (var propertyInfo in settings.GetType().GetProperties().Where(p => PropertyIsSetting(p.GetCustomAttributes<Attribute>(true)) && p.CanRead && p.CanWrite))
                propertyInfo.SetValue(settings, propertyInfo.GetValue(settingsNew));
        }

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
            if (settings is IWrapper wrapper && overrideSettings is IWrapper overrideWrapper)
                CopyProperties(wrapper.Object, overrideWrapper.Object);
            else
                CopyProperties(settings, overrideSettings);
        }

        public static bool IsDropdown(Type type)
        {
            return ReflectionUtils.ImplementsOrImplementsEquivalent(type, typeof(IDropdownProvider)) ||
                   ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MBOptionScreen.Data.IDropdownProvider");
        }

        public static SelectorVM<SelectorItemVM> GetSelector(object dropdown)
        {
            var selectorProperty = dropdown.GetType().GetProperty("Selector");
            if (selectorProperty == null)
                return new SelectorVM<SelectorItemVM>(0, _ => { });
            return (SelectorVM<SelectorItemVM>) selectorProperty.GetValue(dropdown);
        }
    }
}