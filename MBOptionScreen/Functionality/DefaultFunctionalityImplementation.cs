using HarmonyLib;

using MBOptionScreen.Attributes;

using System;
using System.Collections;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MBOptionScreen.Functionality
{
    [Version("e1.0.0",  200)]
    [Version("e1.0.1",  200)]
    [Version("e1.0.2",  200)]
    [Version("e1.0.3",  200)]
    [Version("e1.0.4",  200)]
    [Version("e1.0.5",  200)]
    [Version("e1.0.6",  200)]
    [Version("e1.0.7",  200)]
    [Version("e1.0.8",  200)]
    [Version("e1.0.9",  200)]
    [Version("e1.0.10", 200)]
    [Version("e1.0.11", 200)]
    [Version("e1.1.0",  200)]
    [Version("e1.2.0",  200)]
    [Version("e1.2.1",  200)]
    [Version("e1.3.0",  200)]
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