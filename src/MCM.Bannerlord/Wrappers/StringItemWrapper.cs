using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;

namespace MCM.Common.Wrappers
{
    public readonly ref struct StringItemWrapper
    {
        private static readonly ConcurrentDictionary<Type, GetStringItemDelegate?> _getStringItemCache = new();
        private static readonly ConcurrentDictionary<Type, SetStringItemDelegate?> _setStringItemCache = new();
        
        private delegate string GetStringItemDelegate(object instance);
        private delegate void SetStringItemDelegate(object instance, string value);

        private readonly GetStringItemDelegate? _getStringItem;
        private readonly SetStringItemDelegate? _setStringItem;

        private readonly object? _object;

        public string StringItem
        {
            get => _getStringItem?.Invoke(_object!) ?? string.Empty;
            set => _setStringItem?.Invoke(_object!, value);
        }

        public StringItemWrapper(object? @object)
        {
            _object = @object;
            var type = @object?.GetType();

            _getStringItem = type is not null
                ? _getStringItemCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<GetStringItemDelegate>(t, "StringItem"))
                : null;
            _setStringItem = type is not null
                ? _setStringItemCache.GetOrAdd(type, static t => AccessTools2.GetPropertySetterDelegate<SetStringItemDelegate>(t, "StringItem"))
                : null;
        }
    }
}