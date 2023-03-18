using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.GameFeatures;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

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
    abstract class BaseJsonSettingsFormat : ISettingsFormat
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

        public virtual bool Save(BaseSettings settings, GameDirectory directory, string filename)
        {
            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider) return false;
            if (fileSystemProvider.GetOrCreateFile(directory, $"{filename}.json") is not { } file) return false;

            var content = SaveJson(settings);

            try
            {
                return fileSystemProvider.WriteData(file, Encoding.UTF8.GetBytes(content));
            }
            catch (Exception)
            {
                return false;
            }
        }
        public virtual BaseSettings Load(BaseSettings settings, GameDirectory directory, string filename)
        {
            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider) return settings;
            if (fileSystemProvider.GetFile(directory, $"{filename}.json") is not { } file) return settings;
            if (fileSystemProvider.ReadData(file) is not { } data)
            {
                Save(settings, directory, filename);
                return settings;
            }

            try
            {
                if (!TryLoadFromJson(ref settings, Encoding.UTF8.GetString(data)))
                    Save(settings, directory, filename);
            }
            catch (Exception) { }

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