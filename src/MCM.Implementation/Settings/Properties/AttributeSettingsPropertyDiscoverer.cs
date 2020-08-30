using HarmonyLib;

using MCM.Abstractions.Attributes;
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
using MCM.Abstractions.Settings.Base;

namespace MCM.Implementation.Settings.Properties
{
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
                if (property.Name == nameof(BaseSettings.Id))
                    continue;
                if (property.Name == nameof(BaseSettings.DisplayName))
                    continue;
                if (property.Name == nameof(BaseSettings.FolderName))
                    continue;
                if (property.Name == nameof(BaseSettings.Format))
                    continue;
                if (property.Name == nameof(BaseSettings.SubFolder))
                    continue;
                if (property.Name == nameof(BaseSettings.UIVersion))
                    continue;

                var attributes = property.GetCustomAttributes().ToList();

                object? groupAttrObj = attributes.SingleOrDefault(a => a is IPropertyGroupDefinition);
                var groupDefinition = groupAttrObj != null
                    ? new PropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = GetPropertyDefinitionWrappers(attributes).ToList();
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

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionBool);
            if (propAttr != null)
                yield return new PropertyDefinitionBoolWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionDropdown);
            if (propAttr != null)
                yield return new PropertyDefinitionDropdownWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionGroupToggle);
            if (propAttr != null)
                yield return new PropertyDefinitionGroupToggleWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionText);
            if (propAttr != null)
                yield return new PropertyDefinitionTextWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithActionFormat);
            if (propAttr != null)
                yield return new PropertyDefinitionWithActionFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithCustomFormatter);
            if (propAttr != null)
                yield return new PropertyDefinitionWithCustomFormatterWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithEditableMinMax);
            if (propAttr != null)
                yield return new PropertyDefinitionWithEditableMinMaxWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithFormat);
            if (propAttr != null)
                yield return new PropertyDefinitionWithFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithId);
            if (propAttr != null)
                yield return new PropertyDefinitionWithIdWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is IPropertyDefinitionWithMinMax);
            if (propAttr != null)
                yield return new PropertyDefinitionWithMinMaxWrapper(propAttr);
        }
    }
}