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
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class DefaultSettingsProvider : BaseSettingsProvider
    {
        private readonly IBUTRLogger _logger;
        private readonly List<ISettingsContainer> _settingsContainers;
        private readonly List<IExternalSettingsProvider> _externalSettingsProviders;

        public override IEnumerable<SettingsDefinition> SettingsDefinitions => _settingsContainers.OfType<ISettingsContainerHasSettingsDefinitions>()
            .SelectMany(sp => sp.SettingsDefinitions).Concat(_externalSettingsProviders.SelectMany(x => x.SettingsDefinitions));

        public DefaultSettingsProvider(IBUTRLogger<DefaultSettingsProvider> logger)
        {
            _logger = logger;

            var globalSettingsContainers = (GenericServiceProvider.GetService<IEnumerable<IGlobalSettingsContainer>>() ??
                                            Enumerable.Empty<IGlobalSettingsContainer>()).ToList();
            var perSaveSettingsContainers = (GenericServiceProvider.GetService<IEnumerable<IPerSaveSettingsContainer>>() ??
                                             Enumerable.Empty<IPerSaveSettingsContainer>()).ToList();
            var perCampaignSettingsContainers = (GenericServiceProvider.GetService<IEnumerable<IPerCampaignSettingsContainer>>() ??
                                             Enumerable.Empty<IPerCampaignSettingsContainer>()).ToList();
            var externalSettingsProviders = (GenericServiceProvider.GetService<IEnumerable<IExternalSettingsProvider>>() ??
                                             Enumerable.Empty<IExternalSettingsProvider>()).ToList();

            foreach (var globalSettingsContainer in globalSettingsContainers)
            {
                logger.LogInformation($"Found Global container {globalSettingsContainer.GetType()} ({globalSettingsContainer.SettingsDefinitions.Count()})");
            }
            foreach (var perSaveSettingsContainer in perSaveSettingsContainers)
            {
                logger.LogInformation($"Found PerSave container {perSaveSettingsContainer.GetType()} ({perSaveSettingsContainer.SettingsDefinitions.Count()})");
            }
            foreach (var perCampaignSettingsContainer in perCampaignSettingsContainers)
            {
                logger.LogInformation($"Found Campaign container {perCampaignSettingsContainer.GetType()} ({perCampaignSettingsContainer.SettingsDefinitions.Count()})");
            }
            foreach (var externalSettingsProvider in externalSettingsProviders)
            {
                logger.LogInformation($"Found external provider {externalSettingsProvider.GetType()} ({externalSettingsProvider.SettingsDefinitions.Count()})");
            }

            _settingsContainers = Enumerable.Empty<ISettingsContainer>()
                .Concat(globalSettingsContainers)
                .Concat(perSaveSettingsContainers)
                .Concat(perCampaignSettingsContainers)
                .ToList();

            _externalSettingsProviders = externalSettingsProviders.ToList();
        }

        public override BaseSettings? GetSettings(string id)
        {
            foreach (var settingsContainer in _settingsContainers)
            {
                if (settingsContainer.GetSettings(id) is { } settings)
                {
                    return settings;
                }
            }
            foreach (var settingsProvider in _externalSettingsProviders)
            {
                if (settingsProvider.GetSettings(id) is { } settings)
                {
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
            foreach (var settingsProvider in _externalSettingsProviders)
                settingsProvider.SaveSettings(settings);
        }

        public override void ResetSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in _settingsContainers.OfType<ISettingsContainerCanReset>())
                settingsContainer.ResetSettings(settings);
            foreach (var settingsProvider in _externalSettingsProviders)
                settingsProvider.ResetSettings(settings);
        }

        public override void OverrideSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in _settingsContainers.OfType<ISettingsContainerCanOverride>())
                settingsContainer.OverrideSettings(settings);
            foreach (var settingsProvider in _externalSettingsProviders)
                settingsProvider.OverrideSettings(settings);
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
            foreach (var settingsProvider in _externalSettingsProviders)
            {
                foreach (var preset in settingsProvider.GetPresets(id))
                {
                    yield return preset;
                }
            }
        }

        public override IEnumerable<UnavailableSetting> GetUnavailableSettings()
        {
            foreach (var settingsContainer in _settingsContainers.OfType<ISettingsContainerHasUnavailable>())
            {
                foreach (var unavailableSetting in settingsContainer.GetUnavailableSettings())
                    yield return unavailableSetting;
            }
        }

        /// <inheritdoc />
        public override IEnumerable<SettingSnapshot> SaveAvailableSnapshots()
        {
            foreach (var settingsContainer in _settingsContainers.OfType<ISettingsContainerHasSettingsPack>())
            {
                foreach (var snapshot in settingsContainer.SaveAvailableSnapshots())
                    yield return snapshot;
            }
        }

        /// <inheritdoc />
        public override IEnumerable<BaseSettings> LoadAvailableSnapshots(IEnumerable<SettingSnapshot> snapshots)
        {
            foreach (var settingsContainer in _settingsContainers.OfType<ISettingsContainerHasSettingsPack>())
            {
                foreach (var settings in settingsContainer.LoadAvailableSnapshots(snapshots))
                {
                    yield return settings;
                }
            }
        }
    }
}