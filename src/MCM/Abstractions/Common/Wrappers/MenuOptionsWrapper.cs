using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Library;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct MenuOptionsWrapper
    {
        private delegate IMBBindingList GetMenuOptionsDelegate();
        private delegate void SetMenuOptionsDelegate(IMBBindingList value);

        private readonly GetMenuOptionsDelegate? _getMenuOptionsDelegate;
        private readonly SetMenuOptionsDelegate? _setMenuOptionsDelegate;

        public IMBBindingList MenuOptions
        {
            get => _getMenuOptionsDelegate?.Invoke() ?? default!;
            set => _setMenuOptionsDelegate?.Invoke(value);
        }

        public MenuOptionsWrapper(object @object)
        {
            var type = @object?.GetType();

            _getMenuOptionsDelegate = type is not null
                ? AccessTools2.GetPropertyGetterDelegate<GetMenuOptionsDelegate>(@object, type, nameof(MenuOptions))
                : null;
            _setMenuOptionsDelegate = type is not null
                ? AccessTools2.GetPropertySetterDelegate<SetMenuOptionsDelegate>(@object, type, nameof(MenuOptions))
                : null;
        }
    }
}