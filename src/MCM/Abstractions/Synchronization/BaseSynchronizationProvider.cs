using System;

namespace MCM.Abstractions.Synchronization
{
    public abstract class BaseSynchronizationProvider : IDependency, IDisposable
    {
        public static BaseSynchronizationProvider Create(string name) => null!;


        public virtual string Name { get; } = "";
        public virtual bool IsFirstInitialization { get; protected set; }

        internal BaseSynchronizationProvider() { }
        protected BaseSynchronizationProvider(string name) => Name = name;

        public abstract void Dispose();
    }
}