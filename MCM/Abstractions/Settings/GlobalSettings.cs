using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

namespace MCM.Abstractions.Settings
{
    public abstract class GlobalSettings<T> : GlobalSettings where T : GlobalSettings, new()
    {
        public static T? Instance => SettingsUtils.UnwrapSettings(BaseSettingsProvider.Instance.GetSettings(new T().Id)) as T;
    }

    public abstract class GlobalSettings : BaseSettings
    {
        public abstract string ModuleFolderName { get; }
        public virtual string SubFolder => "";
        public virtual string Format => "json";
    }
}