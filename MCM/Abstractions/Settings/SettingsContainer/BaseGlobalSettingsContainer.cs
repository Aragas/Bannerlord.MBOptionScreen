using System.IO;

namespace MCM.Abstractions.Settings.SettingsContainer
{
    public abstract class BaseGlobalSettingsContainer : BaseSettingsContainer<GlobalSettings>
    {
        protected override string RootFolder { get; }

        protected BaseGlobalSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "Global");
        }
    }
}