using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v1;
using MBOptionScreen.Attributes.v2;
using MBOptionScreen.Settings;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen.Utils
{
    internal static class SettingsUtils
    {
        public static IEnumerable<SettingPropertyDefinition> GetProperties(object @object, string id)
        {
            foreach (var property in @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = property.GetCustomAttributes();

                object groupAttrObj = attributes.FirstOrDefault(a => a.GetType().FullName == "ModLib.Attributes.SettingPropertyGroupAttribute")
                    ?? attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyGroupAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyGroupDefinition)));
                var groupDefinition = groupAttrObj != null
                    ? (IPropertyGroupDefinition) new PropertyGroupDefinitionWrapper(groupAttrObj)
                    : (IPropertyGroupDefinition) SettingPropertyGroupAttribute.Default;

                object propAttr;
                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "ModLib.Attributes.SettingPropertyAttribute")
                    ?? attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyAttribute")
                    ?? attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v1.SettingPropertyAttribute");
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new SettingPropertyAttributeWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyBoolAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionBool)));
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new PropertyDefinitionBoolWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyFloatingIntegerAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionFloatingInteger)));
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new PropertyDefinitionFloatingIntegerWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyIntegerAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionInteger)));
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new PropertyDefinitionIntegerWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyTextAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionText)));
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new PropertyDefinitionTextWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyDropdownAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionDropdown)));
                if (propAttr != null)
                {
                    yield return new SettingPropertyDefinition(
                        new PropertyDefinitionDropdownWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }
            }
        }
    }
}