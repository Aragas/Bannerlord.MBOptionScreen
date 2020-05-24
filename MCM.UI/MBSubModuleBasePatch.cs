using HarmonyLib;

using MCM.Abstractions.Functionality;
using MCM.Abstractions.Settings.Base;
using MCM.UI.Functionality.Loaders;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.View.Screen;

namespace MCM.UI
{
    /// <summary>
    /// Instead of having a direct dependency of Sandbox Module, hook to OnSubModuleLoad to check when it's
    /// done with initialization
    /// </summary>
    internal class MBSubModuleBasePatch
    {
        private static bool _loaded = false;

        public static MethodBase OnGauntletUISubModuleSubModuleLoadTargetMethod() =>
            AccessTools.Method(typeof(GauntletUISubModule), "OnSubModuleLoad");
        public static MethodBase OnSubModuleUnloadedTargetMethod() =>
            AccessTools.Method(typeof(MBSubModuleBase), "OnSubModuleUnloaded");
        public static MethodBase OnBeforeInitialModuleScreenSetAsRootTargetMethod() =>
            AccessTools.Method(typeof(MBSubModuleBase), "OnBeforeInitialModuleScreenSetAsRoot");

        public static void OnGauntletUISubModuleSubModuleLoadPostfix(MBSubModuleBase __instance)
        {
            if (!(__instance is GauntletUISubModule)) 
                return;

            SandBoxSubModuleOnSubModuleLoad();
            _loaded = true;
        }
        public static void OnSubModuleUnloadedPostfix(MBSubModuleBase __instance)
        {
            if (__instance is SubModuleV300)
                OnSubModuleLoad();
        }
        public static void OnBeforeInitialModuleScreenSetAsRootPostfix()
        {
            // GauntletUISubModule.OnSubModuleLoad wont hit if it is loading before MCM, fallback
            if (_loaded)
                return;

            SandBoxSubModuleOnSubModuleLoad();
            _loaded = true;
        }


        private static void SandBoxSubModuleOnSubModuleLoad()
        {
            BrushLoader.Inject(BaseResourceHandler.Instance);
            PrefabsLoader.Inject(BaseResourceHandler.Instance);
            WidgetLoader.Inject(BaseResourceHandler.Instance);

            UpdateOptionScreen(MCMUISettings.Instance!);
            MCMUISettings.Instance!.PropertyChanged += MCMSettings_PropertyChanged;
        }
        private static void OnSubModuleLoad()
        {
            MCMUISettings.Instance!.PropertyChanged -= MCMSettings_PropertyChanged;
        }

        private static void MCMSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is MCMUISettings settings && e.PropertyName == BaseSettings.SaveTriggered)
            {
                UpdateOptionScreen(settings);
            }
        }

        private static void UpdateOptionScreen(MCMUISettings settings)
        {
            if (settings.UseStandardOptionScreen)
            {
                OverrideEscapeMenu();
                OverrideMissionEscapeMenu();

                BaseGameMenuScreenHandler.Instance.RemoveScreen("MCM_OptionScreen_v3");
                BaseIngameMenuScreenHandler.Instance.RemoveScreen("MCM_OptionScreen_v3");
            }
            else
            {
                OverrideEscapeMenu(true);
                OverrideMissionEscapeMenu(true);

                BaseGameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen_v3",
                    9990,
                    () => DI.GetImplementation<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=MainMenu_ModOptions}Mod Options"));
                BaseIngameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen_v3",
                    1,
                    () => DI.GetImplementation<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=EscapeMenu_ModOptions}Mod Options", null));
            }
        }

        private static void OverrideEscapeMenu(bool returnDefault = false)
        {
            if (returnDefault)
            {
                OverrideView(typeof(OptionsScreen), typeof(OptionsGauntletScreen));
            }
            else
            {
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(IOptionsWithMCMOptionsScreen)));
                var latestImplementation = VersionUtils.GetLastImplementation(ApplicationVersionUtils.GameVersion(), types);
                if (latestImplementation != null)
                {
                    OverrideView(typeof(OptionsScreen), latestImplementation?.Type!);
                }
            }
        }
        private static void OverrideMissionEscapeMenu(bool returnDefault = false)
        {
            if (returnDefault)
            {
                OverrideView(typeof(MissionOptionsUIHandler), typeof(MissionGauntletOptionsUIHandler));
            }
            else
            {
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(IOptionsWithMCMOptionsMissionView)));
                var latestImplementation = VersionUtils.GetLastImplementation(ApplicationVersionUtils.GameVersion(), types);
                if (latestImplementation != null)
                {
                    OverrideView(typeof(MissionOptionsUIHandler), latestImplementation?.Type!);
                }
            }
        }

        private static void OverrideView(Type baseType, Type type)
        {
            var actualViewTypes = (Dictionary<Type, Type>)AccessTools.Field(typeof(ViewCreatorManager), "_actualViewTypes").GetValue(null);

            if (actualViewTypes.ContainsKey(baseType))
                actualViewTypes[baseType] = type;
            else
                actualViewTypes.Add(baseType, type);
        }
    }
}