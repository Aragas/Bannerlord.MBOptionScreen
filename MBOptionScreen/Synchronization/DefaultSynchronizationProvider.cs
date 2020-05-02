using System.Collections;
using MBOptionScreen.Attributes;

using System.Collections.Concurrent;
using HarmonyLib;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MBOptionScreen.Synchronization
{
    [Version("e1.0.0",  220)]
    [Version("e1.0.1",  220)]
    [Version("e1.0.2",  220)]
    [Version("e1.0.3",  220)]
    [Version("e1.0.4",  220)]
    [Version("e1.0.5",  220)]
    [Version("e1.0.6",  220)]
    [Version("e1.0.7",  220)]
    [Version("e1.0.8",  220)]
    [Version("e1.0.9",  220)]
    [Version("e1.0.10", 220)]
    [Version("e1.0.11", 220)]
    [Version("e1.1.0",  220)]
    [Version("e1.2.0",  220)]
    [Version("e1.2.1",  220)]
    [Version("e1.3.0",  220)]
    internal sealed class DefaultSynchronizationProvider : ISynchronizationProvider
    {
        /*
        private static readonly AccessTools.FieldRef<Module, IList> _initialStateOptions =
            AccessTools.FieldRefAccess<Module, IList>("_initialStateOptions");

        public string Name { get; }
        public bool IsFirstInitialization { get; }

        public DefaultSynchronizationProvider(string name)
        {
            Name = name;

            if (Module.CurrentModule.GetInitialStateOptionWithId(name) is Container container && container.Id == name)
                IsFirstInitialization = false;
            else
            {
                Module.CurrentModule.AddInitialStateOption(new Container(name));
                IsFirstInitialization = true;
            }
        }

        private class Container : InitialStateOption
        {
            public Container(string name) : base(name, new TextObject(""), 1, () => { }, true) { }
        }

        public void Dispose()
        {
            var container = Module.CurrentModule.GetInitialStateOptionWithId(Name);
            _initialStateOptions(Module.CurrentModule).Remove(container);
        }
        */

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