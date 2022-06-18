using MCM.Abstractions.Common.ViewModelWrappers;
using MCM.Abstractions.Common.Wrappers;
using MCM.UI.Actions;

using System.ComponentModel;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        private SelectorVMWrapper? _selectorVMWrapper;
        
        [DataSourceProperty]
        public bool IsDropdown { get; }
        [DataSourceProperty]
        public bool IsDropdownDefault { get; }
        [DataSourceProperty]
        public bool IsDropdownCheckbox { get; }

        [DataSourceProperty]
        public SelectorVMWrapper DropdownValue => _selectorVMWrapper ??= new SelectorVMWrapper(new SelectorWrapper(PropertyReference.Value).Selector);

        private void DropdownValue_PropertyChanged(object? obj, PropertyChangedEventArgs args)
        {
            if (obj is not null && args.PropertyName == "SelectedIndex")
            {
                URS.Do(new SetSelectedIndexAction(PropertyReference, obj));
                SettingsVM.RecalculateIndex();
            }
        }
        private void DropdownValue_PropertyChangedWithValue(object obj, PropertyChangedWithValueEventArgs args)
        {
            if (args.PropertyName == "SelectedIndex")
            {
                URS.Do(new SetSelectedIndexAction(PropertyReference, obj));
                SettingsVM.RecalculateIndex();
            }
        }
    }
}