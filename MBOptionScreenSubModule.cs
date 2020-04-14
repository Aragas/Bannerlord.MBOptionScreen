using HarmonyLib;

using MBOptionScreen.Attributes;
using MBOptionScreen.ResourceInjection;
using MBOptionScreen.SettingDatabase;

using ModLib;

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
                    ScreenManager.PushScreen((ScreenBase) Activator.CreateInstance(SyncObject.ModOptionScreen));
                },
                null, false, false));
        }

        static MBOptionScreenSubModule()
        {
            // Mark so if necessary, something can unpatch it
            new Harmony("bannerlord.mboptionscreen.defaultmapscreeninjection_v1").Patch(
                original: typeof(MapScreen).GetMethod("GetEscapeMenuItems", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
                prefix: new HarmonyMethod(typeof(MBOptionScreenSubModule).GetMethod("GetEscapeMenuItemsPostfix", BindingFlags.Static | BindingFlags.NonPublic)));
        }

        internal static SyncObjectV1 SyncObject;

        protected IResourceInjector ResourceInjector => SyncObject.ResourceInjector;

        protected override void OnSubModuleLoad()
        {
            // Check if another 'copy' of MBOptionScreen inside another mod has already done initialization
            SyncObject = (SyncObjectV1) Module.CurrentModule.GetInitialStateOptionWithId(SyncObjectV1.SyncId);

            // This is the first instance of MBOptionScreen to make the initialization 
            if (SyncObject == null)
            {
                // Find the latest implementation among loaded mods
                // The currently loaded MBOptionScreen might not the the latest one,
                // but some other mod might have it inside itself
                var version = ApplicationVersionParser.TryParse(Managed.GetVersionStr(), out var v) ? v : default;
                var modOptionsGauntletScreenType = AttributeHelper.Get<ModuleOptionVersionAttribute>(version);
                var fileStorageType = AttributeHelper.Get<FileStorageVersionAttribute>(version);
                var settingsStorageType = AttributeHelper.Get<SettingsStorageVersionAttribute>(version);
                var resourceInjectorType = AttributeHelper.Get<ResourceInjectorVersionAttribute>(version);

                // Initialize the shared class among other instances of MBOptionScreen.
                SyncObject = new SyncObjectV1
                {
                    FileStorage = (IFileStorage) Activator.CreateInstance(fileStorageType.Type),
                    SettingsStorage = (ISettingsStorage) Activator.CreateInstance(settingsStorageType.Type),
                    ResourceInjector = (IResourceInjector) Activator.CreateInstance(resourceInjectorType.Type),
                    ModOptionScreen = modOptionsGauntletScreenType.Type
                };
                // TODO: Get folder from SettingsBase
                SyncObject.FileStorage.Initialize("MBOptionScreen");

                // Module._initialStateOptions was used to host the shared class,
                // as the game is using the list only after OnBeforeInitialModuleScreenSetAsRoot() was called
                Module.CurrentModule.AddInitialStateOption(SyncObject); // Workaround


                Module.CurrentModule.AddInitialStateOption(new InitialStateOption("ModOptionsMenu", new TextObject("{=HiZbHGvYG}Mod Options"), 9990, () =>
                {
                    var screen = (ScreenBase) Activator.CreateInstance(SyncObject.ModOptionScreen);
                    ScreenManager.PushScreen(screen);
                }, false));
            }


            var settingsEnumerable = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(SettingsBase)) && t != typeof(MBOptionScreenSettings));
            foreach (var settings in settingsEnumerable)
                SettingsDatabase.RegisterSettings((SettingsBase) Activator.CreateInstance(settings));
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (!SyncObject.HasInitializedVM)
            {
                ResourceInjector.InjectBrush(BrushLoader.DividerBrushes_v1());
                ResourceInjector.InjectBrush(BrushLoader.ModSettingsItem_v1Brush_v1());
                ResourceInjector.InjectBrush(BrushLoader.ResetButtonBrush_v1());
                ResourceInjector.InjectBrush(BrushLoader.TextBrushes_v1());

                ResourceInjector.InjectPrefab("ModSettingsItem_v1", PrefabsLoader.ModSettingsItem_v1());
                ResourceInjector.InjectPrefab("SettingPropertyGroupView_v1", PrefabsLoader.SettingPropertyGroupView_v1());
                ResourceInjector.InjectPrefab("SettingPropertyView_v1", PrefabsLoader.SettingPropertyView_v1());
                ResourceInjector.InjectPrefab("SettingsView_v1", PrefabsLoader.SettingsView_v1());

                SyncObject.HasInitializedVM = true;
            }

            var list = InitialStateOptionsField.GetValue(Module.CurrentModule) as IList;
            list?.Remove(SyncObject);
        }


        /// <summary>
        /// A shareable object between multiple mods that will use this library
        /// Life length expectation - OnSubModuleLoad()->OnBeforeInitialModuleScreenSetAsRoot()
        /// </summary>
        internal class SyncObjectV1 : InitialStateOption
        {
            public static string SyncId = "mboptionscreen_synchronization_object";

            public bool HasInitializedVM { get; internal set; }
            public Type ModOptionScreen { get; internal set; }
            public IFileStorage FileStorage { get; internal set; }
            public ISettingsStorage SettingsStorage { get; internal set; }
            public IResourceInjector ResourceInjector { get; internal set; }

            public SyncObjectV1() : base(SyncId, new TextObject(""), 1, null, true) { }
        }
    }
}