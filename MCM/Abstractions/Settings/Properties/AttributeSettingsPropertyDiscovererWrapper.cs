namespace MCM.Abstractions.Settings.Properties
{
    public sealed class AttributeSettingsPropertyDiscovererWrapper :
        BaseSettingsPropertyDiscovererWrapper,
        IAttributeSettingsPropertyDiscoverer
    {
        public AttributeSettingsPropertyDiscovererWrapper(object @object) : base(@object) { }
    }
}