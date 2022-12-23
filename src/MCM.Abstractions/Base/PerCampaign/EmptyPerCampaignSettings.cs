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
    sealed class EmptyPerCampaignSettings : PerCampaignSettings<EmptyPerCampaignSettings>
    {
        /// <inheritdoc/>
        public override string Id => "empty_percampaign_v1";
        /// <inheritdoc/>
        public override string DisplayName => "Empty PerCampaign Settings";
    }
}