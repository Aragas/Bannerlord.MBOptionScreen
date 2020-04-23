using System;

namespace MBOptionScreen.Synchronization
{
    public interface ISynchronizationProvider : IDisposable
    {
        string Name { get; }
        bool IsFirstInitialization { get; }
    }
}