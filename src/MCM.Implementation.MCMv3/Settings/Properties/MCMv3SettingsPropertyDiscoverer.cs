extern alias v3;
extern alias v4;

using HarmonyLib;

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
using v4::MCM.Abstractions.Settings.Properties;
using v4::MCM.Utils;

namespace MCM.Implementation.MCMv3.Settings.Properties
{
    internal sealed class MCMv3SettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
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
                if (property.Name == nameof(v3::MCM.Abstractions.Settings.Base.BaseSettings.Id))
                    continue;
                if (property.Name == nameof(v3::MCM.Abstractions.Settings.Base.BaseSettings.DisplayName))
                    continue;
                if (property.Name == nameof(v3::MCM.Abstractions.Settings.Base.BaseSettings.FolderName))
                    continue;
                if (property.Name == nameof(v3::MCM.Abstractions.Settings.Base.BaseSettings.Format))
                    continue;
                if (property.Name == nameof(v3::MCM.Abstractions.Settings.Base.BaseSettings.SubFolder))
                    continue;
                if (property.Name == nameof(v3::MCM.Abstractions.Settings.Base.BaseSettings.UIVersion))
                    continue;

                var attributes = property.GetCustomAttributes().ToList();

                object? groupAttrObj = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyGroupDefinition);
                var groupDefinition = groupAttrObj != null
                    ? new MCMv3PropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                var propertyDefinitionWrappers = GetPropertyDefinitionWrappers(attributes).ToList();
                if (propertyDefinitionWrappers.Count > 0)
                {
                    propertyDefinitions.AddRange(propertyDefinitionWrappers);

                    if (groupDefinition is MCMv3PropertyGroupDefinitionWrapper groupWrapper && groupWrapper.IsMainToggle)
                        propertyDefinitions.Add(new AttributePropertyDefinitionGroupToggleWrapper(propertyDefinitions[0]));
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
            object? propAttr;

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionBool);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionBoolWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionDropdown);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionDropdownWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionText);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionTextWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithActionFormat);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionWithActionFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithCustomFormatter);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionWithCustomFormatterWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithEditableMinMax);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionWithEditableMinMaxWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithFormat);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionWithFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithId);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionWithIdWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithMinMax);
            if (propAttr != null)
                yield return new MCMv3PropertyDefinitionWithMinMaxWrapper(propAttr);
        }
    }
}