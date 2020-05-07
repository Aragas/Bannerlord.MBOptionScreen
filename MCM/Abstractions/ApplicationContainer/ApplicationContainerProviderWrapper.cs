using HarmonyLib;

using System;
using System.Reflection;

namespace MCM.Abstractions.ApplicationContainer
{
    public sealed class ApplicationContainerProviderWrapper : IApplicationContainerProvider, IWrapper
    {
        public object Object { get; }
        private MethodInfo? GetMethod { get; }
        private MethodInfo? SetMethod { get; }
        private MethodInfo? ClearMethod { get; }
        public bool IsCorrect { get; }

        public ApplicationContainerProviderWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            GetMethod = AccessTools.Method(type, nameof(Get));
            SetMethod = AccessTools.Method(type, nameof(Set));
            ClearMethod = AccessTools.Method(type, nameof(Clear));

            IsCorrect = GetMethod != null && SetMethod != null && ClearMethod != null;
        }

        public object? Get(string name) => GetMethod?.Invoke(Object, new object[] { name });
        public void Set(string name, object value) => SetMethod?.Invoke(Object, new object[] { name, value });
        public void Clear() => ClearMethod?.Invoke(Object, Array.Empty<object>());
    }
}