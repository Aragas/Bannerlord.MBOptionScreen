using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Containers.PerCampaign;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Providers
{
    internal sealed class DefaultSettingsProvider : BaseSettingsProvider
    {
        private readonly ILogger _logger;
        private readonly List<ISettingsContainer> _settingsContainers;

        public override IEnumerable<SettingsDefinition> CreateModSettingsDefinitions => _settingsContainers
            .SelectMany(sp => sp.CreateModSettingsDefinitions);

        public DefaultSettingsProvider(ILogger<DefaultSettingsProvider> logger)
        {
            _logger = logger;

             var globalSettingsContainers = (ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<IGlobalSettingsContainer>>() ??
                                           Enumerable.Empty<IGlobalSettingsContainer>()).ToList();
            var perCampaignSettingsContainers = (ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<IPerCampaignSettingsContainer>>() ??
                                                Enumerable.Empty<IPerCampaignSettingsContainer>()).ToList();

            foreach (var globalSettingsContainer in globalSettingsContainers)
            {
                logger.LogInformation("Found Global container {type}.", globalSettingsContainer.GetType());
            }
            foreach (var perCampaignSettingsContainer in perCampaignSettingsContainers)
            {
                logger.LogInformation("Found PerCampaign container {type}.", perCampaignSettingsContainer.GetType());
            }

            _settingsContainers = Enumerable.Empty<ISettingsContainer>()
                .Concat(globalSettingsContainers)
                .Concat(perCampaignSettingsContainers)
                .ToList();
        }

        public override BaseSettings? GetSettings(string id)
        {
            foreach (var settingsContainer in _settingsContainers)
            {
                if (settingsContainer.GetSettings(id) is { } settings)
                {
                    _logger.LogTrace("GetSettings {id} returned {type}", id, settings.GetType());
                    return settings;
                }
            }
            _logger.LogWarning("GetSettings {id} returned null", id);
            return null;
        }

        public override void SaveSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in _settingsContainers)
                settingsContainer.SaveSettings(settings);
            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
        }

        public override void ResetSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in _settingsContainers)
                settingsContainer.ResetSettings(settings);
        }
        public override void OverrideSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in _settingsContainers)
                settingsContainer.OverrideSettings(settings);
        }

        public override void OnGameStarted(Game game)
        {
            foreach (var perCampaignContainer in _settingsContainers.OfType<IPerCampaignSettingsContainer>())
                perCampaignContainer.OnGameStarted(game);
        }
        public override void OnGameEnded(Game game)
        {
            foreach (var perCampaignContainer in _settingsContainers.OfType<IPerCampaignSettingsContainer>())
                perCampaignContainer.OnGameEnded(game);
        }
    }
}