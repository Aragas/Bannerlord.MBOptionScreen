using MCM.Abstractions.Base;

namespace MCM.Abstractions
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