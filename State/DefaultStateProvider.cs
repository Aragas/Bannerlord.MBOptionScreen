using MBOptionScreen.Attributes;
using MBOptionScreen.Interfaces;

using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using Module = TaleWorlds.MountAndBlade.Module;

namespace MBOptionScreen.State
{
    [StateProviderVersion("e1.0.0",  1)]
    [StateProviderVersion("e1.0.1",  1)]
    [StateProviderVersion("e1.0.2",  1)]
    [StateProviderVersion("e1.0.3",  1)]
    [StateProviderVersion("e1.0.4",  1)]
    [StateProviderVersion("e1.0.5",  1)]
    [StateProviderVersion("e1.0.6",  1)]
    [StateProviderVersion("e1.0.7",  1)]
    [StateProviderVersion("e1.0.8",  1)]
    [StateProviderVersion("e1.0.9",  1)]
    [StateProviderVersion("e1.0.10", 1)]
    [StateProviderVersion("e1.0.11", 1)]
    [StateProviderVersion("e1.1.0",  1)]
    public class DefaultStateProvider : IStateProvider
    {
        private static FieldInfo InitialStateOptionsField { get; } =
            typeof(Module).GetField("_initialStateOptions", BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly List<Container> _containers = new List<Container>();

        public T Get<T>() where T : ISharedStateObject
        {
            var fullName = typeof(T).FullName;
            if (Module.CurrentModule.GetInitialStateOptionWithId(fullName) is Container container && container.SharedStateObject is T value)
                return value;
            return default;
        }

        public void Set<T>(T value) where T : ISharedStateObject
        {
            var fullName = typeof(T).FullName;
            if (Module.CurrentModule.GetInitialStateOptionWithId(fullName) is Container container)
                container.SharedStateObject = value;
            else
            {
                container = new Container(value);
                _containers.Add(container);

                // Module._initialStateOptions was used to host the shared class,
                // as the game is using the list only after OnBeforeInitialModuleScreenSetAsRoot() was called
                Module.CurrentModule.AddInitialStateOption(container);
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
            public ISharedStateObject SharedStateObject { get; internal set; }

            public Container(ISharedStateObject sharedStateObject) : base(sharedStateObject.GetType().FullName, new TextObject(""), 1, null, true)
            {
                SharedStateObject = sharedStateObject;
            }
        }
    }
}