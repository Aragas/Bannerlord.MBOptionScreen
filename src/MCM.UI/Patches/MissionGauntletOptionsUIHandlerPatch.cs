using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.TwoDimension;

namespace MCM.UI.Patches
{
    internal static class MissionGauntletOptionsUIHandlerPatch
    {
        private static readonly ConditionalWeakTable<MissionGauntletOptionsUIHandler, SpriteCategory> _spriteCategorySaveLoads = new();

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
        private static void OnInitializePostfix(MissionGauntletOptionsUIHandler __instance)
        {
            if (ApplicationVersionHelper.GameVersion() is { } gameVersion)
            {
                if ((gameVersion.Major >= 1 && gameVersion.Minor >= 6 && gameVersion.Revision >= 1) || (gameVersion.Major >= 1 && gameVersion.Minor >= 7))
                {
                    var spriteCategorySaveLoad = UIResourceManager.SpriteData.SpriteCategories.TryGetValue("ui_saveload", out var spriteCategorySaveLoadVal)
                            ? spriteCategorySaveLoadVal
                            : null;
                    spriteCategorySaveLoad?.Load(UIResourceManager.ResourceContext, UIResourceManager.UIResourceDepot);
                    _spriteCategorySaveLoads.Add(__instance, spriteCategorySaveLoad);
                }
            }
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnFinalizePostfix(MissionGauntletOptionsUIHandler __instance)
        {
            if (ApplicationVersionHelper.GameVersion() is { } gameVersion)
            {
                if ((gameVersion.Major >= 1 && gameVersion.Minor >= 6 && gameVersion.Revision >= 1) || (gameVersion.Major >= 1 && gameVersion.Minor >= 7))
                {
                    var spriteCategorySaveLoad = UIResourceManager.SpriteData.SpriteCategories.TryGetValue("ui_saveload", out var spriteCategorySaveLoadVal)
                        ? spriteCategorySaveLoadVal
                        : null;
                    spriteCategorySaveLoad?.Unload();
                    _spriteCategorySaveLoads.Remove(__instance);
                }
            }
        }
    }
}