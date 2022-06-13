using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.DelayedSubModule;
using Bannerlord.ButterLib.HotKeys;
using Bannerlord.UIExtenderEx;

using BUTR.DependencyInjection;
using BUTR.DependencyInjection.ButterLib;
using BUTR.DependencyInjection.Extensions;
using BUTR.DependencyInjection.Logger;

using HarmonyLib;

using MCM.Abstractions.Settings.Base;
using MCM.Extensions;
using MCM.UI.ButterLib;
using MCM.UI.Functionality;
using MCM.UI.Functionality.Injectors;
using MCM.UI.GUI.GauntletUI;
using MCM.UI.HotKeys;
using MCM.UI.Patches;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using SandBox;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace MCM.UI
{
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public sealed class MCMUISubModule : MBSubModuleBase
    {
        private const string SMessageContinue =
@"{=eXs6FLm5DP}It's strongly recommended to terminate the game now. Do you wish to terminate it?";
        private const string SWarningTitle =
@"{=dzeWx4xSfR}Warning from MCM!";
        private const string SMessageWrongGameVersion =
@"{=fGt6Gthg5y}This version of MCM is intended for e1.7.2 and higher! You are running {GAMEVERSION}!";


        private static readonly UIExtender Extender = new("MCM.UI");
        internal static ILogger<MCMUISubModule> Logger = NullLogger<MCMUISubModule>.Instance;
        internal static ResetValueToDefault? ResetValueToDefault;

        private bool DelayedServiceCreation { get; set; }
        private bool ServiceRegistrationWasCalled { get; set; }
        private bool OnBeforeInitialModuleScreenSetAsRootWasCalled { get; set; }

        public MCMUISubModule()
        {
            MCMSubModule.Instance?.OverrideServiceContainer(new ButterLibServiceContainer());

            CheckGameVersion();
            CheckLoadOrder();
        }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServiceContainer() is { } services)
            {
                services.AddSettingsContainer<ButterLibSettingsContainer>();

                services.AddTransient(typeof(IServiceProvider), () => this.GetTempServiceProvider() ?? this.GetServiceProvider()!);
                services.AddTransient<IBUTRLogger, LoggerWrapper>();
                services.AddTransient(typeof(IBUTRLogger<>), typeof(LoggerWrapper<>));


                services.AddTransient<IMCMOptionsScreen, ModOptionsGauntletScreen>();

                services.AddSingleton<BaseGameMenuScreenHandler, DefaultGameMenuScreenHandler>();
                services.AddSingleton<ResourceInjector, DefaultResourceInjector>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            IServiceProvider serviceProvider;

            if (!ServiceRegistrationWasCalled)
            {
                OnServiceRegistration();
                DelayedServiceCreation = true;
                serviceProvider = this.GetTempServiceProvider()!;
            }
            else
            {
                serviceProvider = this.GetServiceProvider()!;
            }

            Logger = serviceProvider.GetRequiredService<ILogger<MCMUISubModule>>();
            Logger.LogTrace("OnSubModuleLoad: Logging started...");

            var viewmodelwrapperHarmony = new Harmony("bannerlord.mcm.ui.viewmodelpatch");
            ViewModelPatch.Patch(viewmodelwrapperHarmony);

            var optionsGauntletScreenHarmony = new Harmony("bannerlord.mcm.ui.optionsgauntletscreenpatch");
            OptionsGauntletScreenPatch.Patch(optionsGauntletScreenHarmony);
            MissionGauntletOptionsUIHandlerPatch.Patch(optionsGauntletScreenHarmony);

            var optionsSwitchHarmony = new Harmony("bannerlord.mcm.ui.optionsswitchpatch");
            OptionsVMPatch.Patch(optionsSwitchHarmony);


            DelayedSubModuleManager.Register<SandBoxSubModule>();
            DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                nameof(OnSubModuleLoad), SubscriptionType.AfterMethod, (_, _) =>
                {
                    Extender.Register(typeof(MCMUISubModule).Assembly);
                    Extender.Enable();
                });
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (!OnBeforeInitialModuleScreenSetAsRootWasCalled)
            {
                OnBeforeInitialModuleScreenSetAsRootWasCalled = true;

                if (DelayedServiceCreation)
                {
                    Logger = this.GetServiceProvider().GetRequiredService<ILogger<MCMUISubModule>>();
                }

                DelayedSubModuleManager.Register<SandBoxSubModule>();
                DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                    nameof(OnBeforeInitialModuleScreenSetAsRoot), SubscriptionType.AfterMethod, (_, _) =>
                    {
                        var resourceInjector = GenericServiceProvider.GetService<ResourceInjector>();
                        resourceInjector?.Inject();

                        UpdateOptionScreen(MCMUISettings.Instance!);
                        MCMUISettings.Instance!.PropertyChanged += MCMSettings_PropertyChanged;
                    });

                if (HotKeyManager.Create("MCM.UI") is { } hkm)
                {
                    ResetValueToDefault = hkm.Add<ResetValueToDefault>();
                    hkm.Build();
                }
            }
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            DelayedSubModuleManager.Register<SandBoxSubModule>();
            DelayedSubModuleManager.Subscribe<SandBoxSubModule, MCMUISubModule>(
                nameof(OnSubModuleUnloaded), SubscriptionType.AfterMethod, (_, _) =>
                {
                    var instance = MCMUISettings.Instance;
                    if (instance is not null)
                        instance.PropertyChanged -= MCMSettings_PropertyChanged;
                });
        }

        private static void MCMSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
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
                BaseGameMenuScreenHandler.Instance?.RemoveScreen("MCM_OptionScreen");
            }
            else
            {
                BaseGameMenuScreenHandler.Instance?.AddScreen(
                    "MCM_OptionScreen",
                    9990,
                    () => GenericServiceProvider.GetService<IMCMOptionsScreen>() as ScreenBase,
                    new TextObject("{=MainMenu_ModOptions}Mod Options"));
            }
        }

        private static void CheckGameVersion()
        {
            var e172 = ApplicationVersionHelper.TryParse("e1.7.2", out var e172Var) ? e172Var : ApplicationVersion.Empty;
            if (ApplicationVersionHelper.GameVersion() is { } gameVersion && gameVersion < e172)
            {
                var sb = new StringBuilder();
                sb.AppendLine(new TextObject(SMessageWrongGameVersion, new() { { "GAMEVERSION", ApplicationVersionHelper.ToString(gameVersion) } }).ToString());
                sb.AppendLine();
                sb.AppendLine(new TextObject(SMessageContinue).ToString());
                switch (MessageBox.Show(sb.ToString(),
                            new TextObject(SWarningTitle).ToString(), MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions) 0x40000))
                {
                    case DialogResult.Yes:
                        Environment.Exit(1);
                        break;
                }
            }
        }
        private static void CheckLoadOrder()
        {
            var loadedModules = ModuleInfoHelper.GetLoadedModules().ToList();
            if (loadedModules.Count == 0) return;

            var sb = new StringBuilder();
            if (!ModuleInfoHelper.ValidateLoadOrder(typeof(MCMUISubModule), out var report))
            {
                sb.AppendLine(report);
                sb.AppendLine();
                sb.AppendLine(new TextObject(SMessageContinue).ToString());
                switch (MessageBox.Show(sb.ToString(),
                            new TextObject(SWarningTitle).ToString(), MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions) 0x40000))
                {
                    case DialogResult.Yes:
                        Environment.Exit(1);
                        break;
                }
            }
        }
    }
}