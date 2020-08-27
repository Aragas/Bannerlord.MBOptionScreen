using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Containers.PerCampaign;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Properties;
using MCM.Abstractions.Settings.Providers;

using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<BaseSettingsProvider, TImplementation>();
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
            services.AddSingleton<ISettingsFormat, TImplementation>();
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
            services.AddSingleton<ISettingsPropertyDiscoverer, TImplementation>();
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
            services.AddSingleton<ISettingsContainer, TImplementation>();
            if (typeof(IPerCampaignSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
            {
                services.AddSingleton(typeof(IPerCampaignSettingsContainer), typeof(TImplementation));
            }
            if (typeof(IGlobalSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
            {
                services.AddSingleton(typeof(IGlobalSettingsContainer), typeof(TImplementation));
            }
            return services;
        }
    }
}