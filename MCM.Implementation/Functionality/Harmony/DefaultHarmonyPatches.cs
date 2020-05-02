using HarmonyLib;

using MCM.Abstractions.Attributes;

using SandBox.View.Map;

using System;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MCM.Implementation.Functionality.Harmony
{
    [Version("e1.0.0",  220)]
    [Version("e1.0.1",  220)]
    [Version("e1.0.2",  220)]
    [Version("e1.0.3",  220)]
    [Version("e1.0.4",  220)]
    [Version("e1.0.5",  220)]
    [Version("e1.0.6",  220)]
    [Version("e1.0.7",  220)]
    [Version("e1.0.8",  220)]
    [Version("e1.0.9",  220)]
    [Version("e1.0.10", 220)]
    [Version("e1.0.11", 220)]
    [Version("e1.1.0",  220)]
    [Version("e1.2.0",  220)]
    [Version("e1.2.1",  220)]
    [Version("e1.3.0",  220)]
    internal sealed class DefaultHarmonyPatches : BaseHarmonyPatches
    {
        private static readonly AccessTools.FieldRef<EscapeMenuItemVM, TextObject> _itemObj =
            AccessTools.FieldRefAccess<EscapeMenuItemVM, TextObject>("_itemObj");

        private static MethodInfo OnEscapeMenuToggledMethod { get; } = AccessTools.Method(typeof(MapScreen), "OnEscapeMenuToggled");
        public static void GetEscapeMenuItemsHarmony(MapScreen __instance, List<EscapeMenuItemVM> __result)
        {
            foreach (var (Index, ScreenFactory, Text) in Screens)
            {
                __result.Insert(Index, new EscapeMenuItemVM(
                    Text,
                    _ =>
                    {
                        // So the game will be paused
                        //OnEscapeMenuToggledMethod.Invoke(__instance, new object[] { false });
                        ScreenManager.PushScreen(ScreenFactory());
                    },
                    null, false, false));
            }
        }

        private static List<(int, Func<ScreenBase>, TextObject)> Screens { get; } = new List<(int, Func<ScreenBase>, TextObject)>();

        public override HarmonyMethod GetEscapeMenuItems =>
            new HarmonyMethod(AccessTools.Method(typeof(DefaultHarmonyPatches), nameof(GetEscapeMenuItemsHarmony)));

        public override void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text)
        {
            //if (!Screens.Any(s => s.Item3 == text))
                Screens.Add((index, screenFactory, text));
        }
    }
}