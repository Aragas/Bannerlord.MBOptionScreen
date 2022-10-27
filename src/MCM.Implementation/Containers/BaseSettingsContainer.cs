using BUTR.DependencyInjection;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.GameFeatures;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Path = System.IO.Path;

namespace MCM.Implementation
{
    public abstract class BaseSettingsContainer<TSettings> :
        ISettingsContainer,
        ISettingsContainerHasSettingsDefinitions,
        ISettingsContainerCanOverride,
        ISettingsContainerCanReset,
        ISettingsContainerPresets
        where TSettings : BaseSettings
    {
        protected virtual string RootFolder { get; } = Path.Combine(GenericServiceProvider.GetService<IPathProvider>()?.GetDocumentsPath(), "ModSettings");
        protected virtual Dictionary<string, TSettings> LoadedSettings { get; } = new();

        /// <inheritdoc/>
        public virtual IEnumerable<SettingsDefinition> SettingsDefinitions => LoadedSettings.Keys.Select(id => new SettingsDefinition(id));

        protected virtual void RegisterSettings(TSettings? settings)
        {
            if (settings is null || LoadedSettings.ContainsKey(settings.Id))
                return;

            LoadedSettings.Add(settings.Id, settings);

            var directoryPath = Path.Combine(RootFolder, settings.FolderName, settings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Load(settings, directoryPath, settings.Id);
        }

        /// <inheritdoc/>
        public virtual BaseSettings? GetSettings(string id) => LoadedSettings.TryGetValue(id, out var result) ? result : null;
        /// <inheritdoc/>
        public virtual bool SaveSettings(BaseSettings settings)
        {
            if (settings is not TSettings || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            var directoryPath = Path.Combine(RootFolder, settings.FolderName, settings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Save(settings, directoryPath, settings.Id);

            return true;
        }

        /// <inheritdoc/>
        public virtual bool OverrideSettings(BaseSettings settings)
        {
            if (settings is not TSettings || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            SettingsUtils.OverrideSettings(LoadedSettings[settings.Id], settings);
            return true;
        }
        /// <inheritdoc/>
        public virtual bool ResetSettings(BaseSettings settings)
        {
            if (settings is not TSettings || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            SettingsUtils.ResetSettings(LoadedSettings[settings.Id]);
            return true;
        }

        /// <inheritdoc />
        public IEnumerable<ISettingsPreset> GetPresets(string settingsId)
        {
            if (!LoadedSettings.TryGetValue(settingsId, out var settings))
                yield break;

            var directoryPath = new DirectoryInfo(Path.Combine(RootFolder, settings.FolderName, settings.SubFolder, "Presets"));
            directoryPath.Create();
            foreach (var filePath in directoryPath.GetFiles("*.json", SearchOption.TopDirectoryOnly))
            {
                if (JsonSettingsPreset.FromFile(settings, filePath.FullName) is { } preset)
                    yield return preset;
            }
        }

        /// <inheritdoc />
        public bool SavePresets(ISettingsPreset preset)
        {
            if (!LoadedSettings.TryGetValue(preset.SettingsId, out var settings))
                return false;

            return preset.SavePreset(settings);
        }
    }
}