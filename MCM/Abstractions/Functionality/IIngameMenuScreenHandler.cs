using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Functionality
{
    public interface IIngameMenuScreenHandler
    {
        void AddScreen(string internalName, int index, Func<ScreenBase> screenFactory, TextObject text);
        void RemoveScreen(string internalName);
    }
}