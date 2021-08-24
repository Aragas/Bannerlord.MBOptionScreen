extern alias v3;
extern alias v4;

using HarmonyLib.BUTR.Extensions;

using MCM.Adapter.MCMv3.Settings.Definitions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using v4::MCM.Abstractions;
using v4::MCM.Abstractions.Attributes;
using v4::MCM.Abstractions.Ref;
using v4::MCM.Abstractions.Settings.Base;
using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;
using v4::MCM.Abstractions.Settings.Models;
using v4::MCM.Abstractions.Settings.Properties;
using v4::MCM.Utils;

namespace MCM.Adapter.MCMv3.Settings.Properties
{
    internal sealed class MCMv3SettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new[] { "mcm_v3_attributes" };

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
                var groupDefinition = groupAttrObj is not null
                    ? new MCMv3PropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                var propertyDefinitionWrappers = GetPropertyDefinitionWrappers(attributes).ToList();
                if (propertyDefinitionWrappers.Count > 0)
                {
                    propertyDefinitions.AddRange(propertyDefinitionWrappers);

                    if (groupDefinition is MCMv3PropertyGroupDefinitionWrapper { IsMainToggle: true })
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

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionBool);
            if (propAttr is not null)
                yield return new MCMv3PropertyDefinitionBoolWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionDropdown);
            if (propAttr is not null)
                yield return new MCMv3PropertyDefinitionDropdownWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionText);
            if (propAttr is not null)
                yield return new MCMv3PropertyDefinitionTextWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithCustomFormatter);
            if (propAttr is not null)
                yield return new MCMv3PropertyDefinitionWithCustomFormatterWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithFormat);
            if (propAttr is not null)
                yield return new MCMv3PropertyDefinitionWithFormatWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v3::MCM.Abstractions.Settings.Definitions.IPropertyDefinitionWithMinMax);
            if (propAttr is not null)
                yield return new MCMv3PropertyDefinitionWithMinMaxWrapper(propAttr);
        }
    }
}