using Bannerlord.BUTR.Shared.Extensions;
using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.UI.Utils;

using SandBox.View.Map;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions.Singleplayer;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace MCM.UI.Functionality
{
    internal sealed class DefaultIngameMenuScreenHandler : BaseIngameMenuScreenHandler
    {
        private delegate void OnEscapeMenuToggledMapScreenDelegate(object instance, bool isOpened = false);
        private delegate void OnEscapeMenuToggledGauntletMissionEscapeMenuBaseDelegate(object instance, bool isOpened = false);

        private static readonly OnEscapeMenuToggledMapScreenDelegate? OnEscapeMenuToggledMapScreen =
            AccessTools2.GetDelegateObjectInstance<OnEscapeMenuToggledMapScreenDelegate>(typeof(MapScreen), "OnEscapeMenuToggled");
        private static readonly OnEscapeMenuToggledGauntletMissionEscapeMenuBaseDelegate? OnEscapeMenuToggledGauntletMissionEscapeMenuBase =
            AccessTools2.GetDelegateObjectInstance<OnEscapeMenuToggledGauntletMissionEscapeMenuBaseDelegate>(typeof(GauntletMissionEscapeMenuBase), "OnEscapeMenuToggled");

        private static readonly AccessTools.FieldRef<GauntletMissionEscapeMenuBase, EscapeMenuVM>? DataSource =
            AccessTools2.FieldRefAccess<GauntletMissionEscapeMenuBase, EscapeMenuVM>("_dataSource") ??
            AccessTools2.FieldRefAccess<GauntletMissionEscapeMenuBase, EscapeMenuVM>("DataSource");
        private static readonly AccessTools.FieldRef<EscapeMenuItemVM, object>? Identifier =
            AccessTools2.FieldRefAccess<EscapeMenuItemVM, object>("_identifier");

        private static readonly WeakReference<GauntletMissionEscapeMenuBase> _instance = new(null!);
        private static Dictionary<string, (int, Func<ScreenBase?>, TextObject)> ScreensCache { get; } = new();

        public DefaultIngameMenuScreenHandler()
        {
            var harmony = new Harmony("bannerlord.mcm.escapemenuinjection_v3");

            harmony.Patch(
                AccessTools2.Method(typeof(MapScreen), "GetEscapeMenuItems"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultIngameMenuScreenHandler), nameof(MapScreen_GetEscapeMenuItems)), 300));

            // TODO: We can't replace MissionSingleplayerEscapeMenu at runtime because it's injected in the MissionView[]
            // TODO: Won't work if the type is replaced at runtime
            var actualViewTypes = (Dictionary<Type, Type>) (AccessTools2.Field(typeof(ViewCreatorManager), "_actualViewTypes")?.GetValue(null) ?? new Dictionary<Type, Type>());
            var overrideType = actualViewTypes[typeof(MissionSingleplayerEscapeMenu)];
            harmony.Patch(
                AccessTools2.DeclaredMethod(overrideType, "GetEscapeMenuItems"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultIngameMenuScreenHandler), nameof(MissionSingleplayerEscapeMenu_GetEscapeMenuItems)), 300));
            harmony.Patch(
                AccessTools2.DeclaredMethod(overrideType, "OnMissionScreenInitialize"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultIngameMenuScreenHandler), nameof(OnMissionScreenInitialize)), 300));
            harmony.Patch(
                AccessTools2.DeclaredMethod(overrideType, "OnMissionScreenFinalize") ?? AccessTools2.DeclaredMethod(typeof(GauntletMissionEscapeMenuBase), "OnMissionScreenFinalize"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultIngameMenuScreenHandler), nameof(OnMissionScreenFinalize)), 300));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void MapScreen_GetEscapeMenuItems(MapScreen __instance, ref List<EscapeMenuItemVM> __result)
        {
            foreach (var (key, value) in ScreensCache)
            {
                var (index, screenFactory, text) = value;
                var escapeMenuItemVM = new EscapeMenuItemVM(text,
                    _ =>
                    {
                        var screen = screenFactory();
                        if (screen is not null)
                        {
                            OnEscapeMenuToggledGauntletMissionEscapeMenuBase?.Invoke(__instance, true);
                            ScreenManager.PushScreen(screen);
                        }
                    },
                    key, () => new Tuple<bool, TextObject>(false, new TextObject(string.Empty)));
                __result.Insert(index, escapeMenuItemVM);
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void MissionSingleplayerEscapeMenu_GetEscapeMenuItems(GauntletMissionEscapeMenuBase __instance, ref List<EscapeMenuItemVM> __result)
        {
            foreach (var (key, value) in ScreensCache)
            {
                var (index, screenFactory, text) = value;
                var escapeMenuItemVM = new EscapeMenuItemVM(text,
                    _ =>
                    {
                        var screen = screenFactory();
                        if (screen is not null)
                        {
                            OnEscapeMenuToggledGauntletMissionEscapeMenuBase?.Invoke(__instance, true);
                            ScreenManager.PushScreen(screen);
                        }
                    },
                    key, () => new Tuple<bool, TextObject>(false, new TextObject(string.Empty)));
                __result.Insert(index, escapeMenuItemVM);
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnMissionScreenInitialize(MissionView __instance)
        {
            if (__instance is GauntletMissionEscapeMenuBase gauntletMissionEscapeMenuBase)
            {
                _instance.SetTarget(gauntletMissionEscapeMenuBase);
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnMissionScreenFinalize(MissionView __instance)
        {
            if (__instance is GauntletMissionEscapeMenuBase)
            {
                _instance.SetTarget(null!);
            }
        }

        public override void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject? text)
        {
            if (text is null) return;

            if (_instance.TryGetTarget(out var instance) && DataSource is not null)
            {
                var dataSource = DataSource(instance);
                var escapeMenuItemVM = new EscapeMenuItemVM(text,
                    _ => ScreenManager.PushScreen(screenFactory()),
                    internalName, () => new Tuple<bool, TextObject>(false, new TextObject(string.Empty)));
                dataSource.MenuItems.Insert(index, escapeMenuItemVM);
            }

            ScreensCache[internalName] = (index, screenFactory, text);
        }
        public override void RemoveScreen(string internalName)
        {
            if (_instance.TryGetTarget(out var instance) && DataSource is not null && Identifier is not null)
            {
                var dataSource = DataSource(instance);
                var found = dataSource.MenuItems.FirstOrDefault(i => Identifier(i) is string text && text == internalName);
                if (found is not null)
                    dataSource.MenuItems.Remove(found);
            }

            if (ScreensCache.ContainsKey(internalName))
                ScreensCache.Remove(internalName);
        }
    }
}