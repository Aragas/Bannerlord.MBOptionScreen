namespace MCM.Abstractions.Settings.Base.PerCampaign
{
    public sealed class EmptyPerCampaignSettings : PerCampaignSettings<EmptyPerCampaignSettings>
    {
        /// <inheritdoc/>
        public override string Id => "empty_percampaign_v1";
        /// <inheritdoc/>
        public override string DisplayName => "Empty PerCampaign Settings";
    }
}