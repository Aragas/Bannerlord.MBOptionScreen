using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

namespace MCM.Abstractions.Settings
{
    public abstract class PerCharacterSettings<T> : PerCharacterSettings where T : PerCharacterSettings, new()
    {
        public static T? Instance => SettingsUtils.UnwrapSettings(BaseSettingsProvider.Instance.GetSettings(new T().Id)) as T;
    }

    public abstract class PerCharacterSettings : BaseSettings
    {
        public string CharacterId { get; } = "ERROR";
    }
}