using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Formats;
using MCM.Utils;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;

namespace MCM.Implementation.Settings.Formats
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
            lock (_lock)
            {
                JsonConvert.PopulateObject(content, settings, JsonSerializerSettings);
            }

            return settings;
        }

        public virtual bool Save(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, $"{filename}.json");

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
            var path = Path.Combine(directoryPath, $"{filename}.json");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                var reader = file.OpenText();
                var content = reader.ReadToEnd();
                reader.Dispose();

                return LoadFromJson(settings, content);
            }
            else
            {
                Save(settings, directoryPath, filename);
                return settings;
            }
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
    }
}