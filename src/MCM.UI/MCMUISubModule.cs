using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.DelayedSubModule;
using Bannerlord.UIExtenderEx;
using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib;

using MCM.Abstractions.Settings.Base;
using MCM.UI.Functionality;
using MCM.UI.Functionality.Injectors;
using MCM.UI.GUI.GauntletUI;
using MCM.UI.Patches;

using Microsoft.Extensions.DependencyInjection;

using SandBox;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine.Screens;
using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    public sealed class MCMUISubModule : MBSubModuleBase
    {
        private static readonly UIExtender Extender = new UIExtender("MCM.UI");
        private bool FirstInit { get; set; } = true;

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var editabletextpatchHarmony = new Harmony("bannerlord.mcm.ui.editabletextpatch");
            EditableTextPatch.Patch(editabletextpatchHarmony);

            var viewmodelwrapperHarmony = new Harmony("bannerlord.mcm.ui.viewmodelpatch");
            ViewModelPatch.Patch(viewmodelwrapperHarmony);

            var services = this.GetServices();
            services.AddSingleton<BaseIngameMenuScreenHandler, DefaultIngameMenuScreenHandler>();
            services.AddTransient<IMCMOptionsScreen, ModOptionsGauntletScreen>();

            DelayedSubModuleManager.Register<SandBoxSubModule>();
            DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                nameof(OnSubModuleLoad), SubscriptionType.AfterMethod, (s, e) =>
                {
                    Extender.Register(typeof(MCMUISubModule).Assembly);
                });

            if (ApplicationVersionUtils.GameVersion() is { } gameVersion)
            {
                if (gameVersion.Major <= 1 && gameVersion.Minor <= 5 && gameVersion.Revision <= 7)
                    services.AddSingleton<BaseGameMenuScreenHandler, Pre158GameMenuScreenHandler>();
                else
                    services.AddSingleton<BaseGameMenuScreenHandler, Post158GameMenuScreenHandler>();

                if (gameVersion.Major <= 1 && gameVersion.Minor <= 5 && gameVersion.Revision <= 3)
                    services.AddSingleton<IResourceInjector, ResourceInjectorPre154>();
                else
                    services.AddSingleton<IResourceInjector, ResourceInjectorPost154>();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (FirstInit)
            {
                FirstInit = false;

                DelayedSubModuleManager.Register<SandBoxSubModule>();
                DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                    nameof(OnBeforeInitialModuleScreenSetAsRoot), SubscriptionType.AfterMethod, (s, e) =>
                    {
                        var resourceInjector = this.GetServiceProvider().GetRequiredService<IResourceInjector>();

                        UpdateOptionScreen(MCMUISettings.Instance!);
                        MCMUISettings.Instance!.PropertyChanged += MCMSettings_PropertyChanged;
                    });
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            DelayedSubModuleManager.Register<SandBoxSubModule>();
            DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                nameof(OnSubModuleUnloaded), SubscriptionType.AfterMethod, (s, e) =>
                {
                    var instance = MCMUISettings.Instance;
                    if (instance is { })
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

                BaseGameMenuScreenHandler.Instance?.RemoveScreen("MCM_OptionScreen");
                BaseIngameMenuScreenHandler.Instance?.RemoveScreen("MCM_OptionScreen");
            }
            else
            {
                Extender.Disable();

                BaseGameMenuScreenHandler.Instance?.AddScreen(
                    "MCM_OptionScreen",
                    9990,
                    () => MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMCMOptionsScreen>() as ScreenBase,
                    TextObjectHelper.Create("{=MainMenu_ModOptions}Mod Options"));
                BaseIngameMenuScreenHandler.Instance?.AddScreen(
                    "MCM_OptionScreen",
                    1,
                    () => MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMCMOptionsScreen>() as ScreenBase,
                    TextObjectHelper.Create("{=EscapeMenu_ModOptions}Mod Options"));
            }
        }
    }
}