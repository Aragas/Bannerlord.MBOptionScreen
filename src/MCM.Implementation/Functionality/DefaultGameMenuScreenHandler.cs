using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using MCM.Abstractions.Functionality;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MCM.Implementation.Functionality
{
    internal sealed class DefaultGameMenuScreenHandler : BaseGameMenuScreenHandler
    {
        private static readonly WeakReference<InitialMenuVM> _instance = new WeakReference<InitialMenuVM>(null!);
        private static Dictionary<string, (int, Func<ScreenBase?>, TextObject)> ScreensCache { get; } = new Dictionary<string, (int, Func<ScreenBase?>, TextObject)>();

        public DefaultGameMenuScreenHandler()
        {
            var harmony = new Harmony("bannerlord.mcm.mainmenuscreeninjection_v4");
            harmony.Patch(
                AccessTools.Constructor(typeof(InitialMenuVM), Type.EmptyTypes),
                postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultGameMenuScreenHandler), nameof(Constructor)), 300));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
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

            ScreensCache[internalName] = (index, screenFactory, text);
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