using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

using MCM.Abstractions.Synchronization;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;
using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    public static class WidgetFactoryPatch
    {
        private static FieldInfo _builtinTypesField { get; } = AccessTools.Field(typeof(WidgetFactory), "_builtinTypes");
        private static MethodInfo GetPrefabNamesAndPathsFromCurrentPathMethod { get; } =
            AccessTools.Method(typeof(WidgetFactory), "GetPrefabNamesAndPathsFromCurrentPath");

        public static MethodBase TargetMethod() =>
            AccessTools.Method(typeof(WidgetFactory), "Initialize");

        public static bool InitializePrefix(WidgetFactory __instance)
        {
            foreach (var prefabExtension in __instance.PrefabExtensionContext.PrefabExtensions)
            {
                var method = AccessTools.Method(prefabExtension.GetType(), "RegisterAttributeTypes");
                method.Invoke(prefabExtension, new object[] { __instance.WidgetAttributeContext  });
            }
            foreach (var type in WidgetInfo.CollectWidgetTypes())
            {
                var dict = _builtinTypesField.GetValue(__instance) as Dictionary<string, Type>;
                if (!dict.ContainsKey(type.Name))
                    dict.Add(type.Name, type);
            }

            var dict2 = GetPrefabNamesAndPathsFromCurrentPathMethod.Invoke(__instance, Array.Empty<object>()) as Dictionary<string, string>;
            foreach (var keyValuePair in dict2)
            {
                __instance.AddCustomType(keyValuePair.Key, keyValuePair.Value);
            }

            return false;
        }
    }

    public sealed class SubModuleV300 : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnSubModuleLoad_UIv3");
            if (synchronizationProvider.IsFirstInitialization)
            {
                var harmony = new Harmony("bannerlord.mcm.ui.loading_v3");
                harmony.Patch(
                    original: WidgetFactoryPatch.TargetMethod(),
                    prefix: new HarmonyMethod(typeof(WidgetFactoryPatch), nameof(WidgetFactoryPatch.InitializePrefix)));
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnGauntletUISubModuleSubModuleLoadTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnGauntletUISubModuleSubModuleLoadPostfix)));
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnSubModuleUnloadedTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnSubModuleUnloadedPostfix)));
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootPostfix)));
            }
        }
    }
}