using System;

namespace MCM.Abstractions.Synchronization
{
    public interface ISynchronizationProvider : IDisposable
    {
        string Name { get; }
        bool IsFirstInitialization { get; }
    }
}