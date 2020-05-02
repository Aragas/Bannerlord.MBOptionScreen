using HarmonyLib;

using System;
using System.Reflection;

namespace MCM.Abstractions.Functionality.Wrapper
{
    public sealed class ModLibScreenOverriderWrapper : IModLibScreenOverrider, IWrapper
    {
        private readonly object _object;
        private MethodInfo? OverrideModLibScreenMethod { get; }
        public bool IsCorrect { get; }

        public ModLibScreenOverriderWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            OverrideModLibScreenMethod = AccessTools.Method(type, nameof(OverrideModLibScreen));

            IsCorrect = OverrideModLibScreenMethod != null;
        }

        public void OverrideModLibScreen() => OverrideModLibScreenMethod?.Invoke(_object, Array.Empty<object>());
    }
}