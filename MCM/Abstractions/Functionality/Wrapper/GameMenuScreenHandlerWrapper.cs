using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Functionality.Wrapper
{
    public sealed class GameMenuScreenHandlerWrapper : IGameMenuScreenHandler, IWrapper
    {
        public object Object { get; }
        private MethodInfo? AddScreenMethod { get; }
        private MethodInfo? RemoveScreenMethod { get; }
        public bool IsCorrect { get; }

        public GameMenuScreenHandlerWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            AddScreenMethod = AccessTools.Method(type, nameof(AddScreen));
            RemoveScreenMethod = AccessTools.Method(type, nameof(RemoveScreen));

            IsCorrect = AddScreenMethod != null && RemoveScreenMethod != null;
        }

        public void AddScreen(string internalName, int index, Func<ScreenBase> screenFactory, TextObject text) =>
            AddScreenMethod?.Invoke(Object, new object[] { internalName, index, screenFactory, text });
        public void RemoveScreen(string internalName) =>
            RemoveScreenMethod?.Invoke(Object, new object[] { internalName });
    }
}