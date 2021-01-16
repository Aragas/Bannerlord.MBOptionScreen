using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Functionality
{
    public sealed class IngameMenuScreenHandlerWrapper : BaseIngameMenuScreenHandler, IWrapper
    {
        public object Object { get; }
        public bool IsCorrect { get; }

        public IngameMenuScreenHandlerWrapper(object @object) { }

        public override void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject text) { }
        public override void RemoveScreen(string internalName) { }
    }
}