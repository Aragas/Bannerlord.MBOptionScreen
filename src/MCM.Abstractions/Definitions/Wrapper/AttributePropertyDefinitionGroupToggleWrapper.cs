
namespace MCM.Abstractions.Wrapper
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class AttributePropertyDefinitionGroupToggleWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionGroupToggle
    {
        /// <inheritdoc/>
        public bool IsToggle { get; } = true;

        public AttributePropertyDefinitionGroupToggleWrapper(object @object) : base(@object) { }
    }
}