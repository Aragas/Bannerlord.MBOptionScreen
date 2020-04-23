using HarmonyLib;

using MBOptionScreen.Attributes;

using SandBox.View.Map;

using System;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MBOptionScreen.Functionality
{
    [Version("e1.0.0",  200)]
    [Version("e1.0.1",  200)]
    [Version("e1.0.2",  200)]
    [Version("e1.0.3",  200)]
    [Version("e1.0.4",  200)]
    [Version("e1.0.5",  200)]
    [Version("e1.0.6",  200)]
    [Version("e1.0.7",  200)]
    [Version("e1.0.8",  200)]
    [Version("e1.0.9",  200)]
    [Version("e1.0.10", 200)]
    [Version("e1.0.11", 200)]
    [Version("e1.1.0",  200)]
    [Version("e1.2.0",  200)]
    internal sealed class DefaultMapScreenPatches : BaseMapScreenPatches
    {
        private static MethodInfo OnEscapeMenuToggledMethod { get; } = AccessTools.Method(typeof(MapScreen), "OnEscapeMenuToggled");
        public static void GetEscapeMenuItemsHarmonyPostfix(MapScreen __instance, List<EscapeMenuItemVM> __result)
        {
            foreach (var (Index, ScreenFactory, Text) in Screens)
            {
                __result.Insert(Index, new EscapeMenuItemVM(
                    Text,
                    _ =>
                    {
                        OnEscapeMenuToggledMethod.Invoke(__instance, new object[] { false });
                        ScreenManager.PushScreen(ScreenFactory());
                    },
                    null, false, false));
            }
        }

        private static List<(int, Func<ScreenBase>, TextObject)> Screens { get; } = new List<(int, Func<ScreenBase>, TextObject)>();

        public override HarmonyMethod GetEscapeMenuItemsPostfix =>
            new HarmonyMethod(AccessTools.Method(typeof(DefaultMapScreenPatches), nameof(GetEscapeMenuItemsHarmonyPostfix)));

        public override void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text)
        {
            Screens.Add((index, screenFactory, text));
        }
    }
}