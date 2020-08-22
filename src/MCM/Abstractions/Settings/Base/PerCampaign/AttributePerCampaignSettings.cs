using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Properties;
using MCM.Extensions;

namespace MCM.Abstractions.Settings.Base.PerCampaign
{
    public abstract class AttributePerCampaignSettings<T> : PerCampaignSettings<T> where T : PerCampaignSettings, new()
    {
        /// <inheritdoc/>
        protected override ISettingsPropertyDiscoverer? Discoverer { get; } =
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscovererWrapper>();
            //DI.GetImplementation<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscovererWrapper>();
    }
}