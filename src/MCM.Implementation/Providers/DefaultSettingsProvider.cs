using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Containers.PerCampaign;
using MCM.Abstractions.Settings.Containers.PerSave;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Presets;
using MCM.Abstractions.Settings.Providers;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MCM.Implementation.Settings.Providers
{
    internal sealed class DefaultSettingsProvider : BaseSettingsProvider
    {
        private readonly IBUTRLogger _logger;
        private readonly List<ISettingsContainer> _settingsContainers;

        public override IEnumerable<SettingsDefinition> SettingsDefinitions => _settingsContainers.OfType<ISettingsContainerHasSettingsDefinitions>()
            .SelectMany(sp => sp.SettingsDefinitions);

        public DefaultSettingsProvider(IBUTRLogger<DefaultSettingsProvider> logger)
        {
            _logger = logger;

            var globalSettingsContainers = (GenericServiceProvider.GetService<IEnumerable<IGlobalSettingsContainer>>() ??
                                            Enumerable.Empty<IGlobalSettingsContainer>()).ToList();
            var perSaveSettingsContainers = (GenericServiceProvider.GetService<IEnumerable<IPerSaveSettingsContainer>>() ??
                                             Enumerable.Empty<IPerSaveSettingsContainer>()).ToList();
            var perCampaignSettingsContainers = (GenericServiceProvider.GetService<IEnumerable<IPerCampaignSettingsContainer>>() ??
                                             Enumerable.Empty<IPerCampaignSettingsContainer>()).ToList();

            foreach (var globalSettingsContainer in globalSettingsContainers)
            {
                logger.LogInformation("Found Global container {type}.", globalSettingsContainer.GetType());
            }
            foreach (var perSaveSettingsContainer in perSaveSettingsContainers)
            {
                logger.LogInformation("Found PerSave container {type}.", perSaveSettingsContainer.GetType());
            }
            foreach (var perCampaignSettingsContainer in perCampaignSettingsContainers)
            {
                logger.LogInformation("Found Campaign container {type}.", perCampaignSettingsContainer.GetType());
            }

            _settingsContainers = Enumerable.Empty<ISettingsContainer>()
                .Concat(globalSettingsContainers)
                .Concat(perSaveSettingsContainers)
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

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public override void SaveSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in _settingsContainers)
                settingsContainer.SaveSettings(settings);
            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
        }

        public override void ResetSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in _settingsContainers.OfType<ISettingsContainerCanReset>())
                settingsContainer.ResetSettings(settings);
        }
        
        public override void OverrideSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in _settingsContainers.OfType<ISettingsContainerCanOverride>())
                settingsContainer.OverrideSettings(settings);
        }
        
        public override IEnumerable<ISettingsPreset> GetPresets(string id)
        {
            foreach (var settingsContainer in _settingsContainers.OfType<ISettingsContainerPresets>())
            {
                foreach (var preset in settingsContainer.GetPresets(id))
                {
                    yield return preset;
                }
            }
        }
    }
}