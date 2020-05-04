using HarmonyLib;

using System;
using System.Reflection;

namespace MCM.Abstractions.Functionality.Wrapper
{
    public sealed class ModLibScreenOverriderWrapper : IModLibScreenOverrider, IWrapper
    {
        public object Object { get; }
        private MethodInfo? OverrideModLibScreenMethod { get; }
        public bool IsCorrect { get; }

        public ModLibScreenOverriderWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            OverrideModLibScreenMethod = AccessTools.Method(type, nameof(OverrideModLibScreen));

            IsCorrect = OverrideModLibScreenMethod != null;
        }

        public void OverrideModLibScreen() => OverrideModLibScreenMethod?.Invoke(Object, Array.Empty<object>());
    }
}