using HarmonyLib;

using MCM.Abstractions;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ExecuteCommandPatch(object __instance, string commandName, object[] parameters)
        {
            if (__instance is IWrapper { Object: ViewModel viewModel })
            {
                viewModel.ExecuteCommand(commandName, parameters);
                return false;
            }

            return true;
        }
    }
}