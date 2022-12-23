using MCM.Abstractions.Base.Global;

namespace MCM.Abstractions.Global
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IFluentGlobalSettingsContainer : IGlobalSettingsContainer
    {
        void Register(FluentGlobalSettings settings);
        void Unregister(FluentGlobalSettings settings);
    }
}