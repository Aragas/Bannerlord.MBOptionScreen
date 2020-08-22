using System;
using MCM.Abstractions;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Base.PerCampaign;
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
        public static TService GetRequiredService<TService, TServiceWrapper>(this IServiceProvider provider)
            where TServiceWrapper : TService, IWrapper
        {
            return provider.GetRequiredService<TService>();
        }


        public static IServiceCollection AddSettingsProvider<TService, TImplementation>(this IServiceCollection services)
            where TService : BaseSettingsProvider
            where TImplementation : class, TService
        {
            services.AddSettingsProvider<TImplementation>();
            services.AddTransient<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsProvider<TImplementation>(this IServiceCollection services)
            where TImplementation : BaseSettingsProvider
        {
            services.AddTransient<BaseSettingsProvider, TImplementation>();
            return services;
        }

        public static IServiceCollection AddSettingsFormat<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsFormat
            where TImplementation : class, TService
        {
            services.AddSettingsFormat<TImplementation>();
            services.AddTransient<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsFormat<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsFormat
        {
            services.AddTransient<ISettingsFormat, TImplementation>();
            return services;
        }

        public static IServiceCollection AddSettingsPropertyDiscoverer<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsPropertyDiscoverer
            where TImplementation : class, TService
        {
            services.AddSettingsPropertyDiscoverer<TImplementation>();
            services.AddTransient<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsPropertyDiscoverer<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsPropertyDiscoverer
        {
            services.AddTransient<ISettingsPropertyDiscoverer, TImplementation>();
            return services;
        }

        public static IServiceCollection AddSettingsContainer<TService, TImplementation>(this IServiceCollection services)
            where TService : class, ISettingsContainer
            where TImplementation : class, TService
        {
            services.AddSettingsContainer<TImplementation>();
            services.AddTransient<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsContainer<TImplementation>(this IServiceCollection services)
            where TImplementation : class, ISettingsContainer
        {
            services.AddTransient<ISettingsContainer, TImplementation>();
            if (typeof(IPerCampaignSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
            {
                services.AddTransient(typeof(IPerCampaignSettingsContainer), typeof(TImplementation));
            }
            if (typeof(IGlobalSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
            {
                services.AddTransient(typeof(IGlobalSettingsContainer), typeof(TImplementation));
            }
            return services;
        }

        public static IServiceCollection AddSettingsContainerWrapper<TService, TImplementation>(this IServiceCollection services)
            where TService : class, IWrapper
            where TImplementation : class, TService, IWrapper
        {
            services.AddSettingsContainerWrapper<TImplementation>();
            services.AddTransient<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddSettingsContainerWrapper<TImplementation>(this IServiceCollection services)
            where TImplementation : class, IWrapper
        {
            if (typeof(BaseGlobalSettingsWrapper).IsAssignableFrom(typeof(TImplementation)))
            {
                services.AddTransient(typeof(BaseGlobalSettingsWrapper), typeof(TImplementation));
            }
            if (typeof(BasePerCampaignSettingsWrapper).IsAssignableFrom(typeof(TImplementation)))
            {
                services.AddTransient(typeof(BasePerCampaignSettingsWrapper), typeof(TImplementation));
            }
            return services;
        }
    }
}