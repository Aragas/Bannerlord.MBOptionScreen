using HarmonyLib.BUTR.Extensions;

namespace MCM.UI.Data
{
    /// <summary>
    /// A complex wrapper that replaces any Selector ViewModel, just needs the right properties
    /// </summary>
    public class SelectorVMWrapper : ViewModelWrapper
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

        public SelectorVMWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            _getSelectedIndexDelegate = AccessTools2.GetPropertyGetterDelegate<GetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex));
            _setSelectedIndexDelegate = AccessTools2.GetPropertySetterDelegate<SetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex));
        }
    }
}