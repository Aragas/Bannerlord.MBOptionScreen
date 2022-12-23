using MCM.Abstractions.Base.PerSave;

namespace MCM.Abstractions.PerSave
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IFluentPerSaveSettingsContainer : IPerSaveSettingsContainer
    {
        void Register(FluentPerSaveSettings settings);
        void Unregister(FluentPerSaveSettings settings);
    }
}