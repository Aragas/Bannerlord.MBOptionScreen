using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Global;

namespace MCM.Implementation.Global
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class FluentGlobalSettingsContainer : BaseSettingsContainer<FluentGlobalSettings>, IFluentGlobalSettingsContainer
    {
        public void Register(FluentGlobalSettings settings)
        {
            // Ignore derived types, only register concrete FluentGlobalSettings
            if (settings.GetType() != typeof(FluentGlobalSettings))
                return;

            RegisterSettings(settings);
        }
        public void Unregister(FluentGlobalSettings settings)
        {
            // Ignore derived types, only register concrete FluentGlobalSettings
            if (settings.GetType() != typeof(FluentGlobalSettings))
                return;

            if (LoadedSettings.ContainsKey(settings.Id))
                LoadedSettings.Remove(settings.Id);
        }
    }
}