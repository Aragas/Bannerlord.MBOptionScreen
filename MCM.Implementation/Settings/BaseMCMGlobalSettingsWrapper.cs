using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

namespace MCM.Implementation.Settings
{
    public abstract class BaseMCMGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscovererWrapper>();

        protected BaseMCMGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}