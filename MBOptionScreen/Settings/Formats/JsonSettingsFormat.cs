using MBOptionScreen.Attributes;
using MBOptionScreen.Data;
using MBOptionScreen.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen.Settings
{
    [Version("e1.0.0",  200)]
    [Version("e1.0.1",  200)]
    [Version("e1.0.2",  200)]
    [Version("e1.0.3",  200)]
    [Version("e1.0.4",  200)]
    [Version("e1.0.5",  200)]
    [Version("e1.0.6",  200)]
    [Version("e1.0.7",  200)]
    [Version("e1.0.8",  200)]
    [Version("e1.0.9",  200)]
    [Version("e1.0.10", 200)]
    [Version("e1.0.11", 200)]
    [Version("e1.1.0",  200)]
    [Version("e1.2.0",  200)]
    [Version("e1.2.1",  200)]
    [Version("e1.3.0",  200)]
    internal sealed class JsonSettingsFormat : ISettingsFormat
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new IgnorePropertiesResolver(),
            Converters = { new DropdownJsonConverter() }
        };

        public IEnumerable<string> Extensions => new string[] { "json" };

        public bool Save(SettingsBase settings, string path)
        {
            var content = settings is SettingsWrapper wrapperSettings
                ? JsonConvert.SerializeObject(wrapperSettings._object, _jsonSerializerSettings)
                : JsonConvert.SerializeObject(settings, _jsonSerializerSettings);

            var file = new FileInfo(path);
            file.Directory?.Create();
            using var writer = file.CreateText();
            writer.Write(content);

            return true;
        }

        public SettingsBase? Load(SettingsBase settings, string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                using var reader = file.OpenText();
                var content = reader.ReadToEnd();
                if (settings is SettingsWrapper wrapperSettings)
                    JsonConvert.PopulateObject(content, wrapperSettings._object, _jsonSerializerSettings);
                else
                    JsonConvert.PopulateObject(content, settings, _jsonSerializerSettings);
            }
            else
            {
                var content = settings is SettingsWrapper wrapperSettings
                    ? JsonConvert.SerializeObject(wrapperSettings._object, _jsonSerializerSettings)
                    : JsonConvert.SerializeObject(settings, _jsonSerializerSettings);
                file.Directory?.Create();
                using var writer = file.CreateText();
                writer.Write(content);
            }
            return settings;
        }


        public class IgnorePropertiesResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.ShouldSerialize = _ => property.AttributeProvider.GetAttributes(true).Any(a =>
                    ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(BaseSettingPropertyAttribute)));
                return property;
            }
        }

        public class DropdownJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => typeof(IDropdownProvider).IsAssignableFrom(objectType);

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var dropdown = (IDropdownProvider) value;
                var jo = new JObject
                {
                    { "SelectedIndex", dropdown.SelectedIndex },
                };
                jo.WriteTo(writer);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var dropdown = (IDropdownProvider) existingValue;
                try
                {
                    var jo = JObject.Load(reader);
                    dropdown.SelectedIndex = int.TryParse(jo["SelectedIndex"].ToString(), out var res) ? res : dropdown.SelectedIndex;
                }
                catch (Exception) { }
                return dropdown;
            }
        }
    }
}