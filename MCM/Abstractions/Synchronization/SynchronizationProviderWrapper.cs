using HarmonyLib;

using System;
using System.Reflection;

namespace MCM.Abstractions.Synchronization
{
    public sealed class SynchronizationProviderWrapper : ISynchronizationProvider, IWrapper
    {
        public object Object { get; }
        private PropertyInfo? NameProperty { get; }
        private PropertyInfo? IsFirstInitializationProperty { get; }
        private MethodInfo? DisposeMethod { get; }
        public bool IsCorrect { get; }

        public SynchronizationProviderWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            NameProperty = AccessTools.Property(type, nameof(Name));
            IsFirstInitializationProperty = AccessTools.Property(type, nameof(IsFirstInitialization));
            DisposeMethod = AccessTools.Method(type, nameof(Dispose));

            IsCorrect = NameProperty != null && IsFirstInitializationProperty != null && DisposeMethod != null;
        }

        public string Name => NameProperty?.GetValue(Object) as string ?? "ERROR";
        public bool IsFirstInitialization => IsFirstInitializationProperty?.GetValue(Object) as bool? ?? false;
        public void Dispose() => DisposeMethod?.Invoke(Object, Array.Empty<object>());
    }
}