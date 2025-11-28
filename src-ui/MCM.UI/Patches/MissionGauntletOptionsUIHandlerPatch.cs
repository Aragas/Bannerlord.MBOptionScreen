using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;
using TaleWorlds.TwoDimension;

namespace MCM.UI.Patches
{
    internal static class MissionGauntletOptionsUIHandlerPatch
    {
        private static readonly ConditionalWeakTable<object, SpriteCategory?> _spriteCategoriesMCM = new();

        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools2.Constructor(typeof(MissionGauntletOptionsUIHandler)),
                postfix: new HarmonyMethod(typeof(MissionGauntletOptionsUIHandlerPatch), nameof(OnInitializePostfix)));

            harmony.Patch(
                AccessTools2.Method(typeof(MissionGauntletOptionsUIHandler), "OnMissionScreenFinalize"),
                postfix: new HarmonyMethod(typeof(MissionGauntletOptionsUIHandlerPatch), nameof(OnFinalizePostfix)));
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnInitializePostfix(object __instance)
        {
            var spriteCategoryMCM = UIResourceManager.SpriteData.SpriteCategories.TryGetValue("ui_mcm", out var spriteCategoryMCMVal)
                ? spriteCategoryMCMVal
                : null;
#if v134 || v135 || v136
            spriteCategoryMCM?.Load(UIResourceManager.ResourceContext, UIResourceManager.ResourceDepot);
#elif v100 || v101 || v102 || v103 || v110 || v111 || v112 || v113 || v114 || v115 || v116 || v120 || v121 || v122 || v123 || v124 || v125 || v126 || v127 || v128 || v129 || v1210 || v1211 || v1212
            spriteCategoryMCM?.Load(UIResourceManager.ResourceContext, UIResourceManager.UIResourceDepot);
#else
#error DEFINE
#endif
            _spriteCategoriesMCM.Add(__instance, spriteCategoryMCM);
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnFinalizePostfix(object __instance)
        {
            _spriteCategoriesMCM.GetValue(__instance, _ => null)?.Unload();
            _spriteCategoriesMCM.Remove(__instance);
        }
    }
}