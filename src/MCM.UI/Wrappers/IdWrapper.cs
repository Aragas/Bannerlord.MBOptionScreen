using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;

namespace MCM.UI.Wrappers
{
    public readonly ref struct IdWrapper
    {
        private static readonly ConcurrentDictionary<Type, GetIdDelegate?> _getIdCache = new();
        private static readonly ConcurrentDictionary<Type, SetIdDelegate?> _setIdCache = new();

        private delegate string GetIdDelegate(object instance);
        private delegate void SetIdDelegate(object instance, string value);

        private readonly GetIdDelegate? _getId;
        private readonly SetIdDelegate? _setId;

        private readonly object? _object;

        public string Id
        {
            get => _getId?.Invoke(_object!) ?? string.Empty;
            set => _setId?.Invoke(_object!, value);
        }

        public IdWrapper(object? @object)
        {
            _object = @object;
            var type = @object?.GetType();

            _getId = type is not null
                ? _getIdCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<GetIdDelegate>(t, "Id"))
                : null;
            _setId = type is not null
                ? _setIdCache.GetOrAdd(type, static t => AccessTools2.GetPropertySetterDelegate<SetIdDelegate>(t, "Id"))
                : null;
        }
    }
}