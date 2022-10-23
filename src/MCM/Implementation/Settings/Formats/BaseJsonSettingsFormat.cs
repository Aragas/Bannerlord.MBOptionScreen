using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Common;
using MCM.Abstractions.Common.Wrappers;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Models;
using MCM.Extensions;
using MCM.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MCM.Implementation.Settings.Formats
{
    internal abstract class BaseJsonSettingsFormat : ISettingsFormat
    {
        private readonly object _lock = new();
        private readonly Dictionary<string, object?> _existingObjects = new();

        protected readonly IBUTRLogger Logger;
        protected virtual JsonSerializerSettings JsonSerializerSettings { get; }

        protected BaseJsonSettingsFormat(IBUTRLogger logger)
        {
            Logger = logger;
            JsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = { new DropdownJsonConverter(logger, this) }
            };
        }

        public virtual IEnumerable<string> FormatTypes { get; } = new[] { "json" };

        protected static string GetPropertyDefinitionId(ISettingsPropertyDefinition definition) => definition.Id;

        private static bool TryParse(string content, [NotNullWhen(true)] out JObject? jObject)
        {
            try
            {
                jObject = JObject.Parse(content);
                return true;
            }
            catch (JsonReaderException)
            {
                jObject = null;
                return false;
            }
        }

        public string SaveJson(BaseSettings settings)
        {
            var jo = new JObject();
            var serializer = JsonSerializer.CreateDefault(JsonSerializerSettings);

            foreach (var definition in settings.GetAllSettingPropertyDefinitions())
            {
                if (definition.SettingType == SettingType.Button)
                    continue;

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
        public BaseSettings LoadFromJson(BaseSettings settings, string content)
        {
            TryLoadFromJson(ref settings, content);
            return settings;
        }
        protected bool TryLoadFromJson(ref BaseSettings settings, string content)
        {
            lock (_lock)
            {
                if (!TryParse(content, out var jo))
                {
                    return false;
                }

                var serializer = JsonSerializer.CreateDefault(JsonSerializerSettings);

                _existingObjects.Clear();
                foreach (var definition in settings.GetAllSettingPropertyDefinitions())
                {
                    if (definition.SettingType == SettingType.Button)
                        continue;

                    var id = GetPropertyDefinitionId(definition);

                    if (jo.TryGetValue(id, out var value))
                    {
                        _existingObjects.Add(value.CreateReader().Path, definition.PropertyReference.Value);
                        definition.PropertyReference.Value = value.ToObject(definition.PropertyReference.Type, serializer);
                    }
                }
                _existingObjects.Clear();
            }

            return true;
        }

        public virtual bool Save(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, filename + ".json");

            var content = SaveJson(settings);

            var file = new FileInfo(path);
            file.Directory?.Create();
            var writer = file.CreateText();
            writer.Write(content);
            writer.Dispose();

            return true;
        }
        public virtual BaseSettings Load(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, filename + ".json");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                var reader = file.OpenText();
                var content = reader.ReadToEnd();
                reader.Dispose();

                if (!TryLoadFromJson(ref settings, content))
                    Save(settings, directoryPath, filename);
            }
            else
            {
                Save(settings, directoryPath, filename);
            }

            return settings;
        }

        public object? FindExistingValue(string path)
        {
            lock (_lock)
            {
                return _existingObjects.TryGetValue(path, out var value) ? value : null;
            }
        }

        private class DropdownJsonConverter : JsonConverter
        {
            private readonly IBUTRLogger _logger;
            private readonly BaseJsonSettingsFormat _settingsFormat;

            public DropdownJsonConverter(IBUTRLogger logger, BaseJsonSettingsFormat settingsFormat)
            {
                _logger = logger;
                _settingsFormat = settingsFormat;
            }

            public override bool CanConvert(Type objectType) => SettingsUtils.IsForGenericDropdown(objectType);

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var jo = new JObject { { "SelectedIndex", new SelectedIndexWrapper(value).SelectedIndex } };
                jo.WriteTo(writer);
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
                    wrapper.SelectedIndex = int.TryParse(token["SelectedIndex"].ToString(), out var res) ? res : wrapper.SelectedIndex;
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