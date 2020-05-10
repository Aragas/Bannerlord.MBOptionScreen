using System;

using MCM.Utils;

namespace MCM.Abstractions.Synchronization
{
    public abstract class BaseSynchronizationProvider : IDisposable
    {
        private static BaseSynchronizationProvider? _instance;
        public static BaseSynchronizationProvider Create(string name) =>
            _instance ??= DI.GetImplementation<BaseSynchronizationProvider, SynchronizationProviderWrapper>(ApplicationVersionUtils.GameVersion(), name)!;


        public string Name { get; internal set; }
        public bool IsFirstInitialization { get; protected set; }

        internal BaseSynchronizationProvider() { }
        protected BaseSynchronizationProvider(string name) => Name = name;

        protected virtual void Dispose(bool disposing) { }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}