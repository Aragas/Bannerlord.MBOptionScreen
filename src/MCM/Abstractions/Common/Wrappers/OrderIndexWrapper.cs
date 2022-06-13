using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct OrderIndexWrapper
    {
        private delegate int GetOrderIndexDelegate();
        private delegate void SetOrderIndexDelegate(int value);

        private readonly GetOrderIndexDelegate? _getOrderIndexDelegate;
        private readonly SetOrderIndexDelegate? _setOrderIndexDelegate;

        public int OrderIndex
        {
            get => _getOrderIndexDelegate?.Invoke() ?? -1;
            set => _setOrderIndexDelegate?.Invoke(value);
        }

        public OrderIndexWrapper(object @object)
        {
            var type = @object?.GetType();

            _getOrderIndexDelegate = type is not null
                ? AccessTools2.GetPropertyGetterDelegate<GetOrderIndexDelegate>(@object, type, nameof(OrderIndex))
                : null;
            _setOrderIndexDelegate = type is not null
                ? AccessTools2.GetPropertySetterDelegate<SetOrderIndexDelegate>(@object, type, nameof(OrderIndex))
                : null;
        }
    }
}