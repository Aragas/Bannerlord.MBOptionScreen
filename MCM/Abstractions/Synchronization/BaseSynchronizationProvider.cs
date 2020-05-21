using System;

using MCM.Utils;

namespace MCM.Abstractions.Synchronization
{
    public abstract class BaseSynchronizationProvider : IDependency, IDisposable
    {
        private static BaseSynchronizationProvider? _instance;
        public static BaseSynchronizationProvider Create(string name) =>
            _instance ??= DI.GetImplementation<BaseSynchronizationProvider, SynchronizationProviderWrapper>(args: name)!;


        public virtual string Name { get; } = "";
        public virtual bool IsFirstInitialization { get; protected set; }

        internal BaseSynchronizationProvider() { }
        protected BaseSynchronizationProvider(string name) => Name = name;

        public abstract void Dispose();
    }
}