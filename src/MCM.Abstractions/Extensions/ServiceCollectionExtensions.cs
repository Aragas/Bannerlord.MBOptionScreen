using BUTR.DependencyInjection;

using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Global;
using MCM.Abstractions.PerCampaign;
using MCM.Abstractions.PerSave;
using MCM.Abstractions.Properties;

namespace MCM.Abstractions
{
    public static class ServiceCollectionExtensions
    {
        public static IGenericServiceContainer AddSettingsProvider<TService, TImplementation>(this IGenericServiceContainer services)
            where TService : BaseSettingsProvider
            where TImplementation : class, TService
        {
            services.RegisterSingleton<TImplementation>();
            services.RegisterSingleton<TService>(sp => sp.GetService<TImplementation>());
            services.AddSettingsProvider<TImplementation>();
            return services;
        }
        public static IGenericServiceContainer AddSettingsProvider<TImplementation>(this IGenericServiceContainer services)
            where TImplementation : BaseSettingsProvider
        {
            services.RegisterSingleton<TImplementation>();
            services.RegisterSingleton<BaseSettingsProvider>(sp => sp.GetService<TImplementation>());
            return services;
        }

        public static IGenericServiceContainer AddSettingsFormat<TService, TImplementation>(this IGenericServiceContainer services)
            where TService : class, ISettingsFormat
            where TImplementation : class, TService
        {
            services.RegisterSingleton<TImplementation>();
            services.RegisterSingleton<TService>(sp => sp.GetService<TImplementation>());
            services.AddSettingsFormat<TImplementation>();
            return services;
        }
        public static IGenericServiceContainer AddSettingsFormat<TImplementation>(this IGenericServiceContainer services)
            where TImplementation : class, ISettingsFormat
        {
            services.RegisterSingleton<TImplementation>();
            services.RegisterSingleton<ISettingsFormat>(sp => sp.GetService<TImplementation>());
            return services;
        }

        public static IGenericServiceContainer AddSettingsPropertyDiscoverer<TService, TImplementation>(this IGenericServiceContainer services)
            where TService : class, ISettingsPropertyDiscoverer
            where TImplementation : class, TService
        {
            services.RegisterSingleton<TImplementation>();
            services.RegisterSingleton<TService>(sp => sp.GetService<TImplementation>());
            services.AddSettingsPropertyDiscoverer<TImplementation>();
            return services;
        }
        public static IGenericServiceContainer AddSettingsPropertyDiscoverer<TImplementation>(this IGenericServiceContainer services)
            where TImplementation : class, ISettingsPropertyDiscoverer
        {
            services.RegisterSingleton<TImplementation>();
            services.RegisterSingleton<ISettingsPropertyDiscoverer>(sp => sp.GetService<TImplementation>());
            return services;
        }

        public static IGenericServiceContainer AddSettingsContainer<TService, TImplementation>(this IGenericServiceContainer services)
            where TService : class, ISettingsContainer
            where TImplementation : class, TService
        {
            services.RegisterSingleton<TImplementation>();
            services.RegisterSingleton<TService>(sp => sp.GetService<TImplementation>());
            services.AddSettingsContainer<TImplementation>();
            return services;
        }
        public static IGenericServiceContainer AddSettingsContainer<TImplementation>(this IGenericServiceContainer services)
            where TImplementation : class, ISettingsContainer
        {
            services.RegisterSingleton<TImplementation>();

            services.RegisterSingleton<ISettingsContainer>(sp => sp.GetService<TImplementation>());

            if (typeof(IPerCampaignSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.RegisterSingleton<IPerCampaignSettingsContainer>(sp => (IPerCampaignSettingsContainer) sp.GetService<TImplementation>());
            if (typeof(IPerSaveSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.RegisterSingleton<IPerSaveSettingsContainer>(sp => (IPerSaveSettingsContainer) sp.GetService<TImplementation>());
            if (typeof(IGlobalSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.RegisterSingleton<IGlobalSettingsContainer>(sp => (IGlobalSettingsContainer) sp.GetService<TImplementation>());
            if (typeof(IFluentPerCampaignSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.RegisterSingleton<IFluentPerCampaignSettingsContainer>(sp => (IFluentPerCampaignSettingsContainer) sp.GetService<TImplementation>());
            if (typeof(IFluentPerSaveSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.RegisterSingleton<IFluentPerSaveSettingsContainer>(sp => (IFluentPerSaveSettingsContainer) sp.GetService<TImplementation>());
            if (typeof(IFluentGlobalSettingsContainer).IsAssignableFrom(typeof(TImplementation)))
                services.RegisterSingleton<IFluentGlobalSettingsContainer>(sp => (IFluentGlobalSettingsContainer) sp.GetService<TImplementation>());

            return services;
        }

        public static IGenericServiceContainer AddSettingsBuilderFactory<TImplementation>(this IGenericServiceContainer services)
            where TImplementation : class, ISettingsBuilderFactory
        {
            services.RegisterSingleton<TImplementation>();
            services.RegisterSingleton<ISettingsBuilderFactory>(factory => factory.GetService<TImplementation>());
            return services;
        }
    }
}