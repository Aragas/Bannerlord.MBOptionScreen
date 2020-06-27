using HarmonyLib;

using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Models;
using MCM.Extensions;
using MCM.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;

namespace MCM.Implementation.Settings.Formats
{
    public class BaseJsonSettingsFormat : ISettingsFormat
    {
        private readonly object _lock = new object();
        private readonly Dictionary<string, object> _existingObjects = new Dictionary<string, object>();

        protected readonly JsonSerializerSettings JsonSerializerSettings;

        protected BaseJsonSettingsFormat()
        {
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                Converters = { new DropdownJsonConverter(this) }
            };
        }

        public virtual IEnumerable<string> Extensions => new [] { "json" };

        protected static string GetPropertyDefinitionId(ISettingsPropertyDefinition definition) =>
            AccessTools.Property(definition.GetType(), "Id")?.GetValue(definition) as string ??
            definition.PropertyReference switch
            {
                PropertyRef propertyRef => propertyRef.PropertyInfo.Name,
                _ => $"{definition.GroupName}|{definition.DisplayName}",
            };

        protected string SaveJson(BaseSettings settings)
        {
            var jo = new JObject();
            var serializer = JsonSerializer.CreateDefault(JsonSerializerSettings);

            foreach (var definition in settings.GetAllSettingPropertyDefinitions())
            {
                var id = GetPropertyDefinitionId(definition);

                jo.Add(id, JToken.FromObject(definition.PropertyReference.Value, serializer));
            }

            return JsonConvert.SerializeObject(jo, JsonSerializerSettings);
        }
        // TODO: I can't figure a way to trigger Populate the way I want.
        // It seems not possible to populate a dictionary with a dropdown
        // The way it's serialized is that it relies on the existence of the dropdown
        // And it only sets the SelectedIndex property, without knowing of the other values
        // When I use the standard Populate method for some reason the existing dropdown
        // Gets replaced with a JToken object that ony contains the SelectedIndex.
        // An exception for the dropdown could be made, but a generic approach would be better
        protected BaseSettings? LoadFromJson(BaseSettings settings, string content)
        {
            lock (_lock)
            {
                var jo = JObject.Parse(content);
                var serializer = JsonSerializer.CreateDefault(JsonSerializerSettings);

                _existingObjects.Clear();
                foreach (var definition in settings.GetAllSettingPropertyDefinitions())
                {
                    var id = GetPropertyDefinitionId(definition);

                    if (jo.TryGetValue(id, out var value))
                    {
                        _existingObjects.Add(value.CreateReader().Path, definition.PropertyReference.Value);
                        definition.PropertyReference.Value = value.ToObject(definition.PropertyReference.Type, serializer);
                    }
                }
                _existingObjects.Clear();
            }

            return settings;
        }

        public virtual bool Save(BaseSettings settings, string path)
        {
            var content = SaveJson(settings);

            var file = new FileInfo(path);
            file.Directory?.Create();
            var writer = file.CreateText();
            writer.Write(content);
            writer.Dispose();

            return true;
        }
        public virtual BaseSettings? Load(BaseSettings settings, string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                var reader = file.OpenText();
                var content = reader.ReadToEnd();
                reader.Dispose();

                var set = LoadFromJson(settings, content);
                if (set == null)
                {
                    Save(settings, path);
                    return settings;
                }
                else
                    return set;
            }
            else
            {
                Save(settings, path);
                return settings;
            }
        }

        private object? FindExistingValue(string path) => _existingObjects.TryGetValue(path, out var value) ? value : null;


        private class DropdownJsonConverter : JsonConverter
        {
            private readonly BaseJsonSettingsFormat _settingsFormat;

            public DropdownJsonConverter(BaseJsonSettingsFormat settingsFormat)
            {
                _settingsFormat = settingsFormat;
            }

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

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                try
                {
                    var jo = JObject.Load(reader);

                    existingValue ??= _settingsFormat.FindExistingValue(reader.Path);
                    if (existingValue == null)
                        return null;

                    var selectedIndexProperty = AccessTools.Property(existingValue.GetType(), "SelectedIndex");

                    var index = selectedIndexProperty?.GetValue(existingValue) as int? ?? 0;
                    selectedIndexProperty?.SetValue(existingValue, int.TryParse(jo["SelectedIndex"].ToString(), out var res) ? res : index);
                }
                catch (Exception) { }
                return existingValue;
            }
        }
    }
}