using MCM.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace MCM.UI.DependencyInjection
{
    internal class ButterLibGenericServiceFactory : IGenericServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ButterLibGenericServiceFactory(IServiceProvider factory) => _serviceProvider = factory;

        public TService GetService<TService>() where TService : class => _serviceProvider.GetRequiredService<TService>();
    }
}