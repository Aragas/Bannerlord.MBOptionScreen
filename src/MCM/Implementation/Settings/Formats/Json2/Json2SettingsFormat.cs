using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Common;
using MCM.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

namespace MCM.Implementation.Settings.Formats.Json2
{
    internal sealed class Json2SettingsFormat : BaseJsonSettingsFormat, IJson2SettingsFormat
    {
        public override IEnumerable<string> FormatTypes => new[] { "json2" };
        protected override JsonSerializerSettings JsonSerializerSettings { get; }

        public Json2SettingsFormat(IBUTRLogger<Json2SettingsFormat> logger) : base(logger)
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = { new DropdownJson2Converter(logger, this) }
            };
        }

        private class DropdownJson2Converter : JsonConverter
        {
            private readonly IBUTRLogger _logger;
            private readonly BaseJsonSettingsFormat _settingsFormat;

            public DropdownJson2Converter(IBUTRLogger logger, BaseJsonSettingsFormat settingsFormat)
            {
                _logger = logger;
                _settingsFormat = settingsFormat;
            }

            public override bool CanConvert(Type objectType) => SettingsUtils.IsForGenericDropdown(objectType);

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var wrapper = new SelectedIndexWrapper(value);
                var token = JToken.FromObject(wrapper.SelectedIndex);
                token.WriteTo(writer);
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                try
                {
                    existingValue ??= _settingsFormat.FindExistingValue(reader.Path);
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
}