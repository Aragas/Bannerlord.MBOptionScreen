using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;

using TaleWorlds.Library;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct MenuOptionsWrapper
    {
        private static readonly ConcurrentDictionary<Type, GetMenuOptionsDelegate?> _getMenuOptionsCache = new();
        private static readonly ConcurrentDictionary<Type, SetMenuOptionsDelegate?> _setMenuOptionsCache = new();

        private delegate IMBBindingList GetMenuOptionsDelegate(object instance);
        private delegate void SetMenuOptionsDelegate(object instance, IMBBindingList? value);

        private readonly GetMenuOptionsDelegate? _getMenuOptions;
        private readonly SetMenuOptionsDelegate? _setMenuOptions;

        private readonly object? _object;

        public IMBBindingList? MenuOptions
        {
            get => _getMenuOptions?.Invoke(_object!);
            set => _setMenuOptions?.Invoke(_object!, value);
        }

        public MenuOptionsWrapper(object? @object)
        {
            _object = @object;
            var type = @object?.GetType();

            _getMenuOptions = type is not null
                ? _getMenuOptionsCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<GetMenuOptionsDelegate>(t, "MenuOptions"))
                : null;
            _setMenuOptions = type is not null
                ? _setMenuOptionsCache.GetOrAdd(type, static t => AccessTools2.GetPropertySetterDelegate<SetMenuOptionsDelegate>(t, "MenuOptions"))
                : null;
        }
    }
}