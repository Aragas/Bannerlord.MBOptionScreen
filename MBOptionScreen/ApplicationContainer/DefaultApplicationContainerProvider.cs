using MBOptionScreen.Attributes;

using System.Collections.Concurrent;

namespace MBOptionScreen.ApplicationContainer
{
    [Version("e1.0.0",  201)]
    [Version("e1.0.1",  201)]
    [Version("e1.0.2",  201)]
    [Version("e1.0.3",  201)]
    [Version("e1.0.4",  201)]
    [Version("e1.0.5",  201)]
    [Version("e1.0.6",  201)]
    [Version("e1.0.7",  201)]
    [Version("e1.0.8",  201)]
    [Version("e1.0.9",  201)]
    [Version("e1.0.10", 201)]
    [Version("e1.0.11", 201)]
    [Version("e1.1.0",  201)]
    [Version("e1.2.0",  201)]
    [Version("e1.2.1",  201)]
    [Version("e1.3.0",  201)]
    internal sealed class DefaultApplicationContainerProvider : IApplicationContainerProvider
    {
        private static readonly ConcurrentDictionary<string, object> _containers = new ConcurrentDictionary<string, object>();

        public object Get(string name)
        {
            return _containers.TryGetValue(name, out var value) ? value : default;
        }

        public void Set(string name, object value)
        {
            _containers.AddOrUpdate(name, value, (n, v) => v);
        }

        public void Clear()
        {
            _containers.Clear();
        }
    }
}