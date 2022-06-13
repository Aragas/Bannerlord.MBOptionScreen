using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct SelectorWrapper
    {
        private static readonly ConcurrentDictionary<Type, GetSelectorDelegate?> _getSelectorCache = new();
        private static readonly ConcurrentDictionary<Type, SetSelectorDelegate?> _setSelectorCache = new();
        
        private delegate object GetSelectorDelegate(object instance);
        private delegate void SetSelectorDelegate(object instance, object? value);

        private readonly GetSelectorDelegate? _getSelector;
        private readonly SetSelectorDelegate? _setSelector;

        private readonly object? _object;
        
        public object? Selector
        {
            get => _getSelector?.Invoke(_object!);
            set => _setSelector?.Invoke(_object!, value);
        }

        public SelectorWrapper(object? @object)
        {
            _object = @object;
            var type = @object?.GetType();

            _getSelector = type is not null
                ? _getSelectorCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<GetSelectorDelegate>(t, "Selector"))
                : null;
            _setSelector = type is not null
                ? _setSelectorCache.GetOrAdd(type, static t => AccessTools2.GetPropertySetterDelegate<SetSelectorDelegate>(t, "Selector"))
                : null;
        }
    }
}