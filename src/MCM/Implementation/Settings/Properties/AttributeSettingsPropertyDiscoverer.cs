﻿using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.Settings.Properties
{
    internal sealed class AttributeSettingsPropertyDiscoverer : IAttributeSettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new [] { "attributes" };

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
            var subGroupDelimiter = AccessTools2.Property(type, "SubGroupDelimiter")?.GetValue(@object) as char? ?? '/';
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (property.Name == nameof(BaseSettings.Id))
                    continue;
                if (property.Name == nameof(BaseSettings.DisplayName))
                    continue;
                if (property.Name == nameof(BaseSettings.FolderName))
                    continue;
                if (property.Name == nameof(BaseSettings.FormatType))
                    continue;
                if (property.Name == nameof(BaseSettings.SubFolder))
                    continue;
                if (property.Name == nameof(BaseSettings.UIVersion))
                    continue;

                var attributes = property.GetCustomAttributes().ToList();

                object? groupAttrObj = attributes.SingleOrDefault(a => a is IPropertyGroupDefinition);
                var groupDefinition = groupAttrObj is not null
                    ? new PropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = GetPropertyDefinitionWrappers(attributes).ToList();
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

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionBool);
            if (propAttr is not null)
                yield return new PropertyDefinitionBoolWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionDropdown);
            if (propAttr is not null)
                yield return new PropertyDefinitionDropdownWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionGroupToggle);
            if (propAttr is not null)
                yield return new PropertyDefinitionGroupToggleWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionText);
            if (propAttr is not null)
                yield return new PropertyDefinitionTextWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithActionFormat);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithActionFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithCustomFormatter);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithCustomFormatterWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithEditableMinMax);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithEditableMinMaxWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithFormat);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithId);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithIdWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithMinMax);
            if (propAttr is not null)
                yield return new PropertyDefinitionWithMinMaxWrapper(propAttr);
        }
    }
}