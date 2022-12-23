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
    sealed class PropertyDefinitionWithEditableMinMaxWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithEditableMinMax
    {
        /// <inheritdoc/>
        public decimal EditableMinValue { get; }
        /// <inheritdoc/>
        public decimal EditableMaxValue { get; }

        public PropertyDefinitionWithEditableMinMaxWrapper(object @object) : base(@object)
        {
            EditableMinValue = @object.GetType().GetProperty(nameof(EditableMinValue))?.GetValue(@object) as decimal? ?? 0;
            EditableMaxValue = @object.GetType().GetProperty(nameof(EditableMaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}