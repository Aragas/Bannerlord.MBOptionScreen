using MCM.Abstractions.Settings.Base.Global;

namespace MCM.Abstractions.Settings.Containers.Global
{
    public abstract class BaseGlobalSettingsContainer : BaseSettingsContainer<GlobalSettings>, IGlobalSettingsContainer
    {
        protected BaseGlobalSettingsContainer() { }
    }
}