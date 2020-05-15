using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Properties;
using MCM.Implementation.MBO.Settings.Properties;
using MCM.Utils;

namespace MCM.Implementation.MBO.Settings
{
    public abstract class BaseMBOGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IMBOSettingsPropertyDiscoverer, MBOSettingsPropertyDiscovererWrapper>();

        protected BaseMBOGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}