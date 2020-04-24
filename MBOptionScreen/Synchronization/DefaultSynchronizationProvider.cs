using HarmonyLib;

using MBOptionScreen.Attributes;

using System.Collections;

using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using Module = TaleWorlds.MountAndBlade.Module;

namespace MBOptionScreen.Synchronization
{
    [Version("e1.0.0",  200)]
    [Version("e1.0.1",  200)]
    [Version("e1.0.2",  200)]
    [Version("e1.0.3",  200)]
    [Version("e1.0.4",  200)]
    [Version("e1.0.5",  200)]
    [Version("e1.0.6",  200)]
    [Version("e1.0.7",  200)]
    [Version("e1.0.8",  200)]
    [Version("e1.0.9",  200)]
    [Version("e1.0.10", 200)]
    [Version("e1.0.11", 200)]
    [Version("e1.1.0",  200)]
    [Version("e1.2.0",  200)]
    internal sealed class DefaultSynchronizationProvider : ISynchronizationProvider
    {
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
    }
}