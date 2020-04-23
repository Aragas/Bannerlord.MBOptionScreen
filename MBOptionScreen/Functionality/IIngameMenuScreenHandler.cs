using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MBOptionScreen.Functionality
{
    public interface IIngameMenuScreenHandler
    {
        void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text);
    }
}