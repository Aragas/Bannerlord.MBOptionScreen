
namespace MCM.Abstractions.Base.Global
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class EmptyGlobalSettings : GlobalSettings<EmptyGlobalSettings>
    {
        /// <inheritdoc/>
        public override string Id => "empty_v1";
        /// <inheritdoc/>
        public override string DisplayName => "Empty Global Settings";
    }
}