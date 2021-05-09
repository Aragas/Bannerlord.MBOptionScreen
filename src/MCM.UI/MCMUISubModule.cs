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

using TaleWorlds.Engine.Screens;
using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public sealed class MCMUISubModule : MBSubModuleBase
    {
        private const string SErrorHarmonyNotFound =
@"{=EEVJa5azpB}Bannerlord.Harmony module was not found!";
        private const string SErrorUIExtenderExNotFound =
@"{=YjsGP3mUaj}Bannerlord.UIExtenderEx module was not found!";
        private const string SErrorButterLibNotFound =
@"{=5EDzm7u4mS}Bannerlord.ButterLib module was not found!";
        private const string SMessageContinue =
@"{=eXs6FLm5DP}It's strongly recommended to terminate the game now. Do you wish to terminate it?";
        private const string SWarningTitle =
@"{=dzeWx4xSfR}Warning from MCM!";
        private const string SErrorOfficialModulesLoadedBeforeMCM =
@"{=BccWuuSR6a}MCM is loaded after the official modules!
Make sure MCM is loaded before them!";
        private const string SErrorOfficialModules =
@"{=JP23gY34Gm}The following modules were loaded before MCM:";


        internal static ILogger<MCMUISubModule> Logger = NullLogger<MCMUISubModule>.Instance;
        private static UIExtender Extender = new("MCM.UI");
        internal static ResetValueToDefault? ResetValueToDefault;

        private bool DelayedServiceCreation { get; set; }
        private bool ServiceRegistrationWasCalled { get; set; }
        private bool OnBeforeInitialModuleScreenSetAsRootWasCalled { get; set; }

        public MCMUISubModule()
        {
            MCMSubModule.Instance?.OverrideServiceContainer(new ButterLibServiceContainer());

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

                if (ApplicationVersionHelper.GameVersion() is { } gameVersion)
                {
                    if (gameVersion.Major <= 1 && gameVersion.Minor <= 5 && gameVersion.Revision <= 7)
                        services.AddSingleton<BaseGameMenuScreenHandler, Pre158GameMenuScreenHandler>();
                    else
                        services.AddSingleton<BaseGameMenuScreenHandler, Post158GameMenuScreenHandler>();

                    if (gameVersion.Major <= 1 && gameVersion.Minor <= 5 && gameVersion.Revision <= 3)
                        services.AddSingleton<ResourceInjector, ResourceInjectorPre154>();
                    else
                        services.AddSingleton<ResourceInjector, ResourceInjectorPost154>();
                }
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

            var editabletextpatchHarmony = new Harmony("bannerlord.mcm.ui.editabletextpatch");
            EditableTextPatch.Patch(editabletextpatchHarmony);

            var viewmodelwrapperHarmony = new Harmony("bannerlord.mcm.ui.viewmodelpatch");
            ViewModelPatch.Patch(viewmodelwrapperHarmony);


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
                    TextObjectHelper.Create("{=MainMenu_ModOptions}Mod Options"));
            }
        }

        private static void CheckLoadOrder()
        {
            var loadedModules = ModuleInfoHelper.GetLoadedModules().ToList();
            if (loadedModules.Count == 0) return;

            var sb = new StringBuilder();

            var harmonyModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.Harmony");
            var harmonyModuleIndex = harmonyModule is not null ? loadedModules.IndexOf(harmonyModule) : -1;
            if (harmonyModuleIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorHarmonyNotFound)?.ToString());
            }

            var butterLibModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.ButterLib");
            var butterLibModuleIndex = butterLibModule is not null ? loadedModules.IndexOf(butterLibModule) : -1;
            if (butterLibModuleIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorButterLibNotFound)?.ToString());
            }

            var uiExtenderExModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.UIExtenderEx");
            var uiExtenderExIndex = uiExtenderExModule is not null ? loadedModules.IndexOf(uiExtenderExModule) : -1;
            if (uiExtenderExIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorUIExtenderExNotFound)?.ToString());
            }

            var mcmModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.MBOptionScreen");
            var mcmIndex = mcmModule is not null ? loadedModules.IndexOf(mcmModule) : -1;
            var officialModules = loadedModules.Where(x => x.IsOfficial).Select(x => (Module: x, Index: loadedModules.IndexOf(x)));
            var modulesLoadedBefore = officialModules.Where(tuple => tuple.Index < mcmIndex).ToList();
            if (modulesLoadedBefore.Count > 0)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorOfficialModulesLoadedBeforeMCM)?.ToString());
                sb.AppendLine(TextObjectHelper.Create(SErrorOfficialModules)?.ToString());
                foreach (var (module, _) in modulesLoadedBefore)
                    sb.AppendLine(module.Id);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SMessageContinue)?.ToString());

                switch (MessageBox.Show(sb.ToString(), TextObjectHelper.Create(SWarningTitle)?.ToString(), MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        Environment.Exit(1);
                        break;
                }
            }
        }
    }
}