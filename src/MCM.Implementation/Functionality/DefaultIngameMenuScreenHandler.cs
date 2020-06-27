using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Functionality;

using SandBox.View.Map;

using System;
using System.Collections.Generic;
using System.Threading;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions.Singleplayer;
using TaleWorlds.MountAndBlade.View.Missions;
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
    internal sealed class DefaultIngameMenuScreenHandler : BaseIngameMenuScreenHandler
    {
        private static readonly WeakReference<EscapeMenuVM> _instance = new WeakReference<EscapeMenuVM>(null!);
        private static Dictionary<string, (int, Func<ScreenBase?>, TextObject)> ScreensCache { get; } = new Dictionary<string, (int, Func<ScreenBase?>, TextObject)>();
        private static int _initialized;

        public DefaultIngameMenuScreenHandler()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                var harmony = new Harmony("bannerlord.mcm.escapemenuinjection_v3");

                harmony.Patch(
                    AccessTools.Method(typeof(MapScreen), "GetEscapeMenuItems"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultIngameMenuScreenHandler), nameof(MapScreen_GetEscapeMenuItems)), 300));

                // TODO: We can't replace MissionSingleplayerEscapeMenu at runtime because it's injected in the MissionView[] 
                // TODO: Won't work if the type is replaced at runtime
                var actualViewTypes = (Dictionary<Type, Type>) (AccessTools.Field(typeof(ViewCreatorManager), "_actualViewTypes")?.GetValue(null) ?? new Dictionary<Type, Type>());
                var overrideType = actualViewTypes[typeof(MissionSingleplayerEscapeMenu)];
                harmony.Patch(
                    AccessTools.DeclaredMethod(overrideType, "GetEscapeMenuItems"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultIngameMenuScreenHandler), nameof(MissionSingleplayerEscapeMenu_GetEscapeMenuItems)), 300));
                harmony.Patch(
                    AccessTools.DeclaredMethod(overrideType, "OnMissionScreenInitialize"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultIngameMenuScreenHandler), nameof(OnMissionScreenInitialize)), 300));
                harmony.Patch(
                    AccessTools.DeclaredMethod(overrideType, "OnMissionScreenFinalize") ?? AccessTools.DeclaredMethod(typeof(GauntletMissionEscapeMenuBase), "OnMissionScreenFinalize"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultIngameMenuScreenHandler), nameof(OnMissionScreenFinalize)), 300));
            }
        }

        private static void MapScreen_GetEscapeMenuItems(MapScreen __instance, ref List<EscapeMenuItemVM> __result)
        {
            foreach (var pair in ScreensCache)
            {
                var (index, screenFactory, text) = pair.Value;
                __result.Insert(index, new EscapeMenuItemVM(
                    text,
                    _ =>
                    {
                        var screen = screenFactory();
                        if (screen != null)
                        {
                            AccessTools.Method(typeof(MapScreen), "OnEscapeMenuToggled")?.Invoke(__instance, new object[] {false});
                            ScreenManager.PushScreen(screen);
                        }
                    },
                    pair.Key, false, false));
            }
        }
        private static void MissionSingleplayerEscapeMenu_GetEscapeMenuItems(GauntletMissionEscapeMenuBase __instance, ref List<EscapeMenuItemVM> __result)
        {
            foreach (var pair in ScreensCache)
            {
                var (index, screenFactory, text) = pair.Value;
                __result.Insert(index, new EscapeMenuItemVM(
                    text,
                    _ =>
                    {
                        var screen = screenFactory();
                        if (screen != null)
                        {
                            AccessTools.Method(typeof(GauntletMissionEscapeMenuBase), "OnEscapeMenuToggled")?.Invoke(__instance, new object[] {false});
                            ScreenManager.PushScreen(screen);
                        }
                    },
                    pair.Key, false, false));
            }
        }
        private static void OnMissionScreenInitialize(MissionView __instance)
        {
            if (__instance is GauntletMissionEscapeMenuBase)
            {
                var dataSource = AccessTools.Field(typeof(GauntletMissionEscapeMenuBase), "_dataSource")?.GetValue(__instance) as EscapeMenuVM;
                _instance.SetTarget(dataSource!);
            }
        }
        private static void OnMissionScreenFinalize(MissionView __instance)
        {
            if (__instance is GauntletMissionEscapeMenuBase)
            {
                _instance.SetTarget(null!);
            }
        }

        public override void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject text)
        {
            if (_instance.TryGetTarget(out var instance))
            {
                // Do not add the Mod Options menu entry because the MissionView will not change
                /*
                instance.MenuItems.Insert(index, new EscapeMenuItemVM(
                    text,
                    _ => ScreenManager.PushScreen(screenFactory()),
                    internalName, false, false));
                */
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
                // Do not remove the screen if it's the mission options because the MissionView will not change
                /*
                var found = instance.MenuItems.FirstOrDefault(i => _identifierField.GetValue(i) is string text && text == internalName);
                if (found != null)
                    instance.MenuItems.Remove(found);
                */
            }

            if (ScreensCache.ContainsKey(internalName))
                ScreensCache.Remove(internalName);
        }
    }
}