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
    sealed class PropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        /// <inheritdoc/>
        public string GroupName { get; }
        /// <inheritdoc/>
        public int GroupOrder { get; }

        public PropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty(nameof(GroupName))?.GetValue(@object) as string ?? "ERROR";
            GroupOrder = type.GetProperty(nameof(GroupOrder))?.GetValue(@object) as int? ?? -1;
        }
    }
}