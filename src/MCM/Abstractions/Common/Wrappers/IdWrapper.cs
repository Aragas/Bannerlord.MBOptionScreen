using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct IdWrapper
    {
        private delegate string GetIdDelegate();
        private delegate void SetIdDelegate(string value);

        private readonly GetIdDelegate? _getIdDelegate;
        private readonly SetIdDelegate? _setIdDelegate;

        public string Id
        {
            get => _getIdDelegate?.Invoke() ?? string.Empty;
            set => _setIdDelegate?.Invoke(value);
        }

        public IdWrapper(object @object)
        {
            var type = @object?.GetType();

            _getIdDelegate = type is not null
                ? AccessTools2.GetPropertyGetterDelegate<GetIdDelegate>(@object, type, nameof(Id))
                : null;
            _setIdDelegate = type is not null
                ? AccessTools2.GetPropertySetterDelegate<SetIdDelegate>(@object, type, nameof(Id))
                : null;
        }
    }
}