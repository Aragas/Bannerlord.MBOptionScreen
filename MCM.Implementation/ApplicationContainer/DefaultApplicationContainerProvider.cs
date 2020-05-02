using MCM.Abstractions.ApplicationContainer;
using MCM.Abstractions.Attributes;

using System.Collections.Concurrent;

namespace MCM.Implementation.ApplicationContainer
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
    internal sealed class DefaultApplicationContainerProvider : IApplicationContainerProvider
    {
        private static readonly ConcurrentDictionary<string, object> _containers = new ConcurrentDictionary<string, object>();

        public object? Get(string name) => _containers.TryGetValue(name, out var value) ? value : default;

        public void Set(string name, object value) => _containers.AddOrUpdate(name, value, (n, v) => v);

        public void Clear() => _containers.Clear();
    }
}