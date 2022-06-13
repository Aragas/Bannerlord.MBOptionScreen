using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct StringItemWrapper
    {
        private delegate string GetStringItemDelegate();
        private delegate void SetStringItemDelegate(string value);

        private readonly GetStringItemDelegate? _getStringItemDelegate;
        private readonly SetStringItemDelegate? _setStringItemDelegate;

        public string StringItem
        {
            get => _getStringItemDelegate?.Invoke() ?? string.Empty;
            set => _setStringItemDelegate?.Invoke(value);
        }

        public StringItemWrapper(object @object)
        {
            var type = @object?.GetType();

            _getStringItemDelegate = type is not null
                ? AccessTools2.GetPropertyGetterDelegate<GetStringItemDelegate>(@object, type, nameof(StringItem))
                : null;
            _setStringItemDelegate = type is not null
                ? AccessTools2.GetPropertySetterDelegate<SetStringItemDelegate>(@object, type, nameof(StringItem))
                : null;
        }
    }
}