using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Global;
using MCM.Abstractions.PerCampaign;
using MCM.Abstractions.PerSave;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MCM.Implementation
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
                logger.LogInformation($"Found Global container {globalSettingsContainer.GetType()}.");
            }
            foreach (var perSaveSettingsContainer in perSaveSettingsContainers)
            {
                logger.LogInformation($"Found PerSave container {perSaveSettingsContainer.GetType()}.");
            }
            foreach (var perCampaignSettingsContainer in perCampaignSettingsContainers)
            {
                logger.LogInformation($"Found Campaign container {perCampaignSettingsContainer.GetType()}.");
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
                    _logger.LogTrace($"GetSettings {id} returned {settings.GetType()}");
                    return settings;
                }
            }
            _logger.LogWarning($"GetSettings {id} returned null");
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