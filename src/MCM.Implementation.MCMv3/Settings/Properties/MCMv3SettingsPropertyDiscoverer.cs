extern alias v4;

using HarmonyLib;

using MCM.Implementation.MCMv3.Attributes;
using MCM.Implementation.MCMv3.Settings.Definitions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using v4::MCM.Abstractions.Attributes;
using v4::MCM.Abstractions.Ref;
using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;
using v4::MCM.Abstractions.Settings.Models;
using v4::MCM.Utils;

namespace MCM.Implementation.MCMv3.Settings.Properties
{
    [Version("e1.0.0",  3)]
    [Version("e1.0.1",  3)]
    [Version("e1.0.2",  3)]
    [Version("e1.0.3",  3)]
    [Version("e1.0.4",  3)]
    [Version("e1.0.5",  3)]
    [Version("e1.0.6",  3)]
    [Version("e1.0.7",  3)]
    [Version("e1.0.8",  3)]
    [Version("e1.0.9",  3)]
    [Version("e1.0.10", 3)]
    [Version("e1.0.11", 3)]
    [Version("e1.1.0",  3)]
    [Version("e1.2.0",  3)]
    [Version("e1.2.1",  3)]
    [Version("e1.3.0",  3)]
    [Version("e1.3.1",  3)]
    [Version("e1.4.0",  3)]
    [Version("e1.4.1",  3)]
    internal class MCMv3SettingsPropertyDiscoverer : IMCMv3SettingsPropertyDiscoverer
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
                    ? new MCMv3PropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                var propertyDefinitionWrappers = GetPropertyDefinitionWrappers(attributes).ToList();
                if (propertyDefinitionWrappers.Count > 0)
                {
                    propertyDefinitions.AddRange(propertyDefinitionWrappers);

                    if (groupDefinition is MCMv3PropertyGroupDefinitionWrapper groupWrapper && groupWrapper.IsMainToggle)
                        propertyDefinitions.Add(new AttributePropertyDefinitionGroupToggleWrapper(propertyDefinitions.First()));
                }

                if (propertyDefinitions.Count > 0)
                    yield return new SettingsPropertyDefinition(
                        propertyDefinitions,
                        groupDefinition,
                        new PropertyRef(property, @object),
                        subGroupDelimiter);
            }
        }

        private static IEnumerable<IPropertyDefinitionBase> GetPropertyDefinitionWrappers(IReadOnlyCollection<Attribute> attributes)
        {
                object? propAttr = null;
                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyAttribute") ??
                           attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v1.SettingPropertyAttribute");
                if (propAttr != null)
                    yield return new MCMv3SettingPropertyAttributeWrapper(propAttr);

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyBoolAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionBool"));
                if (propAttr != null)
                    yield return new PropertyDefinitionBoolWrapper(propAttr);

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyFloatingIntegerAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionFloatingInteger"));
                if (propAttr != null)
                    yield return new MCMv3PropertyDefinitionFloatingIntegerWrapper(propAttr);

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyIntegerAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionInteger"));
                if (propAttr != null)
                    yield return new MCMv3PropertyDefinitionIntegerWrapper(propAttr);

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyTextAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionText"));
                if (propAttr != null)
                    yield return new PropertyDefinitionTextWrapper(propAttr);

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyDropdownAttribute") ??
                           attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyDefinitionDropdown"));
                if (propAttr != null)
                    yield return new PropertyDefinitionDropdownWrapper(propAttr);
        }
    }
}