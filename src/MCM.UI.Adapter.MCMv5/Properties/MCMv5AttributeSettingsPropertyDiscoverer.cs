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

namespace MCM.UI.Adapter.MCMv5.Properties
{
    internal sealed class MCMv5AttributeSettingsPropertyDiscoverer : IAttributeSettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new[] { "mcm_v5_attributes" };

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

        private static IEnumerable<ISettingsPropertyDefinition> GetPropertiesInternal(object? @object)
        {
            if (@object is null) yield break;

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

                object? groupAttrObj = attributes.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyGroupDefinition"));
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

        private static IEnumerable<IPropertyDefinitionBase> GetPropertyDefinitionWrappers(IReadOnlyCollection<object> properties)
        {
            object? propAttr;

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionBool"));
            if (propAttr is not null)
                yield return new PropertyDefinitionBoolWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionDropdown"));
            if (propAttr is not null)
                yield return new PropertyDefinitionDropdownWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionGroupToggle"));
            if (propAttr is not null)
                yield return new PropertyDefinitionGroupToggleWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionText"));
            if (propAttr is not null)
                yield return new PropertyDefinitionTextWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionWithActionFormat"));
            if (propAttr is not null)
                yield return new PropertyDefinitionWithActionFormatWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionWithCustomFormatter"));
            if (propAttr is not null)
                yield return new PropertyDefinitionWithCustomFormatterWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionWithEditableMinMax"));
            if (propAttr is not null)
                yield return new PropertyDefinitionWithEditableMinMaxWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionWithFormat"));
            if (propAttr is not null)
                yield return new PropertyDefinitionWithFormatWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionWithId"));
            if (propAttr is not null)
                yield return new PropertyDefinitionWithIdWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionWithMinMax"));
            if (propAttr is not null)
                yield return new PropertyDefinitionWithMinMaxWrapper(propAttr);

            propAttr = properties.SingleOrDefault(a => ReflectionUtils.ImplementsEquivalentInterface(a.GetType(), "MCM.Abstractions.IPropertyDefinitionButton"));
            if (propAttr is not null)
                yield return new PropertyDefinitionButtonWrapper(propAttr);
        }
    }
}