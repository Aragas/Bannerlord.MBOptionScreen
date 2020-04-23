using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MBOptionScreen.Functionality
{
    internal sealed class IngameMenuScreenHandlerWrapper : IIngameMenuScreenHandler
    {
        private readonly object _object;

        private MethodInfo AddScreenMethod { get; }

        public IngameMenuScreenHandlerWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            AddScreenMethod = AccessTools.Method(type, "AddScreen");
        }

        public void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text) =>
            AddScreenMethod.Invoke(_object, new object[] { index, screenFactory, text });
    }
}