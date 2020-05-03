using HarmonyLib;

using MCM.Abstractions;
using MCM.Abstractions.Functionality;
using MCM.Abstractions.ResourceInjection;
using MCM.Abstractions.Settings;
using MCM.UI.GUI.GauntletUI;
using MCM.UI.ResourceInjection.Loaders;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.View.Screen;

namespace MCM.UI
{
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        private static readonly FieldInfo _actualViewTypesField = AccessTools.Field(typeof(ViewCreatorManager), "_actualViewTypes");

        protected override void OnSubModuleLoad()
        {
            BrushLoader.Inject(BaseResourceInjector.Instance);
            PrefabsLoader.Inject(BaseResourceInjector.Instance);
            WidgetLoader.Inject(BaseResourceInjector.Instance);

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
            if (sender is MCMSettings settings && e.PropertyName == SettingsBase.SaveTriggered)
            {
                UpdateOptionScreen(settings);
            }
        }

        private static void UpdateOptionScreen(MCMSettings settings)
        {
            var gameVersion = ApplicationVersionUtils.GameVersion();
            if (settings.UseStandardOptionScreen)
            {
                OverrideScreen(typeof(OptionsScreen), typeof(OptionsWithModOptionsGauntletScreen));


                Resolver.GameMenuScreenHandler.RemoveScreen("MCM_OptionScreen_v3");
                Resolver.IngameMenuScreenHandler.RemoveScreen("MCM_OptionScreen_v3");
            }
            else
            {
                OverrideScreen(typeof(OptionsScreen), typeof(OptionsGauntletScreen));


                Resolver.GameMenuScreenHandler.AddScreen(
                    "MCM_OptionScreen_v3",
                    9990,
                    () => (ScreenBase) DI.GetImplementation(gameVersion, typeof(MCMOptionsScreen).FullName),
                    new TextObject("{=HiZbHGvYG}Mod Options"));
                Resolver.IngameMenuScreenHandler.AddScreen(
                    "MCM_OptionScreen_v3",
                    1,
                    () => (ScreenBase) DI.GetImplementation(gameVersion, typeof(MCMOptionsScreen).FullName),
                    new TextObject("{=NqarFr4P}Mod Options", null));
            }
        }

        private static void OverrideScreen(Type baseType, Type type)
        {
            var actualViewTypes = (Dictionary<Type, Type>) _actualViewTypesField.GetValue(null);

            if (actualViewTypes.ContainsKey(baseType))
                actualViewTypes[baseType] = type;
            else
                actualViewTypes.Add(baseType, type);
        }
    }
}