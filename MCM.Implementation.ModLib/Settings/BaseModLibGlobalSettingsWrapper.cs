using MCM.Abstractions.Settings;

namespace MCM.Implementation.ModLib.Settings
{
    public abstract class BaseModLibGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected BaseModLibGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}