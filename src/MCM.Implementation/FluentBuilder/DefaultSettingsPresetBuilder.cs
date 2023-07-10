using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.FluentBuilder;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.FluentBuilder
{
    internal sealed class DefaultSettingsPresetBuilder : ISettingsPresetBuilder
    {
        private string Id { get; }
        private string Name { get; }
        private IDictionary<string, object?> PropertyValues { get; } = new Dictionary<string, object?>();

        public DefaultSettingsPresetBuilder(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public ISettingsPresetBuilder SetPropertyValue(string property, object? value)
        {
            if (!PropertyValues.ContainsKey(property))
                PropertyValues[property] = value;
            return this;
        }

        /// <inheritdoc />
        public ISettingsPreset Build(BaseSettings settings) => new MemorySettingsPreset(settings.Id, Id, Name, () =>
        {
            var settingsProperties = settings.GetAllSettingPropertyDefinitions().ToList();
            foreach (var overridePropertyKeyValue in PropertyValues)
            {
                var overridePropertyId = overridePropertyKeyValue.Key;
                var overridePropertyValue = overridePropertyKeyValue.Value;

                if (settingsProperties.Find(x => x.Id == overridePropertyId) is { } property)
                    property.PropertyReference.Value = overridePropertyValue;
            }
            return settings;
        });
    }
}