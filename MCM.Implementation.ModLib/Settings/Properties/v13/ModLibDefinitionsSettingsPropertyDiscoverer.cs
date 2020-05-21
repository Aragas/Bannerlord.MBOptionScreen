using MCM.Abstractions.Attributes;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Models;
using MCM.Implementation.ModLib.Attributes.v13;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.ModLib.Settings.Properties.v13
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    [Version("e1.3.1",  1)]
    [Version("e1.4.0",  1)]
    [Version("e1.4.1",  1)]
    internal class ModLibDefinitionsSettingsPropertyDiscoverer : IModLibDefinitionsSettingsPropertyDiscoverer
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
            foreach (var property in @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = property.GetCustomAttributes().ToList();

                object? groupAttrObj = attributes.FirstOrDefault(a => a.GetType().FullName == "ModLib.Definitions.Attributes.SettingPropertyGroupAttribute");
                var groupDefinition = groupAttrObj != null
                    ? new ModLibDefinitionsPropertyGroupDefinitionWrapper(groupAttrObj)
                    : SettingPropertyGroupAttribute.Default;

                object? propAttr = attributes.FirstOrDefault(a => a.GetType().FullName == "ModLib.Definitions.Attributes.SettingPropertyAttribute");

                if (propAttr != null)
                    yield return new SettingsPropertyDefinition(new ModLibDefinitionsSettingPropertyAttributeWrapper(propAttr), groupDefinition, new PropertyRef(property, @object));
            }
        }
    }
}