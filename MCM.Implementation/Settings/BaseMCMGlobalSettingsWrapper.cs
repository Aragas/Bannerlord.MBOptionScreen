using MCM.Abstractions.Settings;

namespace MCM.Implementation.Settings
{
    public abstract class BaseMCMGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected BaseMCMGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}