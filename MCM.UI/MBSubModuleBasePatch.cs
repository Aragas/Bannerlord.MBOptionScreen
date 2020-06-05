using HarmonyLib;

using MCM.Abstractions.Functionality;
using MCM.Abstractions.Settings.Base;
using MCM.UI.Functionality.Loaders;
using MCM.Utils;

using System.ComponentModel;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;

namespace MCM.UI
{
    /// <summary>
    /// Instead of having a direct dependency of Sandbox Module, hook to OnSubModuleLoad to check when it's
    /// done with initialization
    /// </summary>
    internal class MBSubModuleBasePatch
    {
        private static bool _loaded = false;

        public static MethodBase OnGauntletUISubModuleSubModuleLoadTargetMethod { get; } =
            AccessTools.Method(typeof(GauntletUISubModule), "OnSubModuleLoad");
        public static MethodBase OnSubModuleUnloadedTargetMethod { get; } =
            AccessTools.Method(typeof(MBSubModuleBase), "OnSubModuleUnloaded");
        public static MethodBase OnBeforeInitialModuleScreenSetAsRootTargetMethod { get; } =
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
            if (__instance is MCMUISubModule)
                SubModuleV300SubModuleLoad();
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
            MCMUISubModule._extender.Register();

            BrushLoader.Inject(BaseResourceHandler.Instance);
            PrefabsLoader.Inject(BaseResourceHandler.Instance);
            WidgetLoader.Inject(BaseResourceHandler.Instance);

            UpdateOptionScreen(MCMUISettings.Instance!);
            MCMUISettings.Instance!.PropertyChanged += MCMSettings_PropertyChanged;
        }
        private static void SubModuleV300SubModuleLoad()
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
                MCMUISubModule._extender.Enable();

                BaseGameMenuScreenHandler.Instance.RemoveScreen("MCM_OptionScreen");
                BaseIngameMenuScreenHandler.Instance.RemoveScreen("MCM_OptionScreen");
            }
            else
            {
                MCMUISubModule._extender.Disable();

                BaseGameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen",
                    9990,
                    () => DI.GetImplementation<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=MainMenu_ModOptions}Mod Options"));
                BaseIngameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen",
                    1,
                    () => DI.GetImplementation<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=EscapeMenu_ModOptions}Mod Options"));
            }
        }
    }
}