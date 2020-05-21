using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Properties;
using MCM.Implementation.MBO.Settings.Properties;
using MCM.Utils;

namespace MCM.Implementation.MBO.Settings.Base
{
    public abstract class BaseMBOGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected override ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IMBOSettingsPropertyDiscoverer, MBOSettingsPropertyDiscovererWrapper>();

        protected BaseMBOGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}