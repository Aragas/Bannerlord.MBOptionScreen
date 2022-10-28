using MCM.Common;
using MCM.UI.Actions;
using MCM.UI.Dropdown;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        private MCMSelectorVM<DropdownSelectorItemVM, string>? _selectorVMWrapper;

        [DataSourceProperty]
        public bool IsDropdown { get; }
        [DataSourceProperty]
        public bool IsDropdownDefault { get; }
        [DataSourceProperty]
        public bool IsDropdownCheckbox { get; }

        [DataSourceProperty]
        public MCMSelectorVM<DropdownSelectorItemVM, string> DropdownValue => _selectorVMWrapper ??= new MCMSelectorVM<DropdownSelectorItemVM, string>(
            (PropertyReference.Value as IEnumerable<object> ?? Enumerable.Empty<object>()).Select(x => LocalizationUtils.Localize(x.ToString())),
            new SelectedIndexWrapper(PropertyReference.Value).SelectedIndex, null);

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