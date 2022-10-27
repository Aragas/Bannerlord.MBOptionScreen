using MCM.Abstractions.Base.PerSave;

namespace MCM.Abstractions.GameFeatures
{
    public interface IPerSaveSettingsProvider
    {
        bool SaveSettings(PerSaveSettings perSaveSettings);
        void LoadSettings(PerSaveSettings perSaveSettings);
    }
}