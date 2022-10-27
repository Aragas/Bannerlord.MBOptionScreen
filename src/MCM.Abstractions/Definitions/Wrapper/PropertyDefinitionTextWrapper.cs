namespace MCM.Abstractions.Wrapper
{
    public sealed class PropertyDefinitionTextWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionText
    {
        public PropertyDefinitionTextWrapper(object @object) : base(@object) { }
    }
}