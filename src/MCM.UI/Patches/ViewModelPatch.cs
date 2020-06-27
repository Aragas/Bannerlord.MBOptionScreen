using HarmonyLib;

using MCM.Abstractions;

using System.Reflection;

using TaleWorlds.Library;

namespace MCM.UI.Patches
{
    public static class ViewModelPatch
    {
        public static MethodBase ExecuteCommandMethod { get; } =
            AccessTools.Method(typeof(ViewModel), nameof(ViewModel.ExecuteCommand));

        /// <summary>
        /// Trigger ExecuteCommand in the wrapped VM
        /// We can't extend\copy methods like we do with properties
        /// </summary>
        public static bool ExecuteCommandPatch(ViewModel __instance, string commandName, object[] parameters)
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