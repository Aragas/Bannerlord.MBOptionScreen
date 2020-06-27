namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class AttributePropertyDefinitionGroupToggleWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionGroupToggle
    {
        public bool IsToggle { get; } = true;
        
        public AttributePropertyDefinitionGroupToggleWrapper(object @object) : base(@object) { }
    }
}