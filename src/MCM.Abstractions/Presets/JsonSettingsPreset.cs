using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.GameFeatures;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCM.Implementation
{
    /// <summary>
    /// A persistent preset that can be created at runtime
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class JsonSettingsPreset : ISettingsPreset
    {
        private class PresetContainerDefinition
        {
            [JsonProperty(Order = 1)]
            public string Id { get; set; } = string.Empty;

            [JsonProperty(Order = 2)]
            public string Name { get; set; } = string.Empty;
        }
        private sealed class PresetContainer : PresetContainerDefinition
        {
            [JsonProperty(Order = 3)]
            public BaseSettings? Settings { get; set; }
        }

        public static string? GetPresetId(string content)
        {
            var container = JsonConvert.DeserializeObject<PresetContainerDefinition?>(content);
            if (container is null) return null;

            return container.Id;
        }

        public static JsonSettingsPreset? FromFile(BaseSettings settings, GameFile file) => FromFile(settings.Id, file, settings.CreateNew);
        public static JsonSettingsPreset? FromFile(string settingsId, GameFile file, Func<BaseSettings> getNewSettings)
        {
            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider) return null;
            if (fileSystemProvider.ReadData(file) is not { } data) return null;

            var content = Encoding.UTF8.GetString(data);

            var container = JsonConvert.DeserializeObject<PresetContainerDefinition?>(content);
            if (container is null) return null;

            return new JsonSettingsPreset(settingsId, container.Id, container.Name, file, getNewSettings);
        }

        /// <inheritdoc />
        public string SettingsId { get; }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        private readonly GameFile _file;
        private readonly Func<BaseSettings> _getNewSettings;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly object _lock = new();
        private readonly Dictionary<string, object?> _existingObjects = new();

        public JsonSettingsPreset(BaseSettings settings, string id, string name, GameFile file) : this(settings.Id, id, name, file, settings.CreateNew) { }
        public JsonSettingsPreset(string settingsId, string id, string name, GameFile file, Func<BaseSettings> getNewSettings)
        {
            SettingsId = settingsId;
            Id = id;
            Name = name;
            _file = file;
            _getNewSettings = getNewSettings;

            var logger = GenericServiceProvider.GetService<IBUTRLogger<JsonSettingsPreset>>() ?? new DefaultBUTRLogger<JsonSettingsPreset>();
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters =
                {
                    new BaseSettingsJsonConverter(logger, AddSerializationProperty, ClearSerializationProperties),
                    new DropdownJsonConverter(logger, GetSerializationProperty),
                }
            };
        }

        /// <inheritdoc />
        public BaseSettings LoadPreset()
        {
            var presetBase = _getNewSettings();

            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider) return presetBase;
            if (fileSystemProvider.ReadData(_file) is not { } data)
            {
                SavePreset(presetBase);
                return presetBase;
            }

            var content = Encoding.UTF8.GetString(data);

            lock (_lock)
            {
                var container = new PresetContainer { Id = Id, Name = Name, Settings = presetBase };
                JsonConvert.PopulateObject(content, container, _jsonSerializerSettings);
            }

            return presetBase;
        }

        /// <inheritdoc />
        public bool SavePreset(BaseSettings settings)
        {
            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider) return false;

            var container = new PresetContainer { Id = Id, Name = Name, Settings = settings };
            var content = JsonConvert.SerializeObject(container, _jsonSerializerSettings);

            fileSystemProvider.WriteData(_file, Encoding.UTF8.GetBytes(content));
            return true;
        }

        private object? GetSerializationProperty(string path)
        {
            lock (_lock)
            {
                return _existingObjects.TryGetValue(path, out var value) ? value : null;
            }
        }

        private void AddSerializationProperty(string path, object? value)
        {
            _existingObjects.Add(path, value);
        }

        private void ClearSerializationProperties()
        {
            _existingObjects.Clear();
        }
    }
}