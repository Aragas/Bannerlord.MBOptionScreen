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
    abstract class AttributePerSaveSettings<T> : PerSaveSettings<T> where T : PerSaveSettings, new()
    {
        public sealed override string DiscoveryType => "attributes";
    }
}