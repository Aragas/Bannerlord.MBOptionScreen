using MCM.Abstractions.Base;

namespace MCM.Abstractions
{
    public interface ISettingsContainer
    {
        BaseSettings? GetSettings(string id);
        bool SaveSettings(BaseSettings settings);
    }
}