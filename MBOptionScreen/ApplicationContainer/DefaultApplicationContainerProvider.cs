using HarmonyLib;

using MBOptionScreen.Attributes;

using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using Module = TaleWorlds.MountAndBlade.Module;

namespace MBOptionScreen.ApplicationContainer
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
    internal sealed class DefaultApplicationContainerProvider : IApplicationContainerProvider
    {
        private static FieldInfo InitialStateOptionsField { get; } = AccessTools.Field(typeof(Module), "_initialStateOptions");

        private readonly List<Container> _containers = new List<Container>();

        public object Get(string name)
        {
            if (Module.CurrentModule.GetInitialStateOptionWithId(name) is Container container)
                return container.Value;
            return default;
        }

        public void Set(string name, object value)
        {
            if (Module.CurrentModule.GetInitialStateOptionWithId(name) is Container container)
                container.Value = value;
            else
            {
                container = new Container(name, value);

                // Module._initialStateOptions was used to host the shared class,
                // as the game is using the list only after OnBeforeInitialModuleScreenSetAsRoot() was called
                Module.CurrentModule.AddInitialStateOption(container);
                _containers.Add(container);
            }
        }

        public void Clear()
        {
            var list = (IList) InitialStateOptionsField.GetValue(Module.CurrentModule);
            foreach (var container in _containers)
                list.Remove(container);
            _containers.Clear();
        }

        private class Container : InitialStateOption
        {
            public object Value { get; internal set; }

            public Container(string name, object value) : base(name, new TextObject(""), 1, () => { }, true)
            {
                Value = value;
            }
        }
    }
}