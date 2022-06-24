using BUTR.DependencyInjection;

using MCM.Abstractions;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.PerCampaign;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Formats;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerCampaign
{
    internal sealed class FluentPerCampaignSettingsContainer : BaseSettingsContainer<FluentPerCampaignSettings>, IMCMFluentPerCampaignSettingsContainer
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

            var directoryPath = Path.Combine(RootFolder, Campaign.Current.UniqueGameId, perCampaignSettings.FolderName, perCampaignSettings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == perCampaignSettings.FormatType));
            settingsFormat?.Load(perCampaignSettings, directoryPath, perCampaignSettings.Id);
        }

        /// <inheritdoc/>
        public override bool SaveSettings(BaseSettings settings)
        {
            if (settings is not PerCampaignSettings campaignSettings)
                return false;

            var directoryPath = Path.Combine(RootFolder, Campaign.Current.UniqueGameId, campaignSettings.FolderName, campaignSettings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == campaignSettings.FormatType));
            settingsFormat?.Save(campaignSettings, directoryPath, campaignSettings.Id);

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

        private void OnGameStarted(Game game) => LoadedSettings.Clear();
        private void OnGameEnded(Game game) => LoadedSettings.Clear();
    }
}