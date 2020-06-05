using MCM.Utils;

using System;
using System.Collections.Concurrent;

namespace MCM.Abstractions.Synchronization
{
    public abstract class BaseSynchronizationProvider : IDependency, IDisposable
    {
        private static readonly ConcurrentDictionary<string, BaseSynchronizationProvider> _instances =
            new ConcurrentDictionary<string, BaseSynchronizationProvider>();
        public static BaseSynchronizationProvider Create(string name) =>
            _instances.GetOrAdd(name, n => DI.GetImplementation<BaseSynchronizationProvider, SynchronizationProviderWrapper>(n)!);


        public virtual string Name { get; } = "";
        public virtual bool IsFirstInitialization { get; protected set; }

        internal BaseSynchronizationProvider() { }
        protected BaseSynchronizationProvider(string name) => Name = name;

        public abstract void Dispose();
    }
}