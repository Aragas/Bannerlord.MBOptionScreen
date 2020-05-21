using MCM.Abstractions.Settings.Base.Global;

namespace MCM.Abstractions.Settings.Containers.Global
{
    public interface IFluentGlobalSettingsContainer : IGlobalSettingsContainer, IDependency
    {
        void Register(FluentGlobalSettings settings);
        void Unregister(FluentGlobalSettings settings);
    }
}