using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Properties;

using Microsoft.Extensions.DependencyInjection;

namespace MCM.Abstractions.Settings.Base.PerCampaign
{
    public abstract class AttributePerCampaignSettings<T> : PerCampaignSettings<T> where T : PerCampaignSettings, new()
    {
        /// <inheritdoc/>
        protected override ISettingsPropertyDiscoverer? Discoverer =>
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IAttributeSettingsPropertyDiscoverer>();
    }
}