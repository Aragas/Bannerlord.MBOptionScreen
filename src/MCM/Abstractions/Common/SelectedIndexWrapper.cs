using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Common
{
    public sealed class SelectedIndexWrapper : IWrapper
    {
        private delegate int GetSelectedIndexDelegate();
        private delegate void SetSelectedIndexDelegate(int value);

        private readonly GetSelectedIndexDelegate _getSelectedIndexDelegate;
        private readonly SetSelectedIndexDelegate? _setSelectedIndexDelegate;

        /// <inheritdoc/>
        public object Object { get; }

        public int SelectedIndex
        {
            get => _getSelectedIndexDelegate.Invoke();
            set => _setSelectedIndexDelegate?.Invoke(value);
        }

        public SelectedIndexWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            _getSelectedIndexDelegate = AccessTools2.GetPropertyGetterDelegate<GetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex));
            _setSelectedIndexDelegate = AccessTools2.GetPropertySetterDelegate<SetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex));
        }
    }
}