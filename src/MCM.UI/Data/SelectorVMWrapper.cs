using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

namespace MCM.UI.Data
{
    /// <summary>
    /// A complex wrapper that replaces any Selector ViewModel, just needs the right properties
    /// </summary>
    public class SelectorVMWrapper : ViewModelWrapper
    {
        private delegate int GetSelectedIndexDelegate();
        private delegate void SetSelectedIndexDelegate(int value);

        private readonly GetSelectedIndexDelegate _getSelectedIndexDelegate;
        private readonly SetSelectedIndexDelegate _setSelectedIndexDelegate;

        public int SelectedIndex
        {
            get => _getSelectedIndexDelegate.Invoke();
            set => _setSelectedIndexDelegate.Invoke(value);
        }

        public SelectorVMWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            var selectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));
            _getSelectedIndexDelegate = AccessTools2.GetDelegate<GetSelectedIndexDelegate>(@object, selectedIndexProperty.GetMethod)!;
            _setSelectedIndexDelegate = AccessTools2.GetDelegate<SetSelectedIndexDelegate>(@object, selectedIndexProperty.SetMethod)!;
        }
    }
}