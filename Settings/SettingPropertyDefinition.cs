using MBOptionScreen.Attributes;

using System;
using System.Reflection;

namespace MBOptionScreen.Settings
{
    public class SettingPropertyDefinition
    {
        public SettingPropertyGroupAttribute GroupAttribute { get; }
        public SettingPropertyAttribute SettingAttribute { get; }
        public PropertyInfo Property { get; }
        public SettingsBase SettingsInstance { get; }
        public SettingType SettingType { get; }

        public string Name => SettingAttribute.DisplayName;
        public float MaxValue => SettingAttribute.MaxValue;
        public float MinValue => SettingAttribute.MinValue;

        public SettingPropertyDefinition(SettingPropertyAttribute settingAttribute, SettingPropertyGroupAttribute groupAttribute, PropertyInfo property, SettingsBase instance)
        {
            SettingAttribute = settingAttribute;
            GroupAttribute = groupAttribute;

            Property = property;
            SettingsInstance = instance;

            if (Property.PropertyType == typeof(bool))
                SettingType = SettingType.Bool;
            else if (Property.PropertyType == typeof(int))
                SettingType = SettingType.Int;
            else if (Property.PropertyType == typeof(float))
                SettingType = SettingType.Float;
            else
                throw new Exception($"Property {Property.Name} in {SettingsInstance.GetType().FullName} has an invalid type.\nValid types are {string.Join(",", Enum.GetNames(typeof(SettingType)))}");
        }
    }
}