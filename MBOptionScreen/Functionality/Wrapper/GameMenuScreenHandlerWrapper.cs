using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MBOptionScreen.Functionality
{
    internal sealed class GameMenuScreenHandlerWrapper : IGameMenuScreenHandler, IWrapper
    {
        private readonly object _object;
        private MethodInfo AddScreenMethod { get; }
        private MethodInfo RemoveScreenMethod { get; }
        public bool IsCorrect { get; }

        public GameMenuScreenHandlerWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            AddScreenMethod = AccessTools.Method(type, "AddScreen");
            RemoveScreenMethod = AccessTools.Method(type, "RemoveScreen");

            IsCorrect = AddScreenMethod != null && RemoveScreenMethod != null;
        }

        public void AddScreen(string internalName, int index, Func<ScreenBase> screenFactory, TextObject text) =>
            AddScreenMethod.Invoke(_object, new object[] { internalName, index, screenFactory, text });
        public void RemoveScreen(string internalName) =>
            RemoveScreenMethod.Invoke(_object, new object[] { internalName });
    }
}