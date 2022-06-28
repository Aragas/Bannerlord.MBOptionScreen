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
            var props = settings.GetAllSettingPropertyDefinitions().ToList();
            foreach (var kv in PropertyValues)
            {
                if (props.Find(x => x.Id == kv.Key) is { } property)
                    property.PropertyReference.Value = kv.Value;
            }
            return settings;
        });
    }
}