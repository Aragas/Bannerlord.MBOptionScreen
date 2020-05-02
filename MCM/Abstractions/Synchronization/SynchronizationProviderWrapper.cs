using HarmonyLib;

using System;
using System.Reflection;

namespace MCM.Abstractions.Synchronization
{
    public sealed class SynchronizationProviderWrapper : ISynchronizationProvider, IWrapper
    {
        private readonly object _object;
        private PropertyInfo NameProperty { get; }
        private PropertyInfo IsFirstInitializationProperty { get; }
        private MethodInfo DisposeMethod { get; }
        public bool IsCorrect { get; }

        public SynchronizationProviderWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            NameProperty = AccessTools.Property(type, nameof(Name));
            IsFirstInitializationProperty = AccessTools.Property(type, nameof(IsFirstInitialization));
            DisposeMethod = AccessTools.Method(type, nameof(Dispose));

            IsCorrect = NameProperty != null && IsFirstInitializationProperty != null && DisposeMethod != null;
        }

        public string Name => NameProperty.GetValue(_object) as string ?? "ERROR";
        public bool IsFirstInitialization => IsFirstInitializationProperty.GetValue(_object) as bool? ?? false;
        public void Dispose() => DisposeMethod.Invoke(_object, Array.Empty<object>());
    }
}