using HarmonyLib;

using MBOptionScreen.Utils;

using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MBOptionScreen.Functionality
{
    public abstract class BaseMapScreenPatches
    {
        private static BaseMapScreenPatches? _instance;
        public static BaseMapScreenPatches Instance =>
            _instance ??= DI.GetImplementation<BaseMapScreenPatches, MapScreenPatchesWrapper>(ApplicationVersionUtils.GameVersion());

        public virtual HarmonyMethod? GetEscapeMenuItemsPostfix { get; }

        public abstract void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text);
    }
}