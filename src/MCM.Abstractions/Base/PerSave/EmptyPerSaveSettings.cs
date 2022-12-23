namespace MCM.Abstractions.Base.PerSave
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class EmptyPerSaveSettings : PerSaveSettings<EmptyPerSaveSettings>
    {
        /// <inheritdoc/>
        public override string Id => "empty_persave_v1";
        /// <inheritdoc/>
        public override string DisplayName => "Empty PerSave Settings";
    }
}