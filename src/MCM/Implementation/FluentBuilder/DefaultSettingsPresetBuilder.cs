using MCM.Abstractions.FluentBuilder;

using System.Collections.Generic;

namespace MCM.Implementation.FluentBuilder
{
    internal sealed class DefaultSettingsPresetBuilder : ISettingsPresetBuilder
    {
        public IDictionary<string, object> PropertyValues { get; } = new Dictionary<string, object>();

        public string PresetName { get; }

        public DefaultSettingsPresetBuilder(string presetName)
        {
            PresetName = presetName;
        }

        public ISettingsPresetBuilder SetPropertyValue(string property, object value)
        {
            if (!PropertyValues.ContainsKey(property))
                PropertyValues[property] = value;
            return this;
        }
    }
}