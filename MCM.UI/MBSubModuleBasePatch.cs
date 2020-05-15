using HarmonyLib;

using SandBox;

using System.Reflection;

using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    /// <summary>
    /// Instead of having a direct dependency of Sandbox Module, hook to OnSubModuleLoad to check when it's
    /// done with initialization
    /// </summary>
    internal class MBSubModuleBasePatch
    {
        private static bool _loaded = false;

        public static MethodBase OnSubModuleLoadTargetMethod() =>
            AccessTools.Method(typeof(MBSubModuleBase), "OnSubModuleLoad");
        public static MethodBase OnBeforeInitialModuleScreenSetAsRootTargetMethod() =>
            AccessTools.Method(typeof(MBSubModuleBase), "OnBeforeInitialModuleScreenSetAsRoot");

        public static void OnSubModuleLoadPostfix(MBSubModuleBase __instance)
        {
            if (!(__instance is SandBoxSubModule)) 
                return;

            SubModuleV300.SandBoxSubModuleOnSubModuleLoad();
            _loaded = true;
        }
        public static void OnBeforeInitialModuleScreenSetAsRootPostfix()
        {
            if (_loaded)
                return;

            SubModuleV300.SandBoxSubModuleOnSubModuleLoad();
            _loaded = true;
        }
    }
}