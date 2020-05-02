using HarmonyLib;

using MCM.Abstractions;

using System;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Implementation.Functionality.Harmony
{
    internal sealed class HarmonyPatchesWrapper : BaseHarmonyPatches, IWrapper
    {
        private readonly object _object;
        private PropertyInfo GetEscapeMenuItemsProperty { get; }
        private MethodInfo AddScreenMethod { get; }
        public bool IsCorrect { get; }

        public override HarmonyMethod? GetEscapeMenuItems => GetEscapeMenuItemsProperty.GetValue(_object) as HarmonyMethod;

        public HarmonyPatchesWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            GetEscapeMenuItemsProperty = AccessTools.Property(type, nameof(BaseHarmonyPatches.GetEscapeMenuItems));
            AddScreenMethod = AccessTools.Method(type, nameof(BaseHarmonyPatches.AddScreen));

            IsCorrect = GetEscapeMenuItemsProperty != null && AddScreenMethod != null;
        }

        public override void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text) =>
            AddScreenMethod.Invoke(_object, new object[] { index, screenFactory, text });
    }
}