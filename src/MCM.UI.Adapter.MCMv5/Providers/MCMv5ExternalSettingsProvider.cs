using BUTR.DependencyInjection.Logger;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Base;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MCM.UI.Adapter.MCMv5.Providers
{
    internal sealed class MCMv5ExternalSettingsProvider : IExternalSettingsProvider, IExternalSettingsProviderHasInitialize
    {
        private readonly IBUTRLogger _logger;
        private readonly List<MCMv5SettingsProviderWrapper> _settingsProviderWrappers = new();

        public IEnumerable<SettingsDefinition> SettingsDefinitions => _settingsProviderWrappers
            .SelectMany(x => x.SettingsDefinitions)
            .Where(x => x.SettingsId != "MCM_v5");

        public MCMv5ExternalSettingsProvider(IBUTRLogger<MCMv5ExternalSettingsProvider> logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            var foreignBaseSettingsProviders = AccessTools2.AllTypes()
                .Where(x => x.Assembly != typeof(BaseSettings).Assembly)
                .Where(x => !x.IsSubclassOf(typeof(BaseSettingsProvider)) && ReflectionUtils.ImplementsOrImplementsEquivalent(x, "MCM.Abstractions.BaseSettingsProvider"))
                .Select(x => AccessTools2.DeclaredProperty(x, "Instance") is { GetMethod.IsStatic: true } prop ? prop.GetValue(null) : null)
                .OfType<object>();

            _settingsProviderWrappers.AddRange(foreignBaseSettingsProviders.Select(x => new MCMv5SettingsProviderWrapper(x)));
        }

        public BaseSettings? GetSettings(string id)
        {
            foreach (var settingsProvider in _settingsProviderWrappers)
            {
                if (settingsProvider.GetSettings(id) is { } settings)
                    return settings;
            }
            _logger.LogWarning($"GetSettings {id} returned null");
            return null;
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public void SaveSettings(BaseSettings settings)
        {
            foreach (var settingsProvider in _settingsProviderWrappers)
                settingsProvider.SaveSettings(settings);
        }

        public void ResetSettings(BaseSettings settings)
        {
            foreach (var settingsProvider in _settingsProviderWrappers)
                settingsProvider.ResetSettings(settings);
        }

        public void OverrideSettings(BaseSettings settings)
        {
            foreach (var settingsProvider in _settingsProviderWrappers)
                settingsProvider.OverrideSettings(settings);
        }

        public IEnumerable<ISettingsPreset> GetPresets(string id)
        {
            foreach (var settingsProvider in _settingsProviderWrappers)
            {
                foreach (var preset in settingsProvider.GetPresets(id))
                {
                    yield return preset;
                }
            }
        }
    }
}