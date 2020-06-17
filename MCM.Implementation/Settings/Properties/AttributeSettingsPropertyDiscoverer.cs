using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.Settings.Properties
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
                    ? new PropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                object? propAttr = null;
                propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(SettingPropertyAttribute)));
                if (propAttr != null)
                    propertyDefinitions.Add(new SettingPropertyAttributeWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionBool)));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionBoolWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionWithMinMax)));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionWithMinMaxWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionWithFormat)));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionWithFormatWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionWithActionFormat)));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionWithActionFormatWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionWithCustomFormatter)));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionWithCustomFormatterWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionText)));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionTextWrapper(propAttr));

                propAttr = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionDropdown)));
                if (propAttr != null)
                    propertyDefinitions.Add(new PropertyDefinitionDropdownWrapper(propAttr));

                if(propertyDefinitions.Count > 0)
                    yield return new SettingsPropertyDefinition(
                        propertyDefinitions,
                        groupDefinition,
                        new PropertyRef(property, @object),
                        subGroupDelimiter);
            }
        }
    }
}