using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MBOptionScreen.Functionality
{
    public interface IGameMenuScreenHandler
    {
        void AddScreen(string internalName, int index, Func<ScreenBase> screenFactory, TextObject text);
        void RemoveScreen(string internalName);
    }
}