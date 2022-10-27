using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;

using TaleWorlds.Library;

namespace MCM.UI.Wrappers
{
    public readonly ref struct MenuItemsWrapper
    {
        private static readonly ConcurrentDictionary<Type, GetMenuItemsDelegate?> _getMenuItemsCache = new();
        private static readonly ConcurrentDictionary<Type, SetMenuItemsDelegate?> _setMenuItemsCache = new();

        private delegate IMBBindingList GetMenuItemsDelegate(object instance);
        private delegate void SetMenuItemsDelegate(object instance, IMBBindingList? value);

        private readonly GetMenuItemsDelegate? _getMenuItems;
        private readonly SetMenuItemsDelegate? _setMenuItems;

        private readonly object? _object;

        public IMBBindingList? MenuItems
        {
            get => _getMenuItems?.Invoke(_object!);
            set => _setMenuItems?.Invoke(_object!, value);
        }

        public MenuItemsWrapper(object? @object)
        {
            _object = @object;
            var type = @object?.GetType();

            _getMenuItems = type is not null
                ? _getMenuItemsCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<GetMenuItemsDelegate>(t, "MenuItems"))
                : null;
            _setMenuItems = type is not null
                ? _setMenuItemsCache.GetOrAdd(type, static t => AccessTools2.GetPropertySetterDelegate<SetMenuItemsDelegate>(t, "MenuItems"))
                : null;
        }
    }
}
