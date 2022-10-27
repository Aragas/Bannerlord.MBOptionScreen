namespace MCM.Abstractions.Wrapper
{
    public sealed class PropertyDefinitionBoolWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionBool
    {
        public PropertyDefinitionBoolWrapper(object @object) : base(@object) { }
    }
}