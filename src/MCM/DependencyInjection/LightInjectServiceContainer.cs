using MCM.LightInject;
using MCM.Logger;

using System;
using System.Linq;

namespace MCM.DependencyInjection
{
    internal class LightInjectServiceContainer : IGenericServiceContainer
    {
        private readonly IServiceContainer _serviceContainer;

        public LightInjectServiceContainer(IServiceContainer serviceContainer) => _serviceContainer = serviceContainer;

        public IGenericServiceContainer RegisterSingleton<TService>() where TService : class
        {
            _serviceContainer.RegisterSingleton<TService>();
            return this;
        }

        public IGenericServiceContainer RegisterSingleton<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            _serviceContainer.RegisterSingleton(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        public IGenericServiceContainer RegisterSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterSingleton<TService, TImplementation>();
            return this;
        }

        public IGenericServiceContainer RegisterScoped<TService>() where TService : class
        {
            _serviceContainer.RegisterScoped<TService>();
            return this;
        }

        public IGenericServiceContainer RegisterScoped<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            _serviceContainer.RegisterScoped(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        public IGenericServiceContainer RegisterScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterScoped<TService, TImplementation>();
            return this;
        }

        public IGenericServiceContainer RegisterTransient(Type serviceType, Type implementationType)
        {
            _serviceContainer.RegisterTransient(serviceType, implementationType);
            return this;
        }

        public IGenericServiceContainer RegisterTransient<TService>() where TService : class
        {
            _serviceContainer.RegisterTransient<TService>();
            return this;
        }

        public IGenericServiceContainer RegisterTransient<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            _serviceContainer.RegisterTransient(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        public IGenericServiceContainer RegisterTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterTransient<TService, TImplementation>();
            return this;
        }

        public IGenericServiceContainer RegisterTransient(Type serviceType, Func<object> factory)
        {
            _serviceContainer.RegisterTransient(serviceType, _ => factory());
            return this;
        }

        public IGenericServiceContainer RegisterTransient<TService>(Func<TService> factory) where TService : class
        {
            _serviceContainer.RegisterTransient<TService>(_ => factory());
            return this;
        }

        public IGenericServiceProvider Build()
        {
            if (_serviceContainer.AvailableServices.All(s => s.ServiceType != typeof(IMCMLogger)))
            {
                _serviceContainer.RegisterTransient<IMCMLogger, NullMCMLogger>();
            }
            if (_serviceContainer.AvailableServices.All(s => s.ServiceType != typeof(IMCMLogger<>)))
            {
                _serviceContainer.RegisterTransient(typeof(IMCMLogger<>), typeof(NullMCMLogger<>));
            }

            _serviceContainer.Compile();
            return new LightInjectGenericServiceProvider(_serviceContainer);
        }
    }
}