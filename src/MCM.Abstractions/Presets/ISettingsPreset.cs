using MCM.Abstractions.Settings.Base;

namespace MCM.Abstractions.Settings.Presets
{
    public interface ISettingsPreset
    {
        string SettingsId { get; }
        string Id { get; }
        string Name { get; }
        
        BaseSettings LoadPreset();
        bool SavePreset(BaseSettings settings);
    }
}