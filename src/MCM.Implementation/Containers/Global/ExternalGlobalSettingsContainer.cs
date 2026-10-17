using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Global;

namespace MCM.Implementation.Global
{
    /// <summary>
    /// Holds <see cref="ExternalGlobalSettings"/>, the settings declared in XML. Their values already come from the
    /// declaring file, and saving writes them back into it, so no <c>ModSettings</c> file is involved.
    /// Presets and snapshots work as for every other global settings.
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class ExternalGlobalSettingsContainer : BaseSettingsContainer<ExternalGlobalSettings>, IFluentGlobalSettingsContainer
    {
        private readonly IBUTRLogger _logger;

        public ExternalGlobalSettingsContainer(IBUTRLogger<ExternalGlobalSettingsContainer> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        protected override void RegisterSettings(ExternalGlobalSettings? settings)
        {
            if (settings is null || LoadedSettings.ContainsKey(settings.Id))
                return;

            LoadedSettings.Add(settings.Id, settings);
            settings.OnPropertyChanged(BaseSettings.LoadingComplete);
        }

        /// <inheritdoc/>
        public override bool SaveSettings(BaseSettings settings)
        {
            if (settings is not ExternalGlobalSettings externalGlobalSettings || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            if (!externalGlobalSettings.SaveToFile())
                _logger.LogWarning($"Settings '{settings.Id}' could not be written to '{externalGlobalSettings.FilePath}', the values apply for this session only");

            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
            return true;
        }

        // CreateNew, and with it CopyAsNew, is sealed in FluentGlobalSettings and returns a plain FluentGlobalSettings,
        // so a copy of the registered settings is accepted by its id
        /// <inheritdoc/>
        public override bool OverrideSettings(BaseSettings settings)
        {
            if (settings is not FluentGlobalSettings || !LoadedSettings.TryGetValue(settings.Id, out var loadedSettings))
                return false;

            SettingsUtils.OverrideSettings(loadedSettings, settings);
            return true;
        }

        /// <inheritdoc />
        public void Register(FluentGlobalSettings settings)
        {
            if (settings is ExternalGlobalSettings externalGlobalSettings)
                RegisterSettings(externalGlobalSettings);
        }

        /// <inheritdoc />
        public void Unregister(FluentGlobalSettings settings)
        {
            if (settings is ExternalGlobalSettings externalGlobalSettings)
                LoadedSettings.Remove(externalGlobalSettings.Id);
        }
    }
}
