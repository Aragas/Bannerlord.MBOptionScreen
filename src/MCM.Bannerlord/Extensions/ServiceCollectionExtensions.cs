using BUTR.DependencyInjection;

namespace MCM.Internal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IGenericServiceContainer GetServiceContainer(this TaleWorlds.MountAndBlade.MBSubModuleBase _) =>
            BUTR.DependencyInjection.Extensions.ServiceCollectionExtensions.ServiceContainer;
    }
}