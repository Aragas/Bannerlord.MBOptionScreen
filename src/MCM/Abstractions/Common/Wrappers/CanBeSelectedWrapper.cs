using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct CanBeSelectedWrapper
    {
        private delegate bool GetCanBeSelectedDelegate();
        private delegate void SetCanBeSelectedDelegate(bool value);

        private readonly GetCanBeSelectedDelegate? _getCanBeSelectedDelegate;
        private readonly SetCanBeSelectedDelegate? _setCanBeSelectedDelegate;

        public bool CanBeSelected
        {
            get => _getCanBeSelectedDelegate?.Invoke() ?? false;
            set => _setCanBeSelectedDelegate?.Invoke(value);
        }

        public CanBeSelectedWrapper(object @object)
        {
            var type = @object?.GetType();

            _getCanBeSelectedDelegate = type is not null
                ? AccessTools2.GetPropertyGetterDelegate<GetCanBeSelectedDelegate>(@object, type, nameof(CanBeSelected))
                : null;
            _setCanBeSelectedDelegate = type is not null
                ? AccessTools2.GetPropertySetterDelegate<SetCanBeSelectedDelegate>(@object, type, nameof(CanBeSelected))
                : null;
        }
    }
}