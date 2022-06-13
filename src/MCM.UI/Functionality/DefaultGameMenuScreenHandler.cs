using Bannerlord.BUTR.Shared.Extensions;

using BUTR.DependencyInjection.Logger;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Common.Wrappers;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace MCM.UI.Functionality
{
    internal sealed class DefaultGameMenuScreenHandler : BaseGameMenuScreenHandler
    {
        private static readonly AccessTools.FieldRef<object, object>? InitialStateOption =
            AccessTools2.FieldRefAccess<object>("TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenuOptionVM:InitialStateOption") ??
            AccessTools2.FieldRefAccess<object>("TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu.InitialMenuOptionVM:InitialStateOption");
        
        private static readonly WeakReference<object> _instance = new(null!);
        private static Dictionary<string, (int, Func<ScreenBase?>, TextObject)> ScreensCache { get; } = new();

        private readonly IBUTRLogger _logger;

        public DefaultGameMenuScreenHandler(IBUTRLogger<DefaultGameMenuScreenHandler> logger)
        {
            _logger = logger;

            var harmony = new Harmony("bannerlord.mcm.mainmenuscreeninjection_v4");
            harmony.Patch(
                AccessTools2.Method("TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenuVM:RefreshMenuOptions") ??
                AccessTools2.Method("TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu.InitialMenuVM:RefreshMenuOptions"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultGameMenuScreenHandler), nameof(RefreshMenuOptionsPostfix)), 300));
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RefreshMenuOptionsPostfix(object __instance, ref IMBBindingList? ____menuOptions)
        {
            if (____menuOptions is null || InitialStateOption is null) return;
            
            _instance.SetTarget(__instance);
            foreach (var (key, value) in ScreensCache)
            {
                var (index, screenFactory, text) = value;

                var initialState = new InitialStateOption(key,
                    text,
                    9000,
                    () =>
                    {
                        var screen = screenFactory();
                        if (screen is not null)
                            ScreenManager.PushScreen(screen);
                    },
                    () => (false, null));
                var menuOptionsCopy = ____menuOptions.Cast<object>().ToArray();
                var insertIndex = menuOptionsCopy.FindIndex(i => new OrderIndexWrapper(InitialStateOption(i)).OrderIndex > index);
                ____menuOptions.Insert(insertIndex, InitialMenuOptionVMUtils.Create(initialState));
            }
        }

        public override void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject? text)
        {
            if (text is null) return;

            if (_instance.TryGetTarget(out var instance) && InitialStateOption is not null)
            {
                var initialState = new InitialStateOption(internalName,
                    text,
                    index,
                    () =>
                    {
                        var screen = screenFactory();
                        if (screen is not null)
                            ScreenManager.PushScreen(screen);
                    },
                    () => (false, null));
                var menuOptions = new MenuOptionsWrapper(instance).MenuOptions;
                var menuOptionsCopy = menuOptions?.Cast<object>().ToArray();
                var insertIndex = menuOptionsCopy.FindIndex(i => new OrderIndexWrapper(InitialStateOption(i)).OrderIndex > index);
                menuOptions?.Insert(insertIndex, InitialMenuOptionVMUtils.Create(initialState));
            }

            ScreensCache[internalName] = (index, screenFactory, text);
        }
        public override void RemoveScreen(string internalName)
        {
            if (_instance.TryGetTarget(out var instance) && InitialStateOption is not null)
            {
                var menuOptions = new MenuOptionsWrapper(instance).MenuOptions;
                var found = menuOptions?.Cast<object>().FirstOrDefault(i => new IdWrapper(InitialStateOption(i)).Id == internalName);
                if (found is not null)
                    menuOptions?.Remove(found);
            }

            if (ScreensCache.ContainsKey(internalName))
                ScreensCache.Remove(internalName);
        }
    }
}