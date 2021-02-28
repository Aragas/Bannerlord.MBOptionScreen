using System;
using System.Collections.Generic;

namespace MCM.DependencyInjection
{
    public class WithHistoryGenericServiceContainer : IGenericServiceContainer
    {
        public List<Action<IGenericServiceContainer>> History { get; } = new();

        private readonly IGenericServiceContainer _serviceContainer;

        public WithHistoryGenericServiceContainer(IGenericServiceContainer serviceContainer) => _serviceContainer = serviceContainer;

        public IGenericServiceContainer RegisterSingleton<TService>() where TService : class
        {
            History.Add(serviceContainer => serviceContainer.RegisterSingleton<TService>());
            return _serviceContainer.RegisterSingleton<TService>();
        }

        public IGenericServiceContainer RegisterSingleton<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            History.Add(serviceContainer => serviceContainer.RegisterSingleton(factory));
            return _serviceContainer.RegisterSingleton(factory);
        }

        public IGenericServiceContainer RegisterSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            History.Add(serviceContainer => serviceContainer.RegisterSingleton<TService, TImplementation>());
            return _serviceContainer.RegisterSingleton<TService, TImplementation>();
        }

        public IGenericServiceContainer RegisterScoped<TService>() where TService : class
        {
            History.Add(serviceContainer => serviceContainer.RegisterScoped<TService>());
            return _serviceContainer.RegisterScoped<TService>();
        }

        public IGenericServiceContainer RegisterScoped<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            History.Add(serviceContainer => serviceContainer.RegisterScoped(factory));
            return _serviceContainer.RegisterScoped(factory);
        }

        public IGenericServiceContainer RegisterScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            History.Add(serviceContainer => serviceContainer.RegisterScoped<TService, TImplementation>());
            return _serviceContainer.RegisterScoped<TService, TImplementation>();
        }

        public IGenericServiceContainer RegisterTransient(Type serviceType, Type implementationType)
        {
            History.Add(serviceContainer => serviceContainer.RegisterTransient(serviceType, implementationType));
            return _serviceContainer.RegisterTransient(serviceType, implementationType);
        }

        public IGenericServiceContainer RegisterTransient<TService>() where TService : class
        {
            History.Add(serviceContainer => serviceContainer.RegisterTransient<TService>());
            return _serviceContainer.RegisterTransient<TService>();
        }

        public IGenericServiceContainer RegisterTransient<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            History.Add(serviceContainer => serviceContainer.RegisterTransient(factory));
            return _serviceContainer.RegisterTransient(factory);
        }

        public IGenericServiceContainer RegisterTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            History.Add(serviceContainer => serviceContainer.RegisterTransient<TService, TImplementation>());
            return _serviceContainer.RegisterTransient<TService, TImplementation>();
        }

        public IGenericServiceContainer RegisterTransient<TService>(Func<TService> factory) where TService : class
        {
            History.Add(serviceContainer => serviceContainer.RegisterTransient(factory));
            return _serviceContainer.RegisterTransient(factory);
        }

        public IGenericServiceContainer RegisterTransient(Type serviceType, Func<object> factory)
        {
            History.Add(serviceContainer => serviceContainer.RegisterTransient(serviceType, factory));
            return _serviceContainer.RegisterTransient(serviceType, factory);
        }

        public IGenericServiceProvider Build()
        {
            History.Clear();
            return _serviceContainer.Build();
        }
    }
}