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
    internal sealed class PerCampaignSettingsContainer : BaseSettingsContainer<PerCampaignSettings>, IPerCampaignSettingsContainer
    {
        private readonly IBUTRLogger _logger;
        private readonly IGameEventListener _gameEventListener;

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
        protected override void RegisterSettings(PerCampaignSettings? perCampaignSettings)
        {
            if (perCampaignSettings is null)
                return;

            if (GenericServiceProvider.GameScopeServiceProvider is null)
                return;

            if (GenericServiceProvider.GetService<ICampaignIdProvider>() is not { } campaignIdProvider || campaignIdProvider.GetCurrentCampaignId() is not { } id)
                return;

            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider)
                return;

            LoadedSettings.Add(perCampaignSettings.Id, perCampaignSettings);

            var idDirectory = fileSystemProvider.GetOrCreateDirectory(RootFolder, id);
            var idWithFolderDirectory = fileSystemProvider.GetOrCreateDirectory(idDirectory, perCampaignSettings.FolderName);
            var directory = fileSystemProvider.GetOrCreateDirectory(idWithFolderDirectory, perCampaignSettings.SubFolder);

            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == perCampaignSettings.FormatType));
            settingsFormat?.Load(perCampaignSettings, directory, perCampaignSettings.Id);
            perCampaignSettings.OnPropertyChanged(BaseSettings.LoadingComplete);
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

            var idDirectory = fileSystemProvider.GetOrCreateDirectory(RootFolder, id);
            var idWithFolderDirectory = fileSystemProvider.GetOrCreateDirectory(idDirectory, perCampaignSettings.FolderName);
            var directory = fileSystemProvider.GetOrCreateDirectory(idWithFolderDirectory, perCampaignSettings.SubFolder);

            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == perCampaignSettings.FormatType));
            settingsFormat?.Save(perCampaignSettings, directory, perCampaignSettings.Id);

            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
            return true;
        }

        private void GameStarted()
        {
            LoadedSettings.Clear();
        }

        private void GameLoaded()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            IEnumerable<PerCampaignSettings> GetPerCampaignSettings()
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
                            .Select(t => Activator.CreateInstance(t) as PerCampaignSettings)
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

            foreach (var setting in GetPerCampaignSettings())
                RegisterSettings(setting);
        }

        private void GameEnded()
        {
            LoadedSettings.Clear();
        }
    }
}