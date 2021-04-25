using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Containers.PerSave;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Providers;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Providers
{
    internal sealed class DefaultSettingsProvider : BaseSettingsProvider
    {
        private readonly IBUTRLogger _logger;
        private readonly List<ISettingsContainer> _settingsContainers;

        public override IEnumerable<SettingsDefinition> CreateModSettingsDefinitions => _settingsContainers
            .SelectMany(sp => sp.CreateModSettingsDefinitions);

        public DefaultSettingsProvider(IBUTRLogger<DefaultSettingsProvider> logger)
        {
            _logger = logger;

             var globalSettingsContainers = (GenericServiceProvider.GetService<IEnumerable<IGlobalSettingsContainer>>() ??
                                             Enumerable.Empty<IGlobalSettingsContainer>()).ToList();
            var perSaveSettingsContainers = (GenericServiceProvider.GetService<IEnumerable<IPerSaveSettingsContainer>>() ??
                                             Enumerable.Empty<IPerSaveSettingsContainer>()).ToList();

            foreach (var globalSettingsContainer in globalSettingsContainers)
            {
                logger.LogInformation("Found Global container {type}.", globalSettingsContainer.GetType());
            }
            foreach (var perSaveSettingsContainer in perSaveSettingsContainers)
            {
                logger.LogInformation("Found PerSave container {type}.", perSaveSettingsContainer.GetType());
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
            foreach (var perSaveContainer in _settingsContainers.OfType<IPerSaveSettingsContainer>())
                perSaveContainer.OnGameStarted(game);
        }
        public override void OnGameEnded(Game game)
        {
            foreach (var perSaveContainer in _settingsContainers.OfType<IPerSaveSettingsContainer>())
                perSaveContainer.OnGameEnded(game);
        }
    }
}