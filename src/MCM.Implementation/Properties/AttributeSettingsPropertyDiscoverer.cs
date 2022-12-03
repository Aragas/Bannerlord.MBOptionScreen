using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Base;
using MCM.Abstractions.Properties;
using MCM.Abstractions.Wrapper;
using MCM.Common;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation
{
    internal sealed class AttributeSettingsPropertyDiscoverer : IAttributeSettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new[] { "attributes" };

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings)
        {
            foreach (var propertyDefinition in GetPropertiesInternal(settings))
            {
                SettingsUtils.CheckIsValid(propertyDefinition, settings);
                yield return propertyDefinition;
            }
        }

        private static IEnumerable<ISettingsPropertyDefinition> GetPropertiesInternal(BaseSettings settings)
        {
            var type = settings.GetType();
            var subGroupDelimiter = AccessTools2.Property(type, "SubGroupDelimiter")?.GetValue(settings) as char? ?? '/';
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

                var propertyDefinitions = SettingsUtils.GetPropertyDefinitionWrappers(attributes).ToList();
                if (propertyDefinitions.Count > 0)
                {
                    yield return new SettingsPropertyDefinition(propertyDefinitions,
                        groupDefinition,
                        new PropertyRef(property, settings),
                        subGroupDelimiter);
                }
            }
        }
    }
}