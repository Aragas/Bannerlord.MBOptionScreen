using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.PerCampaign;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.PerCampaign;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.PerCampaign
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class PerCampaignSettingsContainer : BaseSettingsContainer<PerCampaignSettings>, IPerCampaignSettingsContainer, ISettingsContainerHasUnavailable
    {
        private readonly IBUTRLogger _logger;
        private readonly IGameEventListener _gameEventListener;

        private bool _hasGameStarted;

        /// <inheritdoc/>
        protected override GameDirectory RootFolder { get; }

        public PerCampaignSettingsContainer(IBUTRLogger<PerCampaignSettingsContainer> logger, IGameEventListener gameEventListener)
        {
            _logger = logger;
            _gameEventListener = gameEventListener;
            _gameEventListener.GameStarted += GameStarted;
            _gameEventListener.GameLoaded += GameLoaded;
            _gameEventListener.GameEnded += GameEnded;

            var fileSystemProvider = GenericServiceProvider.GetService<IFileSystemProvider>();
            RootFolder = fileSystemProvider!.GetDirectory(base.RootFolder, "PerCampaign")!;
        }

        /// <inheritdoc/>
        protected override void RegisterSettings(PerCampaignSettings? settings)
        {
            if (settings is null)
                return;

            if (GenericServiceProvider.GameScopeServiceProvider is null)
                return;

            if (GenericServiceProvider.GetService<ICampaignIdProvider>() is not { } campaignIdProvider || campaignIdProvider.GetCurrentCampaignId() is not { } id)
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
        public override bool SaveSettings(BaseSettings settings)
        {
            if (settings is not PerCampaignSettings perCampaignSettings)
                return false;

            if (GenericServiceProvider.GameScopeServiceProvider is null)
                return false;

            if (GenericServiceProvider.GetService<ICampaignIdProvider>() is not { } campaignIdProvider || campaignIdProvider.GetCurrentCampaignId() is not { } id)
                return false;

            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider)
                return false;

            var folderDirectory = fileSystemProvider.GetOrCreateDirectory(RootFolder, settings.FolderName);
            var directory = string.IsNullOrEmpty(settings.SubFolder) ? folderDirectory : fileSystemProvider.GetOrCreateDirectory(folderDirectory, settings.SubFolder);

            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == perCampaignSettings.FormatType));
            settingsFormat?.Save(perCampaignSettings, directory, perCampaignSettings.Id);

            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
            return true;
        }

        private void GameStarted()
        {
            _hasGameStarted = true;
            LoadedSettings.Clear();
        }

        private void GameLoaded()
        {
            LoadSettings();
        }

        private IEnumerable<PerCampaignSettings> GetPerCampaignSettings()
        {
            foreach (var assembly in AccessTools2.AllAssemblies().Where(a => !a.IsDynamic))
            {
                IEnumerable<PerCampaignSettings> settings;
                try
                {
                    settings = AccessTools2.GetTypesFromAssemblyIfValid(assembly)
                        .Where(t => t.IsClass && !t.IsAbstract)
                        .Where(t => t.GetConstructor(Type.EmptyTypes) is not null)
                        .Where(t => typeof(PerCampaignSettings).IsAssignableFrom(t))
                        .Where(t => !typeof(EmptyPerCampaignSettings).IsAssignableFrom(t))
                        .Where(t => !typeof(IWrapper).IsAssignableFrom(t))
                        .Select(t =>
                        {
                            try
                            {
                                return Activator.CreateInstance(t) as PerCampaignSettings;
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    _logger.LogError(e, $"Failed to initialize type {t}");
                                }
                                catch (Exception)
                                {
                                    _logger.LogError(e, "Failed to initialize and log type!");
                                }
                                return null;
                            }
                        })
                        .OfType<PerCampaignSettings>()
                        .ToList();
                }
                catch (TypeLoadException ex)
                {
                    settings = Array.Empty<PerCampaignSettings>();
                    _logger.LogError(ex, $"Error while handling assembly {assembly}!");
                }

                foreach (var setting in settings)
                {
                    yield return setting;
                }
            }
        }

        private void LoadSettings()
        {
            foreach (var setting in GetPerCampaignSettings())
                RegisterSettings(setting);
        }

        public IEnumerable<UnavailableSetting> GetUnavailableSettings() => !_hasGameStarted
            ? GetPerCampaignSettings().Select(setting => new UnavailableSetting(setting.Id, setting.DisplayName, UnavailableSettingType.PerCampaign))
            : Enumerable.Empty<UnavailableSetting>();

        private void GameEnded()
        {
            _hasGameStarted = false;
            LoadedSettings.Clear();
        }
    }
}