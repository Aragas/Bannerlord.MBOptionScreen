using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct OrderIndexWrapper
    {
        private static readonly ConcurrentDictionary<Type, GetOrderIndexDelegate?> _getOrderIndexCache = new();
        private static readonly ConcurrentDictionary<Type, SetOrderIndexDelegate?> _setOrderIndexCache = new();

        private delegate int GetOrderIndexDelegate(object instance);
        private delegate void SetOrderIndexDelegate(object instance, int value);

        private readonly GetOrderIndexDelegate? _getOrderIndex;
        private readonly SetOrderIndexDelegate? _setOrderIndex;
        
        private readonly object? _object;
        
        public int OrderIndex
        {
            get => _getOrderIndex?.Invoke(_object!) ?? -1;
            set => _setOrderIndex?.Invoke(_object!, value);
        }

        public OrderIndexWrapper(object? @object)
        {
            _object = @object;
            var type = @object?.GetType();

            _getOrderIndex = type is not null
                ? _getOrderIndexCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<GetOrderIndexDelegate>(t, "OrderIndex"))
                : null;
            _setOrderIndex = type is not null
                ? _setOrderIndexCache.GetOrAdd(type, static t => AccessTools2.GetPropertySetterDelegate<SetOrderIndexDelegate>(t, "OrderIndex"))
                : null;
        }
    }
}