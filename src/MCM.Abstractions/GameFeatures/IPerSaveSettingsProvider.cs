using MCM.Abstractions.Base.PerSave;

namespace MCM.Abstractions.GameFeatures
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPerSaveSettingsProvider
    {
        bool SaveSettings(PerSaveSettings perSaveSettings);
        void LoadSettings(PerSaveSettings perSaveSettings);
    }
}