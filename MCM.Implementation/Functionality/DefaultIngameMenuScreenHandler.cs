using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Functionality;

using System;
using System.Collections.Generic;
using System.Threading;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MCM.Implementation.Functionality
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    internal sealed class DefaultIngameMenuScreenHandler : IIngameMenuScreenHandler
    {
        private static Dictionary<string, (int, Func<ScreenBase>, TextObject)> Screens { get; } = new Dictionary<string, (int, Func<ScreenBase>, TextObject)>();
        private static int _initialized;

        public DefaultIngameMenuScreenHandler()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                new HarmonyLib.Harmony("bannerlord.mcm.mapscreeninjection_v3").Patch(
                    original: AccessTools.Constructor(typeof(EscapeMenuVM), new Type[] { typeof(IEnumerable<EscapeMenuItemVM>), typeof(TextObject) }),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultIngameMenuScreenHandler), nameof(SetEscapeMenuItems)), 300));
            }
        }

        private static void SetEscapeMenuItems(ref MBBindingList<EscapeMenuItemVM> ____menuItems)
        {
            foreach (var (Index, ScreenFactory, Text) in Screens.Values)
            {
                ____menuItems.Insert(Index, new EscapeMenuItemVM(
                    Text,
                    _ => ScreenManager.PushScreen(ScreenFactory()),
                    null, false, false));
            }
        }

        public void AddScreen(string internalName, int index, Func<ScreenBase> screenFactory, TextObject text)
        {
            if (Screens.ContainsKey(internalName))
                Screens[internalName] = (index, screenFactory, text);
            else
                Screens.Add(internalName, (index, screenFactory, text));
        }
        public void RemoveScreen(string internalName)
        {
            if (Screens.ContainsKey(internalName))
                Screens.Remove(internalName);
        }
    }
}