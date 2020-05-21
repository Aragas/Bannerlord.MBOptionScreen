using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class AttributeGlobalSettings<T> : GlobalSettings<T> where T : GlobalSettings, new()
    {
        protected override ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscovererWrapper>();
    }
}