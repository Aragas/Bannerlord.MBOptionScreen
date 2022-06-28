using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

namespace MCM.Implementation
{
    public sealed class DropdownJsonConverter : JsonConverter
    {
        private readonly IBUTRLogger _logger;
        private readonly Func<string, object?> _getSerializationProperty;

        public DropdownJsonConverter(IBUTRLogger logger, Func<string, object?> getSerializationProperty)
        {
            _logger = logger;
            _getSerializationProperty = getSerializationProperty;
        }

        public override bool CanConvert(Type objectType) => SettingsUtils.IsForGenericDropdown(objectType);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var token = JToken.FromObject(new SelectedIndexWrapper(value).SelectedIndex);
            token.WriteTo(writer);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            try
            {
                existingValue ??= _getSerializationProperty(reader.Path);
                if (existingValue is null)
                    return null;

                var wrapper = new SelectedIndexWrapper(existingValue);
                var token = JToken.Load(reader);
                wrapper.SelectedIndex = int.TryParse(token.ToString(), out var res) ? res : wrapper.SelectedIndex;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while deserializing Dropdown");
            }
            return existingValue;
        }
    }
}