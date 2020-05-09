using HarmonyLib;

using MCM.Abstractions;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Data;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Formats;
using MCM.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MCM.Implementation.Settings.Formats
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
    internal class JsonSettingsFormat : ISettingsFormat
    {
        protected readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new IgnorePropertiesResolver(),
            Converters = { new DropdownJsonConverter() }
        };

        public virtual IEnumerable<string> Extensions => new string[] { "json" };

        public virtual bool Save(BaseSettings settings, string path)
        {
            var content = settings is IWrapper wrapper
                ? JsonConvert.SerializeObject(wrapper.Object, _jsonSerializerSettings)
                : JsonConvert.SerializeObject(settings, _jsonSerializerSettings);

            var file = new FileInfo(path);
            file.Directory?.Create();
            using var writer = file.CreateText();
            writer.Write(content);

            return true;
        }

        public virtual BaseSettings? Load(BaseSettings settings, string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                try
                {
                    using var reader = file.OpenText();
                    var content = reader.ReadToEnd();
                    if (settings is IWrapper wrapper)
                        JsonConvert.PopulateObject(content, wrapper.Object, _jsonSerializerSettings);
                    else
                        JsonConvert.PopulateObject(content, settings, _jsonSerializerSettings);
                }
                catch (JsonException)
                {
                    Save(settings, path);
                }
            }
            else
            {
                Save(settings, path);
            }
            return settings;
        }


        private class IgnorePropertiesResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.ShouldSerialize = _ => SettingsUtils.PropertyIsSetting(property.AttributeProvider.GetAttributes(true));
                return property;
            }
        }

        private class DropdownJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => ReflectionUtils.ImplementsOrImplementsEquivalent(objectType, typeof(IDropdownProvider));

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var selectedIndexProperty = AccessTools.Property(value.GetType(), "SelectedIndex");
                var jo = new JObject
                {
                    { "SelectedIndex", selectedIndexProperty?.GetValue(value) as int? ?? 0 },
                };
                jo.WriteTo(writer);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var selectedIndexProperty = AccessTools.Property(existingValue.GetType(), "SelectedIndex");
                try
                {
                    var jo = JObject.Load(reader);
                    var index = selectedIndexProperty?.GetValue(existingValue) as int? ?? 0;
                    selectedIndexProperty?.SetValue(existingValue, int.TryParse(jo["SelectedIndex"].ToString(), out var res) ? res : index);
                }
                catch (Exception) { }
                return existingValue;
            }
        }
    }
}