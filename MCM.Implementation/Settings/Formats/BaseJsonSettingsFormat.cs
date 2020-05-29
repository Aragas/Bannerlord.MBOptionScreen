using HarmonyLib;

using MCM.Abstractions;
using MCM.Abstractions.Settings.Base;
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
    public class BaseJsonSettingsFormat : ISettingsFormat
    {
        protected readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new IgnorePropertiesResolver(),
            Converters = { new DropdownJsonConverter() }
        };

        public virtual IEnumerable<string> Extensions => new [] { "json" };

        public virtual bool Save(BaseSettings settings, string path)
        {
            var content = settings is IWrapper wrapper
                ? JsonConvert.SerializeObject(wrapper.Object, JsonSerializerSettings)
                : JsonConvert.SerializeObject(settings, JsonSerializerSettings);

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
                        JsonConvert.PopulateObject(content, wrapper.Object, JsonSerializerSettings);
                    else
                        JsonConvert.PopulateObject(content, settings, JsonSerializerSettings);
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
            public override bool CanConvert(Type objectType) => SettingsUtils.IsDropdown(objectType);

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