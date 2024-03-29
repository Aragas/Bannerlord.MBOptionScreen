﻿using HarmonyLib;
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
            spriteCategoryMCM?.Load(UIResourceManager.ResourceContext, UIResourceManager.UIResourceDepot);
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