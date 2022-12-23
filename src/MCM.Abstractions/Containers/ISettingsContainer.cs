using MCM.Abstractions.Base;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsContainer
    {
        BaseSettings? GetSettings(string id);
        bool SaveSettings(BaseSettings settings);
    }
}