using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Properties;

using Microsoft.Extensions.DependencyInjection;

namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class AttributeGlobalSettings<T> : GlobalSettings<T> where T : GlobalSettings, new()
    {
        /// <inheritdoc/>
        protected override ISettingsPropertyDiscoverer? Discoverer { get; } =
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IAttributeSettingsPropertyDiscoverer>();
    }
}