using Bannerlord.ButterLib.Common.Extensions;

using MCM.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace MCM.UI.DependencyInjection
{
    internal class ButterLibGenericServiceProvider : IGenericServiceProvider
    {
        private IServiceProvider ServiceProvider => MCMSubModule.Instance?.GetTempServiceProvider() ?? MCMSubModule.Instance?.GetServiceProvider();

        public TService GetService<TService>() where TService : class => ServiceProvider.GetRequiredService<TService>();
    }
}