using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Common.Wrappers
{
    public readonly ref struct SelectedIndexWrapper
    {
        private delegate int GetSelectedIndexDelegate();
        private delegate void SetSelectedIndexDelegate(int value);

        private readonly GetSelectedIndexDelegate? _getSelectedIndexDelegate;
        private readonly SetSelectedIndexDelegate? _setSelectedIndexDelegate;

        public int SelectedIndex
        {
            get => _getSelectedIndexDelegate?.Invoke() ?? -1;
            set => _setSelectedIndexDelegate?.Invoke(value);
        }

        public SelectedIndexWrapper(object @object)
        {
            var type = @object?.GetType();

            _getSelectedIndexDelegate = type is not null
                ? AccessTools2.GetPropertyGetterDelegate<GetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex))
                : null;
            _setSelectedIndexDelegate = type is not null
                ? AccessTools2.GetPropertySetterDelegate<SetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex))
                : null;
        }
    }
}