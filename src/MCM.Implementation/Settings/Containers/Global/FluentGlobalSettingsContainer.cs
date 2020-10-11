using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Containers;

namespace MCM.Implementation.Settings.Containers.Global
{
    internal sealed class FluentGlobalSettingsContainer : BaseSettingsContainer<FluentGlobalSettings>, IMCMFluentGlobalSettingsContainer
    {
        public void Register(FluentGlobalSettings settings)
        {
            RegisterSettings(settings);
        }
        public void Unregister(FluentGlobalSettings settings)
        {
            if (LoadedSettings.ContainsKey(settings.Id))
                LoadedSettings.Remove(settings.Id);
        }
    }
}