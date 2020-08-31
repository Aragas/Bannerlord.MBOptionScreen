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
        private ILogger Logger { get; }
        private List<ISettingsContainer> SettingsContainers { get; }

        public override IEnumerable<SettingsDefinition> CreateModSettingsDefinitions => SettingsContainers
            .SelectMany(sp => sp.CreateModSettingsDefinitions);

        public DefaultSettingsProvider(ILogger<DefaultSettingsProvider> logger)
        {
            Logger = logger;

            var globalSettingsContainers = (ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<IGlobalSettingsContainer>>() ??
                                           Enumerable.Empty<IGlobalSettingsContainer>()).ToList();
            var perCampaignSettingsContainers = (ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<IPerCampaignSettingsContainer>>() ??
                                                Enumerable.Empty<IPerCampaignSettingsContainer>()).ToList();

            foreach (var globalSettingsContainer in globalSettingsContainers)
            {
                logger.LogInformation("Found Global container {type}. Registered {count}", globalSettingsContainer.GetType(), globalSettingsContainer.CreateModSettingsDefinitions.Count);
            }
            foreach (var perCampaignSettingsContainer in perCampaignSettingsContainers)
            {
                logger.LogInformation("Found PerCampaign container {type}.", perCampaignSettingsContainer.GetType());
            }

            SettingsContainers = Enumerable.Empty<ISettingsContainer>()
                .Concat(globalSettingsContainers)
                .Concat(perCampaignSettingsContainers)
                .ToList();
        }

        public override BaseSettings? GetSettings(string id)
        {
            foreach (var settingsContainer in SettingsContainers)
            {
                if (settingsContainer.GetSettings(id) is { } settings)
                {
                    Logger.LogTrace("GetSettings {id} returned {type}", id, settings.GetType());
                    return settings;
                }
            }
            Logger.LogWarning("GetSettings {id} returned null", id);
            return null;
        }

        public override void SaveSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in SettingsContainers)
                settingsContainer.SaveSettings(settings);
            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
        }

        public override void ResetSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in SettingsContainers)
                settingsContainer.ResetSettings(settings);
        }
        public override void OverrideSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in SettingsContainers)
                settingsContainer.OverrideSettings(settings);
        }

        public override void OnGameStarted(Game game)
        {
            foreach (var settingsContainer in SettingsContainers)
            {
                if (settingsContainer is IPerCampaignSettingsContainer perCampaignContainer)
                {
                    perCampaignContainer.OnGameStarted(game);
                }
            }
        }
        public override void OnGameEnded(Game game)
        {
            foreach (var settingsContainer in SettingsContainers)
            {
                if (settingsContainer is IPerCampaignSettingsContainer perCampaignContainer)
                {
                    perCampaignContainer.OnGameEnded(game);
                }
            }
        }
    }
}