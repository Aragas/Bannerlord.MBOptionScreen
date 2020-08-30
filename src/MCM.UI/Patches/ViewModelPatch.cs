using HarmonyLib;

using MCM.Abstractions;

using TaleWorlds.Library;

namespace MCM.UI.Patches
{
    internal static class ViewModelPatch
    {
        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.Method(typeof(ViewModel), nameof(ViewModel.ExecuteCommand)),
                prefix: new HarmonyMethod(typeof(ViewModelPatch), nameof(ExecuteCommandPatch)));
        }

        /// <summary>
        /// Trigger ExecuteCommand in the wrapped VM
        /// We can't extend\copy methods like we do with properties
        /// </summary>
        private static bool ExecuteCommandPatch(ViewModel __instance, string commandName, object[] parameters)
        {
            if (__instance is IWrapper wrapper && wrapper.Object is ViewModel viewModel)
            {
                viewModel.ExecuteCommand(commandName, parameters);
                return false;
            }

            return true;
        }
    }
}