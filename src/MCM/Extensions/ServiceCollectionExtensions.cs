using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Containers.PerSave;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Properties;
using MCM.Abstractions.Settings.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MCM.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSettingsProvider<TService, TImplementation>(this IServiceCollection services)
            where TService : BaseSettingsProvider
            where TImplementation : class, TService
        {
            services.AddSettingsProvider<TImplementation>();
            services.AddSingleton<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsProvider<TImplementation>(this IServiceCollection services)
            where TImplementation : BaseSettingsProvider
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<BaseSettingsProvider, TImplementation>());
            return services;
        }

        public static IServiceCollection AddSettingsFormat<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsFormat
            where TImplementation : class, TService
        {
            services.AddSettingsFormat<TImplementation>();
            services.AddSingleton<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsFormat<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsFormat
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ISettingsFormat, TImplementation>());

            return services;
        }

        public static IServiceCollection AddSettingsPropertyDiscoverer<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsPropertyDiscoverer
            where TImplementation : class, TService
        {
            services.AddSettingsPropertyDiscoverer<TImplementation>();
            services.AddSingleton<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsPropertyDiscoverer<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsPropertyDiscoverer
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ISettingsPropertyDiscoverer, TImplementation>());
            return services;
        }

        public static IServiceCollection AddSettingsContainer<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsContainer
            where TImplementation : class, TService
        {
            services.AddSettingsContainer<TImplementation>();
            services.AddSingleton<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsContainer<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsContainer
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ISettingsContainer, TImplementation>());

            if (typeof(IPerSaveSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
            {
                services.AddSingleton(typeof(IPerSaveSettingsContainer), typeof(TImplementation));
            }
            if (typeof(IGlobalSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
            {
                services.AddSingleton(typeof(IGlobalSettingsContainer), typeof(TImplementation));
            }
            return services;
        }

        public static IServiceCollection AddSettingsBuilderFactory<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsBuilderFactory
        {
            services.AddSingleton<ISettingsBuilderFactory, TImplementation>();
            return services;
        }
    }
}