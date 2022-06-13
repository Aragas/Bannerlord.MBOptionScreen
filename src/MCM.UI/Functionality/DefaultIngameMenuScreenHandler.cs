using Bannerlord.BUTR.Shared.Extensions;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Common.Wrappers;
using MCM.Utils;

using SandBox.View.Map;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;

namespace MCM.UI.Functionality
{
    internal sealed class DefaultIngameMenuScreenHandler : BaseIngameMenuScreenHandler
    {
        private delegate void OnEscapeMenuToggledGauntletMissionEscapeMenuBaseDelegate(object instance, bool isOpened = false);

        private static readonly OnEscapeMenuToggledGauntletMissionEscapeMenuBaseDelegate? OnEscapeMenuToggledGauntletMissionEscapeMenuBase =
            AccessTools2.GetDelegate<OnEscapeMenuToggledGauntletMissionEscapeMenuBaseDelegate>("TaleWorlds.MountAndBlade.GauntletUI.GauntletMissionEscapeMenuBase:OnEscapeMenuToggled") ??
            AccessTools2.GetDelegate<OnEscapeMenuToggledGauntletMissionEscapeMenuBaseDelegate>("TaleWorlds.MountAndBlade.GauntletUI.Mission.MissionGauntletEscapeMenuBase:OnEscapeMenuToggled");

        private static readonly AccessTools.FieldRef<object, object>? DataSource =
            AccessTools2.FieldRefAccess<object>("TaleWorlds.MountAndBlade.GauntletUI.GauntletMissionEscapeMenuBase:_dataSource") ??
            AccessTools2.FieldRefAccess<object>("TaleWorlds.MountAndBlade.GauntletUI.GauntletMissionEscapeMenuBase:DataSource") ??
            AccessTools2.FieldRefAccess<object>("TaleWorlds.MountAndBlade.GauntletUI.Mission.MissionGauntletEscapeMenuBase:DataSource");

        private static readonly AccessTools.FieldRef<Dictionary<Type, Type>?>? ActualViewTypes =
            AccessTools2.StaticFieldRefAccess<Dictionary<Type, Type>>("TaleWorlds.MountAndBlade.View.ViewCreatorManager:_actualViewTypes") ??
            AccessTools2.StaticFieldRefAccess<Dictionary<Type, Type>>("TaleWorlds.MountAndBlade.View.Missions.ViewCreatorManager:_actualViewTypes");
        
        private static readonly Type? MissionSingleplayerEscapeMenuType =
            AccessTools2.TypeByName("TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer.MissionSingleplayerEscapeMenu") ??
            AccessTools2.TypeByName("TaleWorlds.MountAndBlade.LegacyGUI.Missions.Singleplayer.MissionSingleplayerEscapeMenu");

        private static readonly AccessTools.FieldRef<object, object>? Identifier =
            AccessTools2.FieldRefAccess<object>("TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenuItemVM:_identifier") ??
            AccessTools2.FieldRefAccess<object>("TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu.EscapeMenuItemVM:_identifier");

        private static readonly WeakReference<object> _instance = new(null!);
        private static Dictionary<string, (int, Func<ScreenBase?>, TextObject)> ScreensCache { get; } = new();

        public DefaultIngameMenuScreenHandler()
        {
            var harmony = new Harmony("bannerlord.mcm.escapemenuinjection_v3");

            harmony.Patch(
                AccessTools2.Method("SandBox.View.Map.MapScreen:GetEscapeMenuItems"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultIngameMenuScreenHandler), nameof(MapScreen_GetEscapeMenuItems)), 300));

            // TODO: We can't replace MissionSingleplayerEscapeMenu at runtime because it's injected in the MissionView[]
            // TODO: Won't work if the type is replaced at runtime
            var overrideType = (ActualViewTypes is not null ? ActualViewTypes() ?? new() : new())[MissionSingleplayerEscapeMenuType];
            harmony.Patch(
                AccessTools2.DeclaredMethod(overrideType, "GetEscapeMenuItems"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultIngameMenuScreenHandler), nameof(MissionSingleplayerEscapeMenu_GetEscapeMenuItems)), 300));
            harmony.Patch(
                AccessTools2.DeclaredMethod(overrideType, "OnMissionScreenInitialize"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultIngameMenuScreenHandler), nameof(OnMissionScreenInitialize)), 300));
            harmony.Patch(
                AccessTools2.DeclaredMethod(overrideType, "OnMissionScreenFinalize") ??
                AccessTools2.DeclaredMethod("TaleWorlds.MountAndBlade.GauntletUI.GauntletMissionEscapeMenuBase:OnMissionScreenFinalize") ??
                AccessTools2.DeclaredMethod("TaleWorlds.MountAndBlade.GauntletUI.Mission.MissionGauntletEscapeMenuBase:OnMissionScreenFinalize"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(DefaultIngameMenuScreenHandler), nameof(OnMissionScreenFinalize)), 300));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void MapScreen_GetEscapeMenuItems(MapScreen __instance, ref IList __result)
        {
            foreach (var (key, value) in ScreensCache)
            {
                var (index, screenFactory, text) = value;
                var escapeMenuItemVM = EscapeMenuItemVMUtils.Create(text,
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
        private static void MissionSingleplayerEscapeMenu_GetEscapeMenuItems(object __instance, ref IList __result)
        {
            foreach (var (key, value) in ScreensCache)
            {
                var (index, screenFactory, text) = value;
                var escapeMenuItemVM = EscapeMenuItemVMUtils.Create(text,
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
        private static void OnMissionScreenInitialize(object __instance)
        {
            if (__instance.GetType().FullName is "TaleWorlds.MountAndBlade.GauntletUI.GauntletMissionEscapeMenuBase" or "TaleWorlds.MountAndBlade.GauntletUI.Mission.MissionGauntletEscapeMenuBase")
            {
                _instance.SetTarget(__instance);
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OnMissionScreenFinalize(object __instance)
        {
            if (__instance.GetType().FullName is "TaleWorlds.MountAndBlade.GauntletUI.GauntletMissionEscapeMenuBase" or "TaleWorlds.MountAndBlade.GauntletUI.Mission.MissionGauntletEscapeMenuBase")
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
                var escapeMenuItemVM = EscapeMenuItemVMUtils.Create(text,
                    _ => ScreenManager.PushScreen(screenFactory()),
                    internalName, () => new Tuple<bool, TextObject>(false, new TextObject(string.Empty)));
                new MenuItemsWrapper(dataSource).MenuItems.Insert(index, escapeMenuItemVM);
            }

            ScreensCache[internalName] = (index, screenFactory, text);
        }
        public override void RemoveScreen(string internalName)
        {
            if (_instance.TryGetTarget(out var instance) && DataSource is not null && Identifier is not null)
            {
                var dataSource = DataSource(instance);
                var found = new MenuItemsWrapper(dataSource).MenuItems.Cast<object>().FirstOrDefault(i => Identifier(i) is string text && text == internalName);
                if (found is not null)
                    new MenuItemsWrapper(dataSource).MenuItems.Remove(found);
            }

            if (ScreensCache.ContainsKey(internalName))
                ScreensCache.Remove(internalName);
        }
    }
}