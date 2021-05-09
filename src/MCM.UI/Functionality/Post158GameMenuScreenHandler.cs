using Bannerlord.BUTR.Shared.Extensions;

using BUTR.DependencyInjection.Logger;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.UI.Utils;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MCM.UI.Functionality
{
    internal sealed class Post158GameMenuScreenHandler : BaseGameMenuScreenHandler
    {
        private static readonly WeakReference<InitialMenuVM> _instance = new(null!);
        private static Dictionary<string, (int, Func<ScreenBase?>, TextObject)> ScreensCache { get; } = new();

        private readonly IBUTRLogger _logger;

        public Post158GameMenuScreenHandler(IBUTRLogger<Post158GameMenuScreenHandler> logger)
        {
            _logger = logger;

            var harmony = new Harmony("bannerlord.mcm.mainmenuscreeninjection_v4");
            harmony.Patch(
                AccessTools2.Method(typeof(InitialMenuVM), "RefreshMenuOptions"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(Post158GameMenuScreenHandler), nameof(RefreshMenuOptionsPostfix)), 300));
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RefreshMenuOptionsPostfix(InitialMenuVM __instance, ref MBBindingList<InitialMenuOptionVM> ____menuOptions)
        {
            _instance.SetTarget(__instance);
            foreach (var (key, value) in ScreensCache)
            {
                var (index, screenFactory, text) = value;

                var initialState = InitialStateOptionUtils.Create(key,
                    text,
                    9000,
                    () =>
                    {
                        var screen = screenFactory();
                        if (screen is not null)
                            ScreenManager.PushScreen(screen);
                    },
                    () => false);
                if (initialState is null)
                {
                    return;
                }
                var insertIndex = ____menuOptions.FindIndex(i => i.InitialStateOption.OrderIndex > index);
                ____menuOptions?.Insert(insertIndex, new InitialMenuOptionVM(initialState));
            }
        }

        public override void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject? text)
        {
            if (text is null) return;

            if (_instance.TryGetTarget(out var instance))
            {
                var initialState = InitialStateOptionUtils.Create(internalName,
                    text,
                    index,
                    () =>
                    {
                        var screen = screenFactory();
                        if (screen is not null)
                            ScreenManager.PushScreen(screen);
                    },
                    () => false);
                if (initialState is null)
                {
                    _logger.LogError("AddScreen: 'initialState' was null! Something was changed again by the game!");
                    return;
                }
                var insertIndex = instance.MenuOptions.FindIndex(i => i.InitialStateOption.OrderIndex > index);
                instance.MenuOptions.Insert(insertIndex, new InitialMenuOptionVM(initialState));
            }

            ScreensCache[internalName] = (index, screenFactory, text);
        }
        public override void RemoveScreen(string internalName)
        {
            if (_instance.TryGetTarget(out var instance))
            {
                var found = instance.MenuOptions.FirstOrDefault(i => i.InitialStateOption.Id == internalName);
                if (found is not null)
                    instance.MenuOptions.Remove(found);
            }

            if (ScreensCache.ContainsKey(internalName))
                ScreensCache.Remove(internalName);
        }
    }
}