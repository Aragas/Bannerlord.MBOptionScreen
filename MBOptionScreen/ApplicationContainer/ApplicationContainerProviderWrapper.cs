using HarmonyLib;

using System;
using System.Reflection;

namespace MBOptionScreen.ApplicationContainer
{
    internal sealed class ApplicationContainerProviderWrapper : IApplicationContainerProvider, IWrapper
    {
        private readonly object _object;
        private MethodInfo GetMethod { get; }
        private MethodInfo SetMethod { get; }
        private MethodInfo ClearMethod { get; }
        public bool IsCorrect { get; }

        public ApplicationContainerProviderWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            GetMethod = AccessTools.Method(type, "Get");
            SetMethod = AccessTools.Method(type, "Set");
            ClearMethod = AccessTools.Method(type, "Clear");

            IsCorrect = GetMethod != null && SetMethod != null && ClearMethod != null;
        }

        public object Get(string name) => GetMethod.Invoke(_object, new object[] { name });
        public void Set(string name, object value) => SetMethod.Invoke(_object, new object[] { name, value });
        public void Clear() => ClearMethod.Invoke(_object, Array.Empty<object>());
    }
}