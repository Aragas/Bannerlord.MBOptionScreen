using BUTR.DependencyInjection;

namespace MCM.Internal.Extensions
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    static class ServiceCollectionExtensions
    {
        public static IGenericServiceContainer GetServiceContainer(this TaleWorlds.MountAndBlade.MBSubModuleBase _) =>
            BUTR.DependencyInjection.Extensions.ServiceCollectionExtensions.ServiceContainer;
    }
}