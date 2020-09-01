extern alias v3;

using HarmonyLib;

using System.Collections.Generic;

using TaleWorlds.MountAndBlade;

using MCMv3IIntegratedLoader = v3::MCM.Abstractions.Loader.IIntegratedLoader;
using MCMv3IntegratedLoaderSubModule = v3::MCM.IntegratedLoaderSubModule;

namespace MCM.Implementation.MCMv3.Patches
{
    internal static class MCMv3IntegratedLoaderSubModulePatch
    {
        // Prevent any Integrated MCMv3 from loading
        private class EmptyIntegratedLoader : MCMv3IIntegratedLoader
        {
            public List<(MBSubModuleBase, System.Type)> MCMImplementationSubModules { get; } = new List<(MBSubModuleBase, System.Type)>();

            public void Load() { }
        }

        private static readonly AccessTools.FieldRef<MCMv3IntegratedLoaderSubModule, MCMv3IIntegratedLoader> _loader =
            AccessTools.FieldRefAccess<MCMv3IntegratedLoaderSubModule, MCMv3IIntegratedLoader>("_loader");

        public static void Patch(Harmony harmony)
        {
            harmony.Patch(AccessTools.Constructor(typeof(MCMv3IntegratedLoaderSubModule)),
                prefix: new HarmonyMethod(SymbolExtensions.GetMethodInfo(() => StopIntegratedLoaderSubModuleCtor(null!))));
        }

        // Prevent any Integrated MCMv3 from loading
        private static bool StopIntegratedLoaderSubModuleCtor(MCMv3IntegratedLoaderSubModule __instance)
        {
            _loader(__instance) = new EmptyIntegratedLoader();

            return false;
        }
    }
}