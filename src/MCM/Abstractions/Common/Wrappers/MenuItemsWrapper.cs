using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Library;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct MenuItemsWrapper
    {
        private delegate IMBBindingList GetMenuItemsDelegate();
        private delegate void SetMenuItemsDelegate(IMBBindingList value);

        private readonly GetMenuItemsDelegate? _getMenuItemsDelegate;
        private readonly SetMenuItemsDelegate? _setMenuItemsDelegate;

        public IMBBindingList MenuItems
        {
            get => _getMenuItemsDelegate?.Invoke() ?? default!;
            set => _setMenuItemsDelegate?.Invoke(value);
        }

        public MenuItemsWrapper(object @object)
        {
            var type = @object?.GetType();

            _getMenuItemsDelegate = type is not null
                ? AccessTools2.GetPropertyGetterDelegate<GetMenuItemsDelegate>(@object, type, nameof(MenuItems))
                : null;
            _setMenuItemsDelegate = type is not null
                ? AccessTools2.GetPropertySetterDelegate<SetMenuItemsDelegate>(@object, type, nameof(MenuItems))
                : null;
        }
    }
}