using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace MCM.Implementation
{
    public abstract class BaseJsonSettingsFormat : ISettingsFormat
    {
        private readonly object _lock = new();
        protected readonly Dictionary<string, object?> _existingObjects = new();

        protected readonly IBUTRLogger Logger;
        protected virtual JsonSerializerSettings JsonSerializerSettings { get; }

        protected BaseJsonSettingsFormat(IBUTRLogger logger)
        {
            Logger = logger;
            JsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters =
                {
                    new BaseSettingsJsonConverter(logger, AddSerializationProperty, ClearSerializationProperties),
                    new DropdownJsonConverter(logger, GetSerializationProperty),
                }
            };
        }

        public virtual IEnumerable<string> FormatTypes { get; } = new[] { "json" };

        public string SaveJson(BaseSettings settings)
        {
            return JsonConvert.SerializeObject(settings, JsonSerializerSettings);
        }

        public BaseSettings LoadFromJson(BaseSettings settings, string content)
        {
            TryLoadFromJson(ref settings, content);
            return settings;
        }
        protected bool TryLoadFromJson(ref BaseSettings settings, string content)
        {
            lock (_lock)
            {
                try
                {
                    if (!TryParse(content, out var jo))
                        return false;

                    using var reader = jo.CreateReader();
                    var serializer = JsonSerializer.CreateDefault(JsonSerializerSettings);
                    var settingsType = settings.GetType();
                    var settingsConverter = JsonSerializerSettings.Converters.OfType<BaseSettingsJsonConverter>().FirstOrDefault();
                    if (settingsConverter is not null && settingsConverter.CanConvert(settingsType))
                        settingsConverter.ReadJson(reader, settingsType, settings, serializer);
                }
                catch (JsonSerializationException)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool Save(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, $"{filename}.json");

            var content = SaveJson(settings);

            try
            {
                var file = new FileInfo(path);
                file.Directory?.Create();
                using var writer = file.CreateText();
                writer.Write(content);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public virtual BaseSettings Load(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, $"{filename}.json");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                try
                {
                    using var reader = file.OpenText();
                    var content = reader.ReadToEnd();
                    if (!TryLoadFromJson(ref settings, content))
                        Save(settings, directoryPath, filename);
                }
                catch (Exception) { }
            }
            else
            {
                Save(settings, directoryPath, filename);
            }

            return settings;
        }

        protected object? GetSerializationProperty(string path)
        {
            lock (_lock)
            {
                return _existingObjects.TryGetValue(path, out var value) ? value : null;
            }
        }

        protected void AddSerializationProperty(string path, object? value)
        {
            _existingObjects.Add(path, value);
        }

        protected void ClearSerializationProperties()
        {
            _existingObjects.Clear();
        }

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
    }
}