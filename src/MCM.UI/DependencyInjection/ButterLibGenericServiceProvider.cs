using Bannerlord.ButterLib.Common.Extensions;

using MCM.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace MCM.UI.DependencyInjection
{
    internal class ButterLibGenericServiceProvider : IGenericServiceProvider
    {
        private static IServiceProvider? ServiceProvider => DependencyInjectionExtensions.GetTempServiceProvider(MCMSubModule.Instance!) ??
                                                            DependencyInjectionExtensions.GetServiceProvider(MCMSubModule.Instance!);

        public TService? GetService<TService>() where TService : class => ServiceProvider?.GetRequiredService<TService>();
    }
}