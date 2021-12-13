﻿extern alias v13;

using MCM.Abstractions;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;
using MCM.Adapter.ModLib.Attributes.v13;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Adapter.ModLib.Settings.Properties.v13
{
    internal sealed class ModLibDefinitionsSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new[] { "modlib_v13_attributes" };

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings)
        {
            var obj = settings switch
            {
                IWrapper wrapper => wrapper.Object,
                _ => settings
            };

            foreach (var propertyDefinition in GetPropertiesInternal(obj))
            {
                SettingsUtils.CheckIsValid(propertyDefinition, obj);
                yield return propertyDefinition;
            }
        }

        private static IEnumerable<ISettingsPropertyDefinition> GetPropertiesInternal(object @object)
        {
            var type = @object.GetType();

            const char subGroupDelimiter = '/';

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (property.Name == nameof(v13::ModLib.Definitions.SettingsBase.ID))
                    continue;
                if (property.Name == nameof(v13::ModLib.Definitions.SettingsBase.ModName))
                    continue;
                if (property.Name == nameof(v13::ModLib.Definitions.SettingsBase.ModuleFolderName))
                    continue;
                if (property.Name == nameof(v13::ModLib.Definitions.SettingsBase.SubFolder))
                    continue;

                var attributes = property.GetCustomAttributes().ToList();

                object? groupAttrObj = attributes.Find(a => a is v13::ModLib.Definitions.Attributes.SettingPropertyGroupAttribute);
                var groupDefinition = groupAttrObj is not null
                    ? new ModLibDefinitionsPropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                var propertyDefinitionWrappers = GetPropertyDefinitionWrappers(attributes).ToList();
                if (propertyDefinitionWrappers.Count > 0)
                {
                    propertyDefinitions.AddRange(propertyDefinitionWrappers);

                    if (groupDefinition is ModLibDefinitionsPropertyGroupDefinitionWrapper { IsMainToggle: true })
                        propertyDefinitions.Add(new AttributePropertyDefinitionGroupToggleWrapper(propertyDefinitions[0]));
                }

                if (propertyDefinitions.Count > 0)
                {
                    yield return new SettingsPropertyDefinition(propertyDefinitions,
                        groupDefinition,
                        new PropertyRef(property, @object),
                        subGroupDelimiter);
                }
            }
        }

        private static IEnumerable<IPropertyDefinitionBase> GetPropertyDefinitionWrappers(IReadOnlyCollection<Attribute> attributes)
        {
            object? propAttr;

            propAttr = attributes.FirstOrDefault(a => a is v13::ModLib.Definitions.Attributes.SettingPropertyAttribute);
            if (propAttr is not null)
                yield return new ModLibDefinitionsSettingPropertyAttributeWrapper(propAttr);
        }
    }
}