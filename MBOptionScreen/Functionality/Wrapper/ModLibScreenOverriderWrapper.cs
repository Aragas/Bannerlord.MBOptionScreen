using HarmonyLib;

using System;
using System.Reflection;

namespace MBOptionScreen.Functionality
{
    internal sealed class ModLibScreenOverriderWrapper : IModLibScreenOverrider
    {
        private readonly object _object;

        private MethodInfo OverrideModLibScreenMethod { get; }

        public ModLibScreenOverriderWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            OverrideModLibScreenMethod = AccessTools.Method(type, "OverrideModLibScreen");
        }

        public void OverrideModLibScreen() => OverrideModLibScreenMethod.Invoke(_object, Array.Empty<object>());
    }
}