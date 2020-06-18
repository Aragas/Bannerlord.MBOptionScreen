using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MCM.Implementation.Attributes;

namespace MCM.Implementation.Settings.Properties
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
    internal class AttributeSettingsPropertyDiscoverer : IAttributeSettingsPropertyDiscoverer
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

                object? groupAttrObj = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyGroupDefinition)));
                var groupDefinition = groupAttrObj != null
                    ? new AttributePropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                var propertyDefinitionWrappers = GetPropertyDefinitionWrappers(attributes).ToList();
                if (propertyDefinitionWrappers.Count > 0)
                {
                    propertyDefinitions.AddRange(propertyDefinitionWrappers);

                    if (groupDefinition is AttributePropertyGroupDefinitionWrapper groupWrapper && groupWrapper.IsMainToggle)
                        propertyDefinitions.Add(new AttributePropertyDefinitionGroupToggleWrapper(propertyDefinitions.First()));
                }

                if(propertyDefinitions.Count > 0)
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
            propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(SettingPropertyAttribute)));
            if (propAttr != null)
                yield return new SettingPropertyAttributeWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionBool)));
            if (propAttr != null)
                yield return new PropertyDefinitionBoolWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionWithMinMax)));
            if (propAttr != null)
                yield return new PropertyDefinitionWithMinMaxWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionWithFormat)));
            if (propAttr != null)
                yield return new PropertyDefinitionWithFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionWithActionFormat)));
            if (propAttr != null)
                yield return new PropertyDefinitionWithActionFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionWithCustomFormatter)));
            if (propAttr != null)
                yield return new PropertyDefinitionWithCustomFormatterWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionText)));
            if (propAttr != null)
                yield return new PropertyDefinitionTextWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionDropdown)));
            if (propAttr != null)
                yield return new PropertyDefinitionDropdownWrapper(propAttr);
        }
    }
}