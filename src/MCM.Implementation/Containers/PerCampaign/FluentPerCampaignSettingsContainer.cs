using BUTR.DependencyInjection;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.PerCampaign;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.PerCampaign;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.PerCampaign
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class FluentPerCampaignSettingsContainer : BaseSettingsContainer<FluentPerCampaignSettings>, IFluentPerCampaignSettingsContainer
    {
        /// <inheritdoc/>
        public event Action? InstanceCacheInvalidated;
        
        private readonly IGameEventListener _gameEventListener;

        public FluentPerCampaignSettingsContainer(IGameEventListener gameEventListener)
        {
            _gameEventListener = gameEventListener;
            _gameEventListener.GameStarted += GameStarted;
            _gameEventListener.GameEnded += GameEnded;
        }

        /// <inheritdoc/>
        protected override void RegisterSettings(FluentPerCampaignSettings? settings)
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

        public void Register(FluentPerCampaignSettings settings)
        {
            RegisterSettings(settings);
        }
        public void Unregister(FluentPerCampaignSettings settings)
        {
            if (LoadedSettings.ContainsKey(settings.Id))
                LoadedSettings.Remove(settings.Id);
        }

        private void GameStarted() => LoadedSettings.Clear();
        private void GameEnded() => LoadedSettings.Clear();

        public IEnumerable<UnavailableSetting> GetUnavailableSettings() => Enumerable.Empty<UnavailableSetting>();
    }
}