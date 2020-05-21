using MCM.Abstractions.Settings.Providers;
using MCM.Utils;

namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class GlobalSettings<T> : GlobalSettings where T : GlobalSettings, new()
    {
        public static T? Instance => SettingsUtils.UnwrapSettings(BaseSettingsProvider.Instance.GetSettings(new T().Id)) as T;
    }

    public abstract class GlobalSettings : BaseSettings
    {

    }
}