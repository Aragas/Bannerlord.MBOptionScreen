using BUTR.DependencyInjection;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.GameFeatures;

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
        ISettingsContainerPresets
        where TSettings : BaseSettings
    {
        protected virtual GameDirectory RootFolder { get; } =
            GenericServiceProvider.GetService<IFileSystemProvider>()?.GetModSettingsDirectory() ?? new GameDirectory(PlatformDirectoryType.User, "Mod Settings");
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

            var idDirectory = fileSystemProvider.GetOrCreateDirectory(RootFolder, settings.Id);
            var idWithFolderDirectory = fileSystemProvider.GetOrCreateDirectory(idDirectory, settings.FolderName);
            var directory = fileSystemProvider.GetOrCreateDirectory(idWithFolderDirectory, settings.SubFolder);

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

            var idDirectory = fileSystemProvider.GetOrCreateDirectory(RootFolder, settings.Id);
            var idWithFolderDirectory = fileSystemProvider.GetOrCreateDirectory(idDirectory, settings.FolderName);
            var directory = fileSystemProvider.GetOrCreateDirectory(idWithFolderDirectory, settings.SubFolder);

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

            var idDirectory = fileSystemProvider.GetOrCreateDirectory(RootFolder, settings.Id);
            var idWithFolderDirectory = fileSystemProvider.GetOrCreateDirectory(idDirectory, settings.FolderName);
            var idWithFolderWithSubFolderDirectory = fileSystemProvider.GetOrCreateDirectory(idWithFolderDirectory, settings.SubFolder);
            var directory = fileSystemProvider.GetOrCreateDirectory(idWithFolderWithSubFolderDirectory, "Presets");

            foreach (var filePath in fileSystemProvider.GetFiles(directory, "*.json"))
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
    }
}