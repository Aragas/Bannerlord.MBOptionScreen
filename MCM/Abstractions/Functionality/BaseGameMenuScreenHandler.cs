using MCM.Utils;

using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Functionality
{
    public abstract class BaseGameMenuScreenHandler
    {
        private static BaseGameMenuScreenHandler? _instance;
        public static BaseGameMenuScreenHandler Instance =>
            _instance ??= DI.GetImplementation<BaseGameMenuScreenHandler, GameMenuScreenHandlerWrapper>()!;

        public abstract void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject text);
        public abstract void RemoveScreen(string internalName);
    }
}