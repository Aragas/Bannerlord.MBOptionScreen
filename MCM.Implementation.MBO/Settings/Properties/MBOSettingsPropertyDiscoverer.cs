using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Abstractions.Settings.Models;
using MCM.Implementation.MBO.Attributes;
using MCM.Implementation.MBO.Settings.Definitions;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.MBO.Settings.Properties
{
    [Version("e1.0.0",  2)]
    [Version("e1.0.1",  2)]
    [Version("e1.0.2",  2)]
    [Version("e1.0.3",  2)]
    [Version("e1.0.4",  2)]
    [Version("e1.0.5",  2)]
    [Version("e1.0.6",  2)]
    [Version("e1.0.7",  2)]
    [Version("e1.0.8",  2)]
    [Version("e1.0.9",  2)]
    [Version("e1.0.10", 2)]
    [Version("e1.0.11", 2)]
    [Version("e1.1.0",  2)]
    [Version("e1.2.0",  2)]
    [Version("e1.2.1",  2)]
    [Version("e1.3.0",  2)]
    [Version("e1.3.1",  2)]
    [Version("e1.4.0",  2)]
    [Version("e1.4.1",  2)]
    internal class MBOSettingsPropertyDiscoverer : IMBOSettingsPropertyDiscoverer
    {
        public IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object)
        {
            foreach (var propertyDefinition in GetPropertiesInternal(@object))
            {
                SettingsUtils.CheckIsValid(propertyDefinition, @object);
                yield return propertyDefinition;
            }
        }

        private static IEnumerable<ISettingsPropertyDefinition> GetPropertiesInternal(object @object)
        {
            var type = @object.GetType();

            var subGroupDelimiter = AccessTools.Property(type, "SubGroupDelimiter")?.GetValue(@object) as char? ?? '/';

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = property.GetCustomAttributes().ToList();

                object? groupAttrObj = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyGroupAttribute") ??
                                      attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyGroupDefinition"));
                var groupDefinition = groupAttrObj != null
                    ? new MBOPropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                object? propAttr = null;
                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyAttribute") ??
                           attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v1.SettingPropertyAttribute");
                if (propAttr != null)
                    propertyDefinitions.Add(new MBOSettingPropertyAttributeWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyBoolAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionBool"));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionBoolWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyFloatingIntegerAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionFloatingInteger"));
                if (propAttr != null)
                    propertyDefinitions.Add(new MBOPropertyDefinitionFloatingIntegerWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyIntegerAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionInteger"));
                if (propAttr != null)
                    propertyDefinitions.Add(new MBOPropertyDefinitionIntegerWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyTextAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionText"));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionTextWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyDropdownAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionDropdown"));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionDropdownWrapper(propAttr));

                if (propertyDefinitions.Count > 0)
                    yield return new SettingsPropertyDefinition(
                        propertyDefinitions,
                        groupDefinition,
                        new PropertyRef(property, @object),
                        subGroupDelimiter);
            }
        }
    }
}