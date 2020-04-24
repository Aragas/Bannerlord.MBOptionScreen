using HarmonyLib;

using MBOptionScreen.Attributes;

using System.Collections;
using System.Collections.Generic;

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
        private static readonly AccessTools.FieldRef<Module, IList> _initialStateOptions =
            AccessTools.FieldRefAccess<Module, IList>("_initialStateOptions");

        private readonly List<Container> _containers = new List<Container>();

        public object Get(string name)
        {
            if (Module.CurrentModule.GetInitialStateOptionWithId(name) is object obj)
            {
                if (!(obj is Container container))
                {
                    var wrapper = new ContainerWrapper(obj);
                    if (!wrapper.IsCorrect)
                        return default;
                    container = wrapper;
                }

                return container.Value;
            }
            return default;
        }

        public void Set(string name, object value)
        {
            if (Module.CurrentModule.GetInitialStateOptionWithId(name) is object obj)
            {
                if (!(obj is Container container))
                {
                    var wrapper = new ContainerWrapper(obj);
                    if (!wrapper.IsCorrect)
                        return;
                    container = wrapper;
                }

                container.Value = value;
            }
            else
            {
                var container = new Container(name, value);
                Module.CurrentModule.AddInitialStateOption(container);
                _containers.Add(container);
            }
        }

        public void Clear()
        {
            var list = _initialStateOptions(Module.CurrentModule);
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
        private class ContainerWrapper : Container, IWrapper
        {
            private static string? GetName(object @object)
            {
                var propInfo = @object.GetType().GetProperty("Id");
                return propInfo?.GetValue(@object) as string;
            }
            private static object? GetValue(object @object)
            {
                var propInfo = @object.GetType().GetProperty("Value");
                return propInfo?.GetValue(@object) as object;
            }

            public bool IsCorrect { get; }

            public ContainerWrapper(object @object) : base(GetName(@object) ?? "ERROR", GetValue(@object))
            {
                IsCorrect = Id != "ERROR" && @object.GetType().GetProperty("Value") != null;
            }
        }
    }
}