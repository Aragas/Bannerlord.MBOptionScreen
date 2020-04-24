using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MBOptionScreen.Functionality
{
    internal sealed class MapScreenPatchesWrapper : BaseMapScreenPatches, IWrapper
    {
        private readonly object _object;
        private PropertyInfo GetEscapeMenuItemsPostfixProperty { get; }
        private MethodInfo AddScreenMethod { get; }
        public bool IsCorrect { get; }

        public override HarmonyMethod? GetEscapeMenuItemsPostfix => GetEscapeMenuItemsPostfixProperty.GetValue(_object) as HarmonyMethod;

        public MapScreenPatchesWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            GetEscapeMenuItemsPostfixProperty = AccessTools.Property(type, "GetEscapeMenuItemsPostfix");
            AddScreenMethod = AccessTools.Method(type, "AddScreen");

            IsCorrect = GetEscapeMenuItemsPostfixProperty != null && AddScreenMethod != null;
        }

        public override void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text) =>
            AddScreenMethod.Invoke(_object, new object[] { index, screenFactory, text });
    }
}