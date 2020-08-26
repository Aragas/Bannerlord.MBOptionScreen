using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.DelayedSubModule;
using Bannerlord.UIExtenderEx;

using HarmonyLib;

using MCM.Abstractions.Functionality;
using MCM.Abstractions.Settings.Base;
using MCM.UI.Functionality;
using MCM.UI.Functionality.Loaders;
using MCM.UI.GUI.GauntletUI;
using MCM.UI.Patches;

using Microsoft.Extensions.DependencyInjection;

using SandBox;

using System.ComponentModel;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    public sealed class MCMUISubModule : MBSubModuleBase
    {
        public static readonly UIExtender _extender = new UIExtender("MCM.UI");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var editabletextpatchHarmony = new Harmony("bannerlord.mcm.ui.editabletextpatch");
            editabletextpatchHarmony.Patch(
                EditableTextPatch.GetCursorPositionMethod,
                finalizer: new HarmonyMethod(typeof(EditableTextPatch), nameof(EditableTextPatch.GetCursorPosition)));

            var viewmodelwrapperHarmony = new Harmony("bannerlord.mcm.ui.viewmodelwrapper");
            viewmodelwrapperHarmony.Patch(
                ViewModelPatch.ExecuteCommandMethod,
                prefix: new HarmonyMethod(typeof(ViewModelPatch), nameof(ViewModelPatch.ExecuteCommandPatch)));

            var services = this.GetServices();
            services.AddTransient<IMCMOptionsScreen, ModOptionsGauntletScreen>();
            services.AddTransient<BaseResourceHandler, DefaultResourceInjector>();

            DelayedSubModuleManager.Register<SandBoxSubModule>();
            DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                nameof(OnSubModuleLoad), SubscriptionType.AfterMethod, (s, e) =>
                {
                    _extender.Register();
                });
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            DelayedSubModuleManager.Register<SandBoxSubModule>();
            DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                nameof(OnBeforeInitialModuleScreenSetAsRoot), SubscriptionType.AfterMethod, (s, e) =>
                {
                    BrushLoader.Inject(BaseResourceHandler.Instance);
                    PrefabsLoader.Inject(BaseResourceHandler.Instance);
                    WidgetLoader.Inject(BaseResourceHandler.Instance);

                    UpdateOptionScreen(MCMUISettings.Instance!);
                    MCMUISettings.Instance!.PropertyChanged += MCMSettings_PropertyChanged;
                });
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            DelayedSubModuleManager.Register<SandBoxSubModule>();
            DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                nameof(OnSubModuleUnloaded), SubscriptionType.AfterMethod, (s, e) =>
                {
                    MCMUISettings.Instance!.PropertyChanged -= MCMSettings_PropertyChanged;
                });
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
                _extender.Enable();

                BaseGameMenuScreenHandler.Instance.RemoveScreen("MCM_OptionScreen");
                BaseIngameMenuScreenHandler.Instance.RemoveScreen("MCM_OptionScreen");
            }
            else
            {
                _extender.Disable();

                BaseGameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen",
                    9990,
                    //() => DI.GetImplementation<IMCMOptionsScreen>() as ScreenBase,
                    () => ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=MainMenu_ModOptions}Mod Options"));
                BaseIngameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen",
                    1,
                    //() => DI.GetImplementation<IMCMOptionsScreen>() as ScreenBase,
                    () => ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=EscapeMenu_ModOptions}Mod Options"));
            }
        }
    }
}