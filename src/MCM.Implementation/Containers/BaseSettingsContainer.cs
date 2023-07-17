using BUTR.DependencyInjection;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.GameFeatures;

using System;
using System.Collections.Generic;
using System.Linq;

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
    abstract class BaseSettingsContainer<TSettings> :
        ISettingsContainer,
        ISettingsContainerHasSettingsDefinitions,
        ISettingsContainerCanOverride,
        ISettingsContainerCanReset,
        ISettingsContainerPresets,
        ISettingsContainerHasSettingsPack
        where TSettings : BaseSettings
    {
        protected virtual GameDirectory RootFolder { get; } =
            GenericServiceProvider.GetService<IFileSystemProvider>()?.GetModSettingsDirectory() ?? new GameDirectory(PlatformDirectoryType.User, "Configs\\ModSettings\\");
        protected virtual Dictionary<string, TSettings> LoadedSettings { get; } = new();

        /// <inheritdoc/>
        public virtual IEnumerable<SettingsDefinition> SettingsDefinitions =>
            LoadedSettings.Values.Select(s => new SettingsDefinition(s.Id, s.DisplayName, s.GetSettingPropertyGroups()));

        protected virtual void RegisterSettings(TSettings? settings)
        {
            if (settings is null || LoadedSettings.ContainsKey(settings.Id))
                return;

            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider)
                return;

            LoadedSettings.Add(settings.Id, settings);

            var folderDirectory = fileSystemProvider.GetOrCreateDirectory(RootFolder, settings.FolderName);
            var directory = string.IsNullOrEmpty(settings.SubFolder) ? folderDirectory : fileSystemProvider.GetOrCreateDirectory(folderDirectory, settings.SubFolder);

            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Load(settings, directory, settings.Id);
            settings.OnPropertyChanged(BaseSettings.LoadingComplete);
        }

        /// <inheritdoc/>
        public virtual BaseSettings? GetSettings(string id) => LoadedSettings.TryGetValue(id, out var result) ? result : null;
        /// <inheritdoc/>
        public virtual bool SaveSettings(BaseSettings settings)
        {
            if (settings is not TSettings || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider)
                return false;

            var folderDirectory = fileSystemProvider.GetOrCreateDirectory(RootFolder, settings.FolderName);
            var directory = string.IsNullOrEmpty(settings.SubFolder) ? folderDirectory : fileSystemProvider.GetOrCreateDirectory(folderDirectory, settings.SubFolder);

            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Save(settings, directory, settings.Id);

            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
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

            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider)
                yield break;

            var presetsDirectory = fileSystemProvider.GetOrCreateDirectory(fileSystemProvider.GetModSettingsDirectory(), "Presets");
            var settingsDirectory = fileSystemProvider.GetOrCreateDirectory(presetsDirectory, settingsId);

            foreach (var filePath in fileSystemProvider.GetFiles(settingsDirectory, "*.json"))
            {
                if (JsonSettingsPreset.FromFile(settings, filePath) is { } preset)
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

        /// <inheritdoc />
        public IEnumerable<SettingSnapshot> SaveAvailableSnapshots()
        {
            var jsonFormat = GenericServiceProvider.GetService<JsonSettingsFormat>();
            if (jsonFormat is null) yield break;

            foreach (var setting in LoadedSettings.Values)
            {
                var json = jsonFormat.SaveJson(setting);
                var folderName = !string.IsNullOrEmpty(setting.FolderName) ? $"{setting.FolderName}/" : string.Empty;
                var subFolderName = !string.IsNullOrEmpty(setting.SubFolder) ? $"{setting.SubFolder}/" : string.Empty;
                var path = $"{folderName}{subFolderName}{setting.Id}.json";
                yield return new(path, json);
            }
        }

        /// <inheritdoc />
        public IEnumerable<BaseSettings> LoadAvailableSnapshots(IEnumerable<SettingSnapshot> snapshots)
        {
            var jsonFormat = GenericServiceProvider.GetService<JsonSettingsFormat>();
            if (jsonFormat is null) yield break;

            var snapshotDict = snapshots.ToDictionary(x => x.Path, x => x.JsonContent);
            foreach (var setting in LoadedSettings.Values)
            {
                var folderName = !string.IsNullOrEmpty(setting.FolderName) ? $"{setting.FolderName}/" : string.Empty;
                var subFolderName = !string.IsNullOrEmpty(setting.SubFolder) ? $"{setting.SubFolder}/" : string.Empty;
                var path = $"{folderName}{subFolderName}{setting.Id}.json";
                if (snapshotDict.TryGetValue(path, out var json))
                {
                    yield return jsonFormat.LoadFromJson(setting.CreateNew(), json);
                }
            }
        }
    }
}