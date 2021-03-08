using Bannerlord.ButterLib.Common.Extensions;

using MCM.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace MCM.UI.DependencyInjection
{
    internal class ButterLibServiceContainer : IGenericServiceContainer
    {
        private static IServiceCollection? ServiceContainer => DependencyInjectionExtensions.GetServices(MCMSubModule.Instance!);

        public IGenericServiceContainer RegisterSingleton<TService>() where TService : class
        {
            ServiceContainer.AddSingleton<TService>();
            return this;
        }

        public IGenericServiceContainer RegisterSingleton<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            ServiceContainer.AddSingleton<TService>(sp => factory(new ButterLibGenericServiceFactory(sp)));
            return this;
        }

        public IGenericServiceContainer RegisterSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            ServiceContainer.AddSingleton<TService, TImplementation>();
            return this;
        }

        public IGenericServiceContainer RegisterScoped<TService>() where TService : class
        {
            ServiceContainer.AddScoped<TService>();
            return this;
        }

        public IGenericServiceContainer RegisterScoped<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            ServiceContainer.AddScoped<TService>(sp => factory(new ButterLibGenericServiceFactory(sp)));
            return this;
        }

        public IGenericServiceContainer RegisterScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            ServiceContainer.AddScoped<TService, TImplementation>();
            return this;
        }

        public IGenericServiceContainer RegisterTransient(Type serviceType, Type implementationType)
        {
            ServiceContainer.AddTransient(serviceType, implementationType);
            return this;
        }

        public IGenericServiceContainer RegisterTransient<TService>() where TService : class
        {
            ServiceContainer.AddTransient<TService>();
            return this;
        }

        public IGenericServiceContainer RegisterTransient<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            ServiceContainer.AddTransient<TService>(sp => factory(new ButterLibGenericServiceFactory(sp)));
            return this;
        }

        public IGenericServiceContainer RegisterTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            ServiceContainer.AddTransient<TService, TImplementation>();
            return this;
        }

        public IGenericServiceContainer RegisterTransient(Type serviceType, Func<object> factory)
        {
            ServiceContainer.AddTransient(serviceType, _ => factory());
            return this;
        }

        public IGenericServiceContainer RegisterTransient<TService>(Func<TService> factory) where TService : class
        {
            ServiceContainer.AddTransient<TService>(_ => factory());
            return this;
        }

        public IGenericServiceProvider Build()
        {
            return new ButterLibGenericServiceProvider();
        }
    }
}
