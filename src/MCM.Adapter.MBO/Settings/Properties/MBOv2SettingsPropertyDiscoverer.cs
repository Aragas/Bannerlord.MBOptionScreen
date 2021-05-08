extern alias v2;
extern alias v4;

using HarmonyLib.BUTR.Extensions;

using MCM.Adapter.MBO.Settings.Definitions;

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

namespace MCM.Adapter.MBO.Settings.Properties
{
    internal sealed class MBOv2SettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new [] { "mbo_v2_attributes" };

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
                if (property.Name == nameof(v2::MBOptionScreen.Settings.SettingsBase.Id))
                    continue;
                if (property.Name == nameof(v2::MBOptionScreen.Settings.SettingsBase.ModName))
                    continue;
                if (property.Name == nameof(v2::MBOptionScreen.Settings.SettingsBase.ModuleFolderName))
                    continue;
                if (property.Name == nameof(v2::MBOptionScreen.Settings.SettingsBase.UIVersion))
                    continue;
                if (property.Name == nameof(v2::MBOptionScreen.Settings.SettingsBase.SubFolder))
                    continue;
                if (property.Name == nameof(v2::MBOptionScreen.Settings.SettingsBase.Format))
                    continue;

                var attributes = property.GetCustomAttributes().ToList();

                object? groupAttrObj = attributes.SingleOrDefault(a => a is v2::MBOptionScreen.Settings.IPropertyGroupDefinition);
                var groupDefinition = groupAttrObj is not null
                    ? new MBOPropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                var propertyDefinitions = new List<IPropertyDefinitionBase>();

                var propertyDefinitionWrappers = GetPropertyDefinitionWrappers(attributes).ToList();
                if (propertyDefinitionWrappers.Count > 0)
                {
                    propertyDefinitions.AddRange(propertyDefinitionWrappers);

                    if (groupDefinition is MBOPropertyGroupDefinitionWrapper { IsMainToggle: true })
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

            propAttr = attributes.SingleOrDefault(a => a is v2::MBOptionScreen.Attributes.v1.SettingPropertyAttribute);
            if (propAttr is not null)
                yield return new MBOv1PropertyDefinitionWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v2::MBOptionScreen.Settings.IPropertyDefinitionBool);
            if (propAttr is not null)
                yield return new PropertyDefinitionBoolWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v2::MBOptionScreen.Settings.IPropertyDefinitionFloatingInteger);
            if (propAttr is not null)
                yield return new MBOPropertyDefinitionFloatingIntegerWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v2::MBOptionScreen.Settings.IPropertyDefinitionInteger);
            if (propAttr is not null)
                yield return new MBOPropertyDefinitionIntegerWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v2::MBOptionScreen.Settings.IPropertyDefinitionText);
            if (propAttr is not null)
                yield return new PropertyDefinitionTextWrapper(propAttr);

            propAttr = attributes.SingleOrDefault(a => a is v2::MBOptionScreen.Settings.IPropertyDefinitionDropdown);
            if (propAttr is not null)
                yield return new PropertyDefinitionDropdownWrapper(propAttr);
        }
    }
}