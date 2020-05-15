using HarmonyLib;

using MCM.Abstractions;

using System;
using System.Reflection;

namespace MCM.Implementation.ModLib.Functionality
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class ModLibScreenOverriderWrapper : BaseModLibScreenOverrider, IWrapper
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

        public override void OverrideModLibScreen() => OverrideModLibScreenMethod?.Invoke(Object, Array.Empty<object>());
    }
}