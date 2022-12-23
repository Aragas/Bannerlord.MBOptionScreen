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
    abstract class AttributeGlobalSettings<T> : GlobalSettings<T> where T : GlobalSettings, new()
    {
        public sealed override string DiscoveryType => "attributes";
    }
}