using MCM.Abstractions.Attributes;
using MCM.Abstractions.Synchronization;

using System.Collections.Concurrent;

namespace MCM.Implementation.Synchronization
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    [Version("e1.3.1",  1)]
    [Version("e1.4.0",  1)]
    internal sealed class DefaultSynchronizationProvider : BaseSynchronizationProvider
    {
        private static readonly ConcurrentDictionary<string, object?> _set = new ConcurrentDictionary<string, object?>();

        public DefaultSynchronizationProvider(string name) : base(name)
        {
            IsFirstInitialization = _set.TryAdd(name, null);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            // Keep the names alive for the whole application lifetime
        }
    }
}