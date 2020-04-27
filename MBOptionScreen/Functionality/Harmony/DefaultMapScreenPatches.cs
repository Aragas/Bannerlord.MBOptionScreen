using HarmonyLib;

using MBOptionScreen.Attributes;

using SandBox.View.Map;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MBOptionScreen.Functionality
{
    [Version("e1.0.0",  202)]
    [Version("e1.0.1",  202)]
    [Version("e1.0.2",  202)]
    [Version("e1.0.3",  202)]
    [Version("e1.0.4",  202)]
    [Version("e1.0.5",  202)]
    [Version("e1.0.6",  202)]
    [Version("e1.0.7",  202)]
    [Version("e1.0.8",  202)]
    [Version("e1.0.9",  202)]
    [Version("e1.0.10", 202)]
    [Version("e1.0.11", 202)]
    [Version("e1.1.0",  202)]
    [Version("e1.2.0",  202)]
    [Version("e1.2.1",  202)]
    [Version("e1.3.0",  202)]
    internal sealed class DefaultMapScreenPatches : BaseMapScreenPatches
    {
        private static readonly AccessTools.FieldRef<EscapeMenuItemVM, TextObject> _itemObj =
            AccessTools.FieldRefAccess<EscapeMenuItemVM, TextObject>("_itemObj");

        private static MethodInfo OnEscapeMenuToggledMethod { get; } = AccessTools.Method(typeof(MapScreen), "OnEscapeMenuToggled");
        public static void GetEscapeMenuItemsHarmonyPostfix(MapScreen __instance, List<EscapeMenuItemVM> __result)
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

        public override HarmonyMethod GetEscapeMenuItemsPostfix =>
            new HarmonyMethod(AccessTools.Method(typeof(DefaultMapScreenPatches), nameof(GetEscapeMenuItemsHarmonyPostfix)));

        public override void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text)
        {
            //if (!Screens.Any(s => s.Item3 == text))
                Screens.Add((index, screenFactory, text));
        }
    }
}