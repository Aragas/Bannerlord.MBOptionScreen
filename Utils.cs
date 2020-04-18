using MBOptionScreen.Attributes;
using MBOptionScreen.Settings;
using MBOptionScreen.Settings.Wrapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen
{
    internal static class Utils
    {
        public static bool AnyBaseTypeIs(Type type, string fullTypeName)
        {
            Type baseType = type.BaseType;
            while (baseType != null)
            {
                if (baseType.FullName == fullTypeName)
                    return true;

                baseType = baseType.BaseType;
            }
            return false;
        }
        public static bool AnyBaseTypeIs(Type type, Type type2)
        {
            Type baseType = type.BaseType;
            while (baseType != null)
            {
                if (baseType == type2)
                    return true;

                baseType = baseType.BaseType;
            }
            return false;
        }

        public static IEnumerable<SettingPropertyDefinition> GetProperties(object @object, string id)
        {
            foreach (var property in @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = property.GetCustomAttributes();

                object groupAttrObj = attributes.FirstOrDefault(a => a.GetType().FullName == "ModLib.Attributes.SettingPropertyGroupAttribute")
                    ?? attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyGroupAttribute");
                var groupAttr = groupAttrObj != null
                    ? (SettingPropertyGroupAttribute) new SettingPropertyGroupAttributeWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                object propAttr;
                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "ModLib.Attributes.SettingPropertyAttribute")
                    ?? attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyAttribute");
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new SettingPropertyAttributeWrapper(propAttr),
                        groupAttr,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyBoolAttribute");
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new SettingPropertyBoolAttributeWrapper(propAttr),
                        groupAttr,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyFloatingIntegerAttribute");
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new SettingPropertyFloatingIntegerAttributeWrapper(propAttr),
                        groupAttr,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyIntegerAttribute");
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new SettingPropertyIntegerAttributeWrapper(propAttr),
                        groupAttr,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyTextAttribute");
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new SettingPropertyTextAttributeWrapper(propAttr),
                        groupAttr,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyDropdownAttribute");
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new SettingPropertyDropdownAttributeWrapper(propAttr),
                        groupAttr,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }
            }
        }
    }
}
