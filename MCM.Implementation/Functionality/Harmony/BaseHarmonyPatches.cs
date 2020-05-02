using HarmonyLib;

using MCM.Utils;

using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Implementation.Functionality.Harmony
{
    public abstract class BaseHarmonyPatches
    {
        private static BaseHarmonyPatches? _instance;
        public static BaseHarmonyPatches Instance =>
            _instance ??= DI.GetImplementation<BaseHarmonyPatches, HarmonyPatchesWrapper>(ApplicationVersionUtils.GameVersion());

        public virtual HarmonyMethod? GetEscapeMenuItems { get; }

        public abstract void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text);
    }
}