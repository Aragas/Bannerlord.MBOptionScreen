using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Functionality.Wrapper
{
    public sealed class IngameMenuScreenHandlerWrapper : IIngameMenuScreenHandler, IWrapper
    {
        private readonly object _object;
        private MethodInfo? AddScreenMethod { get; }
        public bool IsCorrect { get; }

        public IngameMenuScreenHandlerWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            AddScreenMethod = AccessTools.Method(type, nameof(AddScreen));

            IsCorrect = AddScreenMethod != null;
        }

        public void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text) =>
            AddScreenMethod?.Invoke(_object, new object[] { index, screenFactory, text });
    }
}