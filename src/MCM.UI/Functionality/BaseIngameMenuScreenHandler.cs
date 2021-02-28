using MCM.DependencyInjection;

using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.UI.Functionality
{
    public abstract class BaseIngameMenuScreenHandler
    {
        public static BaseIngameMenuScreenHandler? Instance => GenericServiceProvider.GetService<BaseIngameMenuScreenHandler>();

        public abstract void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject? text);
        public abstract void RemoveScreen(string internalName);
    }
}