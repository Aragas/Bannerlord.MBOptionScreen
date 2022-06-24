using MCM.Abstractions.Settings.Base;

namespace MCM.Abstractions.Settings.Containers
{
    public interface ISettingsContainer
    {
        BaseSettings? GetSettings(string id);
        bool SaveSettings(BaseSettings settings);
    }
}