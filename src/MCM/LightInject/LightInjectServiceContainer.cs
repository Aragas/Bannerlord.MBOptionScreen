using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using System;
using System.Linq;

namespace MCM.LightInject
{
    internal class LightInjectServiceContainer : IGenericServiceContainer
    {
        private readonly IServiceContainer _serviceContainer;

        public LightInjectServiceContainer(IServiceContainer serviceContainer) => _serviceContainer = serviceContainer;

        /// <inheritdoc />
        public IGenericServiceContainer RegisterSingleton<TService>() where TService : class
        {
            _serviceContainer.RegisterSingleton<TService>();
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterSingleton<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            _serviceContainer.RegisterSingleton(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterSingleton<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterSingleton<TService, TImplementation>(Func<IGenericServiceFactory, TImplementation> factory) where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterSingleton<TService>(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterSingleton(Type serviceType, Type implementationType)
        {
            _serviceContainer.RegisterSingleton(serviceType, implementationType);
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterSingleton(Type serviceType, Func<object> factory)
        {
            _serviceContainer.RegisterSingleton(serviceType, _ => factory());
            return this;
        }


        /// <inheritdoc />
        public IGenericServiceContainer RegisterScoped<TService>() where TService : class
        {
            _serviceContainer.RegisterScoped<TService>();
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterScoped<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            _serviceContainer.RegisterScoped(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterScoped<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterScoped<TService, TImplementation>(Func<IGenericServiceFactory, TImplementation> factory) where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterScoped<TService>(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterScoped(Type serviceType, Type implementationType)
        {
            _serviceContainer.RegisterScoped(serviceType, implementationType);
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterScoped(Type serviceType, Func<object> factory)
        {
            _serviceContainer.RegisterScoped(serviceType, _ => factory());
            return this;
        }


        /// <inheritdoc />
        public IGenericServiceContainer RegisterTransient<TService>() where TService : class
        {
            _serviceContainer.RegisterTransient<TService>();
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterTransient<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class
        {
            _serviceContainer.RegisterTransient(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterTransient<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterTransient<TService, TImplementation>(Func<IGenericServiceFactory, TImplementation> factory) where TService : class where TImplementation : class, TService
        {
            _serviceContainer.RegisterTransient<TService>(lightInjectFactory => factory(new LightInjectGenericServiceFactory(lightInjectFactory)));
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterTransient(Type serviceType, Type implementationType)
        {
            _serviceContainer.RegisterTransient(serviceType, implementationType);
            return this;
        }

        /// <inheritdoc />
        public IGenericServiceContainer RegisterTransient(Type serviceType, Func<object> factory)
        {
            _serviceContainer.RegisterTransient(serviceType, _ => factory());
            return this;
        }


        /// <inheritdoc />
        public IGenericServiceProvider Build()
        {
            if (_serviceContainer.AvailableServices.All(s => s.ServiceType != typeof(IBUTRLogger)))
            {
                _serviceContainer.RegisterTransient<IBUTRLogger, DefaultBUTRLogger>();
            }
            if (_serviceContainer.AvailableServices.All(s => s.ServiceType != typeof(IBUTRLogger<>)))
            {
                _serviceContainer.RegisterTransient(typeof(IBUTRLogger<>), typeof(DefaultBUTRLogger<>));
            }

            _serviceContainer.Compile();
            return new LightInjectGenericServiceProvider(_serviceContainer);
        }
    }
}