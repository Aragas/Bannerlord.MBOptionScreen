using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Functionality;
using MCM.Implementation.Extensions;

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
    [Version("e1.3.1",  1)]
    [Version("e1.4.0",  1)]
    [Version("e1.4.1",  1)]
    internal sealed class DefaultGameMenuScreenHandler : BaseGameMenuScreenHandler
    {
        private static readonly WeakReference<InitialMenuVM> _instance = new WeakReference<InitialMenuVM>(null!);
        private static Dictionary<string, (int, Func<ScreenBase?>, TextObject)> ScreensCache { get; } = new Dictionary<string, (int, Func<ScreenBase?>, TextObject)>();
        private static int _initialized;

        public DefaultGameMenuScreenHandler()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                var harmony = new Harmony("bannerlord.mcm.mainmenuscreeninjection_v3");
                harmony.Patch(
                    AccessTools.Constructor(typeof(InitialMenuVM), Type.EmptyTypes),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultGameMenuScreenHandler), nameof(Constructor)), 300));
            }
        }

        private static void Constructor(InitialMenuVM __instance, ref MBBindingList<InitialMenuOptionVM> ____menuOptions)
        {
            _instance.SetTarget(__instance);
            foreach (var (key, value) in ScreensCache)
            {
                var (index, screenFactory, text) = value;

                var insertIndex = ____menuOptions.FindIndex(i => i.InitialStateOption.OrderIndex > index);
                ____menuOptions.Insert(insertIndex, new InitialMenuOptionVM(new InitialStateOption(
                    key,
                    text,
                    9000,
                    () =>
                    {
                        var screen = screenFactory();
                        if (screen != null)
                            ScreenManager.PushScreen(screen);
                    },
                    false)));
            }
        }

        public override void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject text)
        {
            if (_instance.TryGetTarget(out var instance))
            {
                var insertIndex = instance.MenuOptions.FindIndex(i => i.InitialStateOption.OrderIndex > index);
                instance.MenuOptions.Insert(insertIndex, new InitialMenuOptionVM(new InitialStateOption(
                    internalName,
                    text,
                    index,
                    () =>
                    {
                        var screen = screenFactory();
                        if (screen != null)
                            ScreenManager.PushScreen(screen);
                    },
                    false)));
            }

            if (ScreensCache.ContainsKey(internalName))
                ScreensCache[internalName] = (index, screenFactory, text);
            else
                ScreensCache.Add(internalName, (index, screenFactory, text));
        }
        public override void RemoveScreen(string internalName)
        {
            if (_instance.TryGetTarget(out var instance))
            {
                var found = instance.MenuOptions.FirstOrDefault(i => i.InitialStateOption.Id == internalName);
                if (found != null)
                    instance.MenuOptions.Remove(found);
            }

            if (ScreensCache.ContainsKey(internalName))
                ScreensCache.Remove(internalName);
        }
    }
}