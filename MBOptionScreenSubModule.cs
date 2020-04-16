using HarmonyLib;

using MBOptionScreen.Attributes;
using MBOptionScreen.Interfaces;
using MBOptionScreen.ResourceInjection.EmbedLoaders;
using MBOptionScreen.Settings;
using MBOptionScreen.Settings.Wrapper;
using MBOptionScreen.State;

using SandBox.View.Map;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.DotNet;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection;

using Module = TaleWorlds.MountAndBlade.Module;

namespace MBOptionScreen
{
    public class MBOptionScreenSubModule : MBSubModuleBase
    {
        private static FieldInfo InitialStateOptionsField { get; } =
            typeof(Module).GetField("_initialStateOptions", BindingFlags.Instance | BindingFlags.NonPublic);
        private static MethodInfo OnEscapeMenuToggledMethod { get; } =
            typeof(MapScreen).GetMethod("OnEscapeMenuToggled", BindingFlags.Instance | BindingFlags.NonPublic);
        private static void GetEscapeMenuItemsPostfix(MapScreen __instance, List<EscapeMenuItemVM> __result)
        {
            __result.Insert(1, new EscapeMenuItemVM(
                new TextObject("{=NqarFr4P}Mod Options", null),
                obj =>
                {
                    OnEscapeMenuToggledMethod.Invoke(__instance, new object[] { false });
                    ScreenManager.PushScreen((ScreenBase) Activator.CreateInstance(SharedStateObject.ModOptionScreen));
                },
                null, false, false));
        }

        internal static SharedStateObject SharedStateObject;
        private static IStateProvider _stateProvider;

        protected override void OnSubModuleLoad()
        {
            StartInitialization();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            EndInitialization();
        }

        private static void StartInitialization()
        {
            var version = ApplicationVersionParser.TryParse(Managed.GetVersionStr(), out var v) ? v : default;
            var stateProviderTuple = AttributeHelper.Get<StateProviderVersionAttribute>(version);
            _stateProvider = (IStateProvider) Activator.CreateInstance(stateProviderTuple.Type);

            // Check if another 'copy' of MBOptionScreen inside another mod has already done initialization
            SharedStateObject = _stateProvider.Get<SharedStateObject>();

            // This is the first instance of MBOptionScreen to make the initialization 
            if (SharedStateObject == null)
            {
                // Find the latest implementation among loaded mods
                // The currently loaded MBOptionScreen might not be the one with the latest implementation versions,
                // but some other mod might have it included
                var modOptionsScreenTuple = AttributeHelper.Get<ModuleOptionVersionAttribute>(version);
                var fileStorageTuple = AttributeHelper.Get<FileStorageVersionAttribute>(version);
                var settingsStorageTuple = AttributeHelper.Get<SettingsStorageVersionAttribute>(version);
                var resourceInjectorTuple = AttributeHelper.Get<ResourceInjectorVersionAttribute>(version);

                // Initialize the shared class among other instances of MBOptionScreen.
                SharedStateObject = new SharedStateObject(
                    (ISettingsProvider) Activator.CreateInstance(settingsStorageTuple.Type),
                    (IResourceInjector) Activator.CreateInstance(resourceInjectorTuple.Type),
                    modOptionsScreenTuple.Type);
                _stateProvider.Set(SharedStateObject);

                Module.CurrentModule.AddInitialStateOption(new InitialStateOption("ModOptionsMenu_MBOptionScreen", new TextObject("{=HiZbHGvYG}Mod Options"), 9990, () =>
                {
                    var screen = (ScreenBase) Activator.CreateInstance(SharedStateObject.ModOptionScreen);
                    ScreenManager.PushScreen(screen);
                }, false));
            }
        }

        private static void EndInitialization()
        {
            if (!SharedStateObject.HasInitialized)
            {
                var settings = new List<SettingsBase>();
                var allTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .ToList();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var type = assembly.GetType("ModLib.SettingsBase") ?? assembly.GetType("MBOptionScreen.Settings.SettingsBase");
                    if (type == null || type == typeof(SettingsBase))
                        continue;

                    settings.AddRange(allTypes
                        .Where(t => t.IsSubclassOf(type))
                        .Select(obj => new WrapperSettings(Activator.CreateInstance(obj))));
                }
                settings.AddRange(allTypes
                    .Where(t => t.IsSubclassOf(typeof(SettingsBase)) &&
                                t != typeof(WrapperSettings)
#if !DEBUG
                                && t != typeof(MBOptionScreenSettings)
                                && t != typeof(TestSettings)
#endif
                                )
                    .Select(t => (SettingsBase) Activator.CreateInstance(t)));
                foreach (var setting in settings)
                    SettingsDatabase.RegisterSettings(setting);


                // Mark so if necessary, something can unpatch it
                new Harmony("bannerlord.mboptionscreen.defaultmapscreeninjection_v1").Patch(
                    original: typeof(MapScreen).GetMethod("GetEscapeMenuItems", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
                    postfix: new HarmonyMethod(typeof(MBOptionScreenSubModule).GetMethod("GetEscapeMenuItemsPostfix", BindingFlags.Static | BindingFlags.NonPublic)));

                var oldOptionScreen = Module.CurrentModule.GetInitialStateOptionWithId("ModOptionsMenu");
                if (oldOptionScreen != null)
                {
                    var list = (IList)InitialStateOptionsField.GetValue(Module.CurrentModule);
                    list.Remove(oldOptionScreen);
                }


                BrushLoaderV1a.Inject(SharedStateObject.ResourceInjector);
                PrefabsLoaderV1a.Inject(SharedStateObject.ResourceInjector);

                SharedStateObject.HasInitialized = true;
            }

            _stateProvider.Clear();
        }
    }
}