using MCM.Utils;

using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Functionality
{
    public abstract class BaseIngameMenuScreenHandler
    {
        private static BaseIngameMenuScreenHandler? _instance;
        public static BaseIngameMenuScreenHandler Instance =>
            _instance ??= DI.GetImplementation<BaseIngameMenuScreenHandler, IngameMenuScreenHandlerWrapper>(ApplicationVersionUtils.GameVersion())!;

        public abstract void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject text);
        public abstract void RemoveScreen(string internalName);
    }
}