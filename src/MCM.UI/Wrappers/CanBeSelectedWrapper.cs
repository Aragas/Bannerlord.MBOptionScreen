using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;

namespace MCM.UI.Wrappers
{
    public readonly ref struct CanBeSelectedWrapper
    {
        private static readonly ConcurrentDictionary<Type, GetCanBeSelectedDelegate?> _getCanBeSelectedCache = new();
        private static readonly ConcurrentDictionary<Type, SetCanBeSelectedDelegate?> _setCanBeSelectedCache = new();

        private delegate bool GetCanBeSelectedDelegate(object instance);
        private delegate void SetCanBeSelectedDelegate(object instance, bool value);

        private readonly GetCanBeSelectedDelegate? _getCanBeSelected;
        private readonly SetCanBeSelectedDelegate? _setCanBeSelected;

        private readonly object? _object;

        public bool CanBeSelected
        {
            get => _getCanBeSelected?.Invoke(_object!) ?? false;
            set => _setCanBeSelected?.Invoke(_object!, value);
        }

        public CanBeSelectedWrapper(object? @object)
        {
            _object = @object;
            var type = @object?.GetType();

            _getCanBeSelected = type is not null
                ? _getCanBeSelectedCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<GetCanBeSelectedDelegate>(t, "CanBeSelected"))
                : null;
            _setCanBeSelected = type is not null
                ? _setCanBeSelectedCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<SetCanBeSelectedDelegate>(t, "CanBeSelected"))
                : null;
        }
    }
}