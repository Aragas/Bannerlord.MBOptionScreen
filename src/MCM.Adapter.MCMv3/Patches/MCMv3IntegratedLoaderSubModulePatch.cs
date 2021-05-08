extern alias v3;
extern alias v4;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using TaleWorlds.MountAndBlade;

using MCMv3IIntegratedLoader = v3::MCM.Abstractions.Loader.IIntegratedLoader;
using MCMv3IntegratedLoaderSubModule = v3::MCM.IntegratedLoaderSubModule;

namespace MCM.Adapter.MCMv3.Patches
{
    internal static class MCMv3IntegratedLoaderSubModulePatch
    {
        // Prevent any Integrated MCMv3 from loading
        private class EmptyIntegratedLoader : MCMv3IIntegratedLoader
        {
            public List<(MBSubModuleBase, System.Type)> MCMImplementationSubModules { get; } = new();

            public void Load() { }
        }

        private static readonly AccessTools.FieldRef<MCMv3IntegratedLoaderSubModule, MCMv3IIntegratedLoader>? Loader =
            AccessTools2.FieldRefAccess<MCMv3IntegratedLoaderSubModule, MCMv3IIntegratedLoader>("_loader");

        public static void Patch(Harmony harmony)
        {
            harmony.Patch(AccessTools2.Constructor(typeof(MCMv3IntegratedLoaderSubModule)),
                prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo(() => StopIntegratedLoaderSubModuleCtor(null!))));
        }

        // Prevent any Integrated MCMv3 from loading
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool StopIntegratedLoaderSubModuleCtor(MCMv3IntegratedLoaderSubModule __instance)
        {
            if (Loader is not null)
                Loader(__instance) = new EmptyIntegratedLoader();

            return false;
        }
    }
}