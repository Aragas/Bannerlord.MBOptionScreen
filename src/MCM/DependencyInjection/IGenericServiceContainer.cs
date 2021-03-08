using System;

namespace MCM.DependencyInjection
{
    public interface IGenericServiceProvider
    {
        TService? GetService<TService>() where TService : class;
    }

    public interface IGenericServiceContainer
    {
        IGenericServiceContainer RegisterSingleton<TService>() where TService : class;
        IGenericServiceContainer RegisterSingleton<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class;
        IGenericServiceContainer RegisterSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        IGenericServiceContainer RegisterScoped<TService>() where TService : class;
        IGenericServiceContainer RegisterScoped<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class;
        IGenericServiceContainer RegisterScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        IGenericServiceContainer RegisterTransient(Type serviceType, Type implementationType);
        IGenericServiceContainer RegisterTransient<TService>() where TService : class;
        IGenericServiceContainer RegisterTransient<TService>(Func<IGenericServiceFactory, TService> factory) where TService : class;
        IGenericServiceContainer RegisterTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService;
        IGenericServiceContainer RegisterTransient<TService>(Func<TService> factory) where TService : class;
        IGenericServiceContainer RegisterTransient(Type serviceType, Func<object> factory);

        IGenericServiceProvider Build();
    }
}