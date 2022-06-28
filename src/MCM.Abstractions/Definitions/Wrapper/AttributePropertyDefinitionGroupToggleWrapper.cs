
namespace MCM.Abstractions.Wrapper
{
    public sealed class AttributePropertyDefinitionGroupToggleWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionGroupToggle
    {
        /// <inheritdoc/>
        public bool IsToggle { get; } = true;

        public AttributePropertyDefinitionGroupToggleWrapper(object @object) : base(@object) { }
    }
}