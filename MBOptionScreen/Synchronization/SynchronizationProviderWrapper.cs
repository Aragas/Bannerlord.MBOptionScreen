using HarmonyLib;

using System;
using System.Reflection;

namespace MBOptionScreen.Synchronization
{
    internal sealed class SynchronizationProviderWrapper : ISynchronizationProvider
    {
        private readonly object _object;

        private PropertyInfo NameProperty { get; }
        private PropertyInfo IsFirstInitializationProperty { get; }
        private MethodInfo DisposeMethod { get; }

        public string Name => NameProperty.GetValue(_object) as string ?? "ERROR";
        public bool IsFirstInitialization => IsFirstInitializationProperty.GetValue(_object) as bool? ?? false;

        public SynchronizationProviderWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            NameProperty = AccessTools.Property(type, "Name");
            IsFirstInitializationProperty = AccessTools.Property(type, "IsFirstInitialization");
            DisposeMethod = AccessTools.Method(type, "Dispose");
        }

        public void Dispose() => DisposeMethod.Invoke(_object, Array.Empty<object>());
    }
}