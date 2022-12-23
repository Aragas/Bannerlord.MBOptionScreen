using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

namespace MCM.Implementation
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
        sealed class BaseSettingsJsonConverter : JsonConverter
    {
        private readonly IBUTRLogger _logger;
        private readonly Action<string, object?> _addSerializationProperty;
        private readonly Action _clearSerializationProperties;

        public BaseSettingsJsonConverter(IBUTRLogger logger, Action<string, object?> addSerializationProperty, Action clearSerializationProperties)
        {
            _logger = logger;
            _addSerializationProperty = addSerializationProperty;
            _clearSerializationProperties = clearSerializationProperties;
        }

        public override bool CanConvert(Type objectType) => objectType.IsSubclassOf(typeof(BaseSettings));

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not BaseSettings settings)
                return;

            var jo = new JObject();

            foreach (var definition in settings.GetAllSettingPropertyDefinitions())
            {
                if (definition.SettingType == SettingType.Button)
                    continue;

                jo.Add(definition.Id, definition.PropertyReference.Value is null ? null : JToken.FromObject(definition.PropertyReference.Value, serializer));
            }

            jo.WriteTo(writer);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (existingValue is not BaseSettings settings)
                return existingValue;

            try
            {
                var jo = JObject.Load(reader);

                _clearSerializationProperties();
                foreach (var definition in settings.GetAllSettingPropertyDefinitions())
                {
                    if (definition.SettingType == SettingType.Button)
                        continue;

                    if (jo.TryGetValue(definition.Id, out var value))
                    {
                        _addSerializationProperty(value.CreateReader().Path, definition.PropertyReference.Value);
                        definition.PropertyReference.Value = value.ToObject(definition.PropertyReference.Type, serializer);
                    }
                }
                _clearSerializationProperties();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while deserializing Settings");
            }
            return existingValue;
        }
    }
}