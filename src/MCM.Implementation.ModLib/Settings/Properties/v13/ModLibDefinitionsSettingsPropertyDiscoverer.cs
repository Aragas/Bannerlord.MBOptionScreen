extern alias v13;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;
using MCM.Implementation.ModLib.Attributes.v13;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.ModLib.Settings.Properties.v13
{
    internal class ModLibDefinitionsSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
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

            const char subGroupDelimiter = '/';

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = property.GetCustomAttributes().ToList();

                object? groupAttrObj = attributes.Find(a => a is v13::ModLib.Definitions.Attributes.SettingPropertyGroupAttribute);
                var groupDefinition = groupAttrObj != null
                    ? new ModLibDefinitionsPropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                var propertyDefinitionWrappers = GetPropertyDefinitionWrappers(attributes).ToList();
                if (propertyDefinitionWrappers.Count > 0)
                {
                    propertyDefinitions.AddRange(propertyDefinitionWrappers);

                    if (groupDefinition is ModLibDefinitionsPropertyGroupDefinitionWrapper groupWrapper && groupWrapper.IsMainToggle)
                        propertyDefinitions.Add(new AttributePropertyDefinitionGroupToggleWrapper(propertyDefinitions.First()));
                }

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

            propAttr = attributes.FirstOrDefault(a => a is v13::ModLib.Definitions.Attributes.SettingPropertyAttribute);
            if (propAttr != null)
                yield return new ModLibDefinitionsSettingPropertyAttributeWrapper(propAttr);
        }
    }
}