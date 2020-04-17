using MBOptionScreen.Attributes;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen.Settings
{
    public class Dropdown
    {
        protected IList Values;
        protected int SelectedIndex;

        public virtual IList GetValues() => Values;
        public virtual object GetSelectedValue() => Values[SelectedIndex];
        public virtual int GetSelectedIndex() => SelectedIndex;
    }

    public class Dropdown<T> : Dropdown
    {
        public static Dropdown<T> Empty => new Dropdown<T>(Array.Empty<T>(), 0);

        public Dropdown(IEnumerable<T> values, int selectedIndex)
        {
            Values = values.ToList();
            SelectedIndex = selectedIndex;

            if (SelectedIndex != 0 && SelectedIndex >= Values.Count)
                throw new Exception();
        }

        public virtual IEnumerable<T> GetEnumerable()
        {
            foreach (var @object in Values)
                yield return (T)@object;
        }
        public virtual IList<T> GetValues() => GetEnumerable().ToList();
        public new virtual T GetSelectedValue() => (T) Values[SelectedIndex];
        public new virtual void SelectValue(T @obj)
        {
            SelectedIndex = Values.IndexOf(@obj);
        }
    }

    public class SettingPropertyDefinition
    {
        public string SettingsId { get; }
        public SettingsBase SettingsInstance => SettingsDatabase.GetSettings(SettingsId);
        public SettingPropertyGroupAttribute GroupAttribute { get; }
        public SettingPropertyAttribute SettingAttribute { get; }
        public PropertyInfo Property { get; }
        public SettingType SettingType { get; }

        public string Name => SettingAttribute.DisplayName;
        public float MaxValue => SettingAttribute.MaxValue;
        public float MinValue => SettingAttribute.MinValue;

        public SettingPropertyDefinition(SettingPropertyAttribute settingAttribute, SettingPropertyGroupAttribute groupAttribute, PropertyInfo property, string settingsId)
        {
            SettingAttribute = settingAttribute;
            GroupAttribute = groupAttribute;

            Property = property;
            SettingsId = settingsId;

            if (Property.PropertyType == typeof(bool))
                SettingType = SettingType.Bool;
            else if (Property.PropertyType == typeof(int))
                SettingType = SettingType.Int;
            else if (Property.PropertyType == typeof(float))
                SettingType = SettingType.Float;
            else if (Property.PropertyType == typeof(string))
                SettingType = SettingType.String;
            else if (Property.PropertyType == typeof(Dropdown) || Property.PropertyType == typeof(Dropdown<string>))
                SettingType = SettingType.Dropdown;
            //else
            //    throw new Exception($"Property {Property.Name} in {SettingsInstance.GetType().FullName} has an invalid type.\nValid types are {string.Join(",", Enum.GetNames(typeof(SettingType)))}");
        }
    }
}