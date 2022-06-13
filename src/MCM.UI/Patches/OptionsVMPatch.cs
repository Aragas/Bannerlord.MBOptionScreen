using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MCM.UI.Patches
{
    internal static class OptionsVMPatch
    {
        public static bool BlockSwitch { get; set; }

        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools2.Method("TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.OptionsVM:SetSelectedCategory"),
                prefix: new HarmonyMethod(typeof(OptionsVMPatch), nameof(SetSelectedCategoryPatch)));
        }

        /// <summary>
        /// Block switching between tabs when searching
        /// </summary>
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool SetSelectedCategoryPatch() => !BlockSwitch;
    }
}