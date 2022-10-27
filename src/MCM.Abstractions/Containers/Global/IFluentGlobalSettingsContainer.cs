using MCM.Abstractions.Base.Global;

namespace MCM.Abstractions.Global
{
    public interface IFluentGlobalSettingsContainer : IGlobalSettingsContainer
    {
        void Register(FluentGlobalSettings settings);
        void Unregister(FluentGlobalSettings settings);
    }
}