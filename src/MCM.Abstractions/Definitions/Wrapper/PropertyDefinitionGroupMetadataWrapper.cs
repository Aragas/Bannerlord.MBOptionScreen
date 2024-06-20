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
    sealed class PropertyDefinitionGroupMetadataWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionGroupMetadata
    {
        public PropertyDefinitionGroupMetadataWrapper(object @object) : base(@object) { }
    }
}