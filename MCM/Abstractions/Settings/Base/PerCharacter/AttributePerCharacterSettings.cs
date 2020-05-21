using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

namespace MCM.Abstractions.Settings.Base.PerCharacter
{
    public abstract class AttributePerCharacterSettings<T> : PerCharacterSettings<T> where T : PerCharacterSettings, new()
    {
        protected override ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscovererWrapper>();
    }
}