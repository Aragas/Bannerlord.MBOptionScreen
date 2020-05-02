using HarmonyLib;

using MBOptionScreen.Attributes;

using System;
using System.Collections;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MBOptionScreen.Functionality
{
    [Version("e1.0.0",  220)]
    [Version("e1.0.1",  220)]
    [Version("e1.0.2",  220)]
    [Version("e1.0.3",  220)]
    [Version("e1.0.4",  220)]
    [Version("e1.0.5",  220)]
    [Version("e1.0.6",  220)]
    [Version("e1.0.7",  220)]
    [Version("e1.0.8",  220)]
    [Version("e1.0.9",  220)]
    [Version("e1.0.10", 220)]
    [Version("e1.0.11", 220)]
    [Version("e1.1.0",  220)]
    [Version("e1.2.0",  220)]
    [Version("e1.2.1",  220)]
    [Version("e1.3.0",  220)]
    public sealed class DefaultFunctionalityImplementation :
        IGameMenuScreenHandler,
        IModLibScreenOverrider
    {
        private static readonly AccessTools.FieldRef<Module, IList> _initialStateOptions =
            AccessTools.FieldRefAccess<Module, IList>("_initialStateOptions");

        void IGameMenuScreenHandler.AddScreen(string internalName, int index, Func<ScreenBase> screenFactory, TextObject text)
        {
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption(
                internalName,
                text,
                index,
                () => ScreenManager.PushScreen(screenFactory()),
                false));
        }
        void IGameMenuScreenHandler.RemoveScreen(string internalName)
        {
            var screen = Module.CurrentModule.GetInitialStateOptionWithId(internalName);
            _initialStateOptions(Module.CurrentModule).Remove(screen);
        }

        void IModLibScreenOverrider.OverrideModLibScreen()
        {
            var oldOptionScreen = Module.CurrentModule.GetInitialStateOptionWithId("ModOptionsMenu");
            if (oldOptionScreen != null)
            {
                _initialStateOptions(Module.CurrentModule).Remove(oldOptionScreen);
            }
        }
    }
}