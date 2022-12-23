using MCM.Abstractions.Base;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPreset
    {
        string SettingsId { get; }
        string Id { get; }
        string Name { get; }

        BaseSettings LoadPreset();
        bool SavePreset(BaseSettings settings);
    }
}