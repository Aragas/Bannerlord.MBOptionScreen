using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Global;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.Global
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class ExternalGlobalSettingsContainer : IFluentGlobalSettingsContainer
    {
        private Dictionary<string, ExternalGlobalSettings> LoadedSettings { get; } = new();

        /// <inheritdoc/>
        public IEnumerable<SettingsDefinition> SettingsDefinitions =>
            LoadedSettings.Values.Select(s => new SettingsDefinition(s.Id, s.DisplayName, s.GetSettingPropertyGroups()));

        public ExternalGlobalSettingsContainer(IBUTRLogger<ExternalGlobalSettingsContainer> logger)
        {

        }

        /// <inheritdoc/>
        public BaseSettings? GetSettings(string id) => LoadedSettings.TryGetValue(id, out var result) ? result : null;

        /// <inheritdoc/>
        public bool SaveSettings(BaseSettings settings)
        {
            if (settings is not ExternalGlobalSettings || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            /*
            var directoryPath = Path.Combine(RootFolder, settings.FolderName, settings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? [];
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Save(settings, directoryPath, settings.Id);
            */

            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
            return true;
        }

        /// <inheritdoc/>
        public bool OverrideSettings(BaseSettings settings)
        {
            if (settings is not ExternalGlobalSettings || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            SettingsUtils.OverrideSettings(LoadedSettings[settings.Id], settings);
            return true;
        }
        /// <inheritdoc/>
        public bool ResetSettings(BaseSettings settings)
        {
            if (settings is not ExternalGlobalSettings || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            SettingsUtils.ResetSettings(LoadedSettings[settings.Id]);
            return true;
        }

        /// <inheritdoc />
        public IEnumerable<ISettingsPreset> GetPresets(string settingsId)
        {
            yield break;
            /*
            if (!LoadedSettings.TryGetValue(settingsId, out var settings))
                yield break;

            var directoryPath = new DirectoryInfo(Path.Combine(RootFolder, settings.FolderName, settings.SubFolder, "Presets"));
            directoryPath.Create();
            foreach (var filePath in directoryPath.GetFiles("*.json", SearchOption.TopDirectoryOnly))
            {
                if (JsonSettingsPreset.FromFile(settings, filePath.FullName) is { } preset)
                    yield return preset;
            }
            */
        }

        /// <inheritdoc />
        public void Register(FluentGlobalSettings settings)
        {
            if (settings is not ExternalGlobalSettings externalGlobalSettings)
                return;

            LoadedSettings.Add(externalGlobalSettings.Id, externalGlobalSettings);
            settings.OnPropertyChanged(BaseSettings.LoadingComplete);
        }

        /// <inheritdoc />
        public void Unregister(FluentGlobalSettings settings)
        {
            if (settings is not ExternalGlobalSettings externalGlobalSettings)
                return;

            if (LoadedSettings.ContainsKey(externalGlobalSettings.Id))
                LoadedSettings.Remove(externalGlobalSettings.Id);
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