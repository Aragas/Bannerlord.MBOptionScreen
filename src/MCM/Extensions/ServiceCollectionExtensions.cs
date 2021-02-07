using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Containers.PerSave;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Properties;
using MCM.Abstractions.Settings.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;

namespace MCM.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSettingsProvider<TService, TImplementation>(this IServiceCollection services)
            where TService : BaseSettingsProvider
            where TImplementation : class, TService
        {
            services.AddSingleton<TImplementation>();
            services.AddSingleton<TService, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            services.AddSettingsProvider<TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsProvider<TImplementation>(this IServiceCollection services)
            where TImplementation : BaseSettingsProvider
        {
            services.AddSingleton<TImplementation>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<BaseSettingsProvider, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));
            return services;
        }

        public static IServiceCollection AddSettingsFormat<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsFormat
            where TImplementation : class, TService
        {
            services.AddSingleton<TImplementation>();
            services.AddSingleton<TService, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            services.AddSettingsFormat<TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsFormat<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsFormat
        {
            services.AddSingleton<TImplementation>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ISettingsFormat, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));
            return services;
        }

        public static IServiceCollection AddSettingsPropertyDiscoverer<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsPropertyDiscoverer
            where TImplementation : class, TService
        {
            services.AddSingleton<TImplementation>();
            services.AddSingleton<TService, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            services.AddSettingsPropertyDiscoverer<TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsPropertyDiscoverer<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsPropertyDiscoverer
        {
            services.AddSingleton<TImplementation>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ISettingsPropertyDiscoverer, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));
            return services;
        }

        public static IServiceCollection AddSettingsContainer<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsContainer
            where TImplementation : class, TService
        {
            services.AddSingleton<TImplementation>();
            services.AddSingleton<TService, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            services.AddSettingsContainer<TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsContainer<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsContainer
        {
            services.AddSingleton<TImplementation>();

            services.TryAddEnumerable(ServiceDescriptor.Singleton<ISettingsContainer, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));

            if (typeof(IPerSaveSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.TryAddEnumerable(SingletonN<IPerSaveSettingsContainer, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));
            if (typeof(IGlobalSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.TryAddEnumerable(SingletonN<IGlobalSettingsContainer, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));
            if (typeof(IFluentPerSaveSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.TryAddEnumerable(SingletonN<IFluentPerSaveSettingsContainer, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));
            if (typeof(IFluentGlobalSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.TryAddEnumerable(SingletonN<IFluentGlobalSettingsContainer, TImplementation>(sp => sp.GetRequiredService<TImplementation>()));

            return services;
        }

        public static IServiceCollection AddSettingsBuilderFactory<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsBuilderFactory
        {
            services.AddSingleton<TImplementation>();
            services.AddSingleton<ISettingsBuilderFactory, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
            return services;
        }


        private static ServiceDescriptor SingletonN<TService, TImplementation>(Func<IServiceProvider, TImplementation>? implementationFactory)
            where TService : class
            where TImplementation : class
        {
            return implementationFactory is not null
                ? ServiceDescriptor.Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton)
                : throw new ArgumentNullException(nameof(implementationFactory));
        }
    }
}