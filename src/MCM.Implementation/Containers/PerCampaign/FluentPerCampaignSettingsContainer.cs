using BUTR.DependencyInjection;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.PerCampaign;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.PerCampaign;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MCM.Implementation.PerCampaign
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class FluentPerCampaignSettingsContainer : BaseSettingsContainer<FluentPerCampaignSettings>, IFluentPerCampaignSettingsContainer
    {
        private readonly IGameEventListener _gameEventListener;

        public FluentPerCampaignSettingsContainer(IGameEventListener gameEventListener)
        {
            _gameEventListener = gameEventListener;
            _gameEventListener.OnGameStarted += OnGameStarted;
            _gameEventListener.OnGameEnded += OnGameEnded;
        }

        /// <inheritdoc/>
        protected override void RegisterSettings(FluentPerCampaignSettings? perCampaignSettings)
        {
            if (perCampaignSettings is null)
                return;

            if (GenericServiceProvider.GameScopeServiceProvider is null)
                return;

            if (GenericServiceProvider.GetService<ICampaignIdProvider>() is not { } campaignIdProvider || campaignIdProvider.GetCurrentCampaignId() is not { } id)
                return;

            LoadedSettings.Add(perCampaignSettings.Id, perCampaignSettings);

            var directoryPath = Path.Combine(RootFolder, id, perCampaignSettings.FolderName, perCampaignSettings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == perCampaignSettings.FormatType));
            settingsFormat?.Load(perCampaignSettings, directoryPath, perCampaignSettings.Id);
                perCampaignSettings.OnPropertyChanged(BaseSettings.LoadingComplete);
        }

        /// <inheritdoc/>
        public override bool SaveSettings(BaseSettings settings)
        {
            if (settings is not PerCampaignSettings campaignSettings)
                return false;

            if (GenericServiceProvider.GameScopeServiceProvider is null)
                return false;

            if (GenericServiceProvider.GetService<ICampaignIdProvider>() is not { } campaignIdProvider || campaignIdProvider.GetCurrentCampaignId() is not { } id)
                return false;

            var directoryPath = Path.Combine(RootFolder, id, campaignSettings.FolderName, campaignSettings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == campaignSettings.FormatType));
            settingsFormat?.Save(campaignSettings, directoryPath, campaignSettings.Id);

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

        private void OnGameStarted() => LoadedSettings.Clear();
        private void OnGameEnded() => LoadedSettings.Clear();
    }
}