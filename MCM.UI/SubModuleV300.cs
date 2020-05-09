using HarmonyLib;

using MCM.Abstractions;
using MCM.Abstractions.Functionality;
using MCM.Abstractions.Settings;
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
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        private static readonly FieldInfo _actualViewTypesField = AccessTools.Field(typeof(ViewCreatorManager), "_actualViewTypes");

        protected override void OnSubModuleLoad()
        {
            BrushLoader.Inject(BaseResourceHandler.Instance);
            PrefabsLoader.Inject(BaseResourceHandler.Instance);
            WidgetLoader.Inject(BaseResourceHandler.Instance);

            UpdateOptionScreen(MCMSettings.Instance!);
            MCMSettings.Instance!.PropertyChanged += MCMSettings_PropertyChanged;
        }
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            MCMSettings.Instance!.PropertyChanged -= MCMSettings_PropertyChanged;
        }

        private static void MCMSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is MCMSettings settings && e.PropertyName == BaseSettings.SaveTriggered)
            {
                UpdateOptionScreen(settings);
            }
        }

        private static void UpdateOptionScreen(MCMSettings settings)
        {
            var gameVersion = ApplicationVersionUtils.GameVersion();

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
                    () => DI.GetImplementation(gameVersion, typeof(MCMOptionsScreen).FullName) as ScreenBase,
                    new TextObject("{=HiZbHGvYG}Mod Options"));
                BaseIngameMenuScreenHandler.Instance.AddScreen(
                    "MCM_OptionScreen_v3",
                    1,
                    () => DI.GetImplementation(gameVersion, typeof(MCMOptionsScreen).FullName) as ScreenBase,
                    new TextObject("{=NqarFr4P}Mod Options", null));
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
                    .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(OptionsWithMCMOptionsScreen)));
                var latestImplementation = AttributeUtils.Get(ApplicationVersionUtils.GameVersion(), types);
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
                    .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(OptionsWithMCMOptionsMissionView)));
                var latestImplementation = AttributeUtils.Get(ApplicationVersionUtils.GameVersion(), types);
                if (latestImplementation != null)
                {
                    OverrideView(typeof(MissionOptionsUIHandler), latestImplementation?.Type!);
                }
            }
        }

        private static void OverrideView(Type baseType, Type type)
        {
            var actualViewTypes = (Dictionary<Type, Type>) _actualViewTypesField.GetValue(null);

            if (actualViewTypes.ContainsKey(baseType))
                actualViewTypes[baseType] = type;
            else
                actualViewTypes.Add(baseType, type);
        }
    }
}