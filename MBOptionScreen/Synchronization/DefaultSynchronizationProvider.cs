using MBOptionScreen.Attributes;

using System.Collections.Concurrent;

namespace MBOptionScreen.Synchronization
{
    [Version("e1.0.0",  202)]
    [Version("e1.0.1",  202)]
    [Version("e1.0.2",  202)]
    [Version("e1.0.3",  202)]
    [Version("e1.0.4",  202)]
    [Version("e1.0.5",  202)]
    [Version("e1.0.6",  202)]
    [Version("e1.0.7",  202)]
    [Version("e1.0.8",  202)]
    [Version("e1.0.9",  202)]
    [Version("e1.0.10", 202)]
    [Version("e1.0.11", 202)]
    [Version("e1.1.0",  202)]
    [Version("e1.2.0",  202)]
    [Version("e1.2.1",  202)]
    [Version("e1.3.0",  202)]
    internal sealed class DefaultSynchronizationProvider : ISynchronizationProvider
    {
        private static readonly ConcurrentDictionary<string, object> _set = new ConcurrentDictionary<string, object>();

        public string Name { get; }
        public bool IsFirstInitialization { get; }

        public DefaultSynchronizationProvider(string name)
        {
            Name = name;
            IsFirstInitialization = _set.TryAdd(name, null);
        }
        public void Dispose()
        {
            // Keep the names alive for the whole application lifetime
        }
    }
}