using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Functionality
{
    public sealed class IngameMenuScreenHandlerWrapper : BaseIngameMenuScreenHandler, IWrapper
    {
        public object Object { get; }
        private MethodInfo? AddScreenMethod { get; }
        private MethodInfo? RemoveScreenMethod { get; }
        public bool IsCorrect { get; }

        public IngameMenuScreenHandlerWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            AddScreenMethod = AccessTools.Method(type, nameof(AddScreen));
            RemoveScreenMethod = AccessTools.Method(type, nameof(RemoveScreen));

            IsCorrect = AddScreenMethod != null;
        }

        public override void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject text) =>
            AddScreenMethod?.Invoke(Object, new object[] { internalName, index, screenFactory, text });
        public override void RemoveScreen(string internalName) =>
            RemoveScreenMethod?.Invoke(Object, new object[] { internalName });
    }
}