using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

namespace MCM.Implementation.Settings
{
    public abstract class BaseMCMPerCharacterSettingsWrapper : BasePerCharacterSettingsWrapper
    {
        protected ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscovererWrapper>();

        protected BaseMCMPerCharacterSettingsWrapper(object @object) : base(@object) { }
    }
}