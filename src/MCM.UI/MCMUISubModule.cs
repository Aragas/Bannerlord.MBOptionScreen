using System;
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
        private static readonly UIExtender Extender = new UIExtender("MCM.UI");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var editabletextpatchHarmony = new Harmony("bannerlord.mcm.ui.editabletextpatch");
            EditableTextPatch.Patch(editabletextpatchHarmony);

            var viewmodelwrapperHarmony = new Harmony("bannerlord.mcm.ui.viewmodelpatch");
            ViewModelPatch.Patch(viewmodelwrapperHarmony);

            var widgetprefabpatchHarmony = new Harmony("bannerlord.mcm.ui.widgetprefabpatch");
            WidgetPrefabPatch.Patch(widgetprefabpatchHarmony);

            var services = this.GetServices();
            services.AddTransient<IMCMOptionsScreen, ModOptionsGauntletScreen>();
            services.AddSingleton<BaseResourceHandler, DefaultResourceHandler>();

            DelayedSubModuleManager.Register<SandBoxSubModule>();
            DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                nameof(OnSubModuleLoad), SubscriptionType.AfterMethod, (s, e) =>
                {
                    Extender.Register();
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
                    var instance = MCMUISettings.Instance;
                    if (instance != null)
                        instance.PropertyChanged -= MCMSettings_PropertyChanged;
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
                Extender.Enable();

                BaseGameMenuScreenHandler.Instance.RemoveScreen("MCM_OptionScreen");
                BaseIngameMenuScreenHandler.Instance.RemoveScreen("MCM_OptionScreen");
            }
            else
            {
                Extender.Disable();

                BaseGameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen",
                    9990,
                    () => ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=MainMenu_ModOptions}Mod Options"));
                BaseIngameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen",
                    1,
                    () => ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=EscapeMenu_ModOptions}Mod Options"));
            }
        }
    }
}