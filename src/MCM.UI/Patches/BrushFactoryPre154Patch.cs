using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using MCM.UI.Functionality.Injectors;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using TaleWorlds.GauntletUI;

namespace MCM.UI.Patches
{
    internal static class BrushFactoryPre154Patch
    {
        private static readonly AccessTools.FieldRef<BrushFactory, Dictionary<string, Brush>>? Brushes =
            AccessTools2.FieldRefAccess<BrushFactory, Dictionary<string, Brush>>("_brushes");

        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.Method(typeof(BrushFactory), "LoadBrushes"),
                postfix: new HarmonyMethod(AccessTools.Method(typeof(BrushFactoryPre154Patch), nameof(LoadBrushesHarmony))));
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void LoadBrushesHarmony(BrushFactory __instance)
        {
            if (Brushes is not null)
            {
                var brushes = Brushes(__instance);
                foreach (var brush in BrushInjector.Brushes.Keys)
                    brushes[brush.Name] = brush;
            }
        }
    }
}