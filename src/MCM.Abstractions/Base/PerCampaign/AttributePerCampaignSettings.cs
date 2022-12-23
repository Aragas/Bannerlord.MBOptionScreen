namespace MCM.Abstractions.Base.PerCampaign
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    abstract class AttributePerCampaignSettings<T> : PerCampaignSettings<T> where T : PerCampaignSettings, new()
    {
        public sealed override string DiscoveryType => "attributes";
    }
}