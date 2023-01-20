using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;

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
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }
        private sealed class PresetContainer : PresetContainerDefinition
        {
            public BaseSettings? Settings { get; set; }
        }

        public static JsonSettingsPreset? FromFile(BaseSettings settings, string filePath) => FromFile(settings.Id, filePath, settings.CreateNew);
        public static JsonSettingsPreset? FromFile(string settingsId, string filePath, Func<BaseSettings> getNewSettings)
        {
            var file = new FileInfo(filePath);
            if (!file.Exists) return null;
            var reader = file.OpenText();
            var content = reader.ReadToEnd();
            reader.Dispose();

            var container = JsonConvert.DeserializeObject<PresetContainerDefinition?>(content);
            if (container is null) return null;

            return new JsonSettingsPreset(settingsId, container.Id, container.Name, filePath, getNewSettings);
        }

        /// <inheritdoc />
        public string SettingsId { get; }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        private readonly string _filePath;
        private readonly Func<BaseSettings> _getNewSettings;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly object _lock = new();
        private readonly Dictionary<string, object?> _existingObjects = new();

        public JsonSettingsPreset(BaseSettings settings, string id, string name, string filePath) : this(settings.Id, id, name, filePath, settings.CreateNew) { }
        public JsonSettingsPreset(string settingsId, string id, string name, string filePath, Func<BaseSettings> getNewSettings)
        {
            SettingsId = settingsId;
            Id = id;
            Name = name;
            _filePath = filePath;
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

            var file = new FileInfo(_filePath);
            if (file.Exists)
            {
                var reader = file.OpenText();
                var content = reader.ReadToEnd();
                reader.Dispose();

                lock (_lock)
                {
                    var container = new PresetContainer { Id = Id, Name = Name, Settings = presetBase };
                    JsonConvert.PopulateObject(content, container, _jsonSerializerSettings);
                }

                return presetBase;
            }
            else
            {
                SavePreset(presetBase);

                return presetBase;
            }
        }

        /// <inheritdoc />
        public bool SavePreset(BaseSettings settings)
        {
            var container = new PresetContainer { Id = Id, Name = Name, Settings = settings };
            var content = JsonConvert.SerializeObject(container, _jsonSerializerSettings);

            var file = new FileInfo(_filePath);
            file.Directory?.Create();
            var writer = file.CreateText();
            writer.Write(content);
            writer.Dispose();

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