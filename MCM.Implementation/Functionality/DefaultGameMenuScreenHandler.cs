using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Functionality;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
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
    internal sealed class DefaultGameMenuScreenHandler : IGameMenuScreenHandler
    {
        private static InitialMenuVM? _instance;
        private static Dictionary<string, (int, Func<ScreenBase>, TextObject)> Screens { get; } = new Dictionary<string, (int, Func<ScreenBase>, TextObject)>();
        private static int _initialized;

        public DefaultGameMenuScreenHandler()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                var harmony = new Harmony("bannerlord.mcm.mainmenuscreeninjection_v3");
                harmony.Patch(
                    original: AccessTools.Constructor(typeof(InitialMenuVM), Type.EmptyTypes),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultGameMenuScreenHandler), nameof(Constructor)), 300));
            }
        }

        private static void Constructor(InitialMenuVM __instance, ref MBBindingList<InitialMenuOptionVM> ____menuOptions)
        {
            _instance = __instance;
            foreach (var pair in Screens)
            {
                var (Index, ScreenFactory, Text) = pair.Value;

                var insertIndex = ____menuOptions.FindIndex(i => i.InitialStateOption.OrderIndex > Index);
                ____menuOptions.Insert(insertIndex, new InitialMenuOptionVM(new InitialStateOption(
                    pair.Key,
                    Text,
                    9000,
                    () => ScreenManager.PushScreen(ScreenFactory()),
                    false)));
            }
        }

        public void AddScreen(string internalName, int index, Func<ScreenBase> screenFactory, TextObject text)
        {
            if (_instance == null)
            {
                if (Screens.ContainsKey(internalName))
                    Screens[internalName] = (index, screenFactory, text);
                else
                    Screens.Add(internalName, (index, screenFactory, text));
            }
            else
            {
                var insertIndex = _instance.MenuOptions.FindIndex(i => i.InitialStateOption.OrderIndex > index);
                _instance.MenuOptions.Insert(insertIndex, new InitialMenuOptionVM(new InitialStateOption(
                    internalName,
                    text,
                    index,
                    () => ScreenManager.PushScreen(screenFactory()),
                    false)));
            }
        }
        public void RemoveScreen(string internalName)
        {
            if (_instance == null)
            {
                if (Screens.ContainsKey(internalName))
                    Screens.Remove(internalName);
            }
            else
            {
                var found = _instance.MenuOptions.FirstOrDefault(i => i.InitialStateOption.Id == internalName);
                if (found != null)
                    _instance.MenuOptions.Remove(found);
            }
        }
    }
}