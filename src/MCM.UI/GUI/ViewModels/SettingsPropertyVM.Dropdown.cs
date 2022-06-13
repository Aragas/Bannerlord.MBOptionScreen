using MCM.Abstractions.Common.ViewModelWrappers;
using MCM.Abstractions.Common.Wrappers;
using MCM.Abstractions.Dropdown;
using MCM.UI.Actions;
using MCM.Utils;

using System.ComponentModel;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        [DataSourceProperty]
        public bool IsDropdown { get; }
        [DataSourceProperty]
        public bool IsDropdownDefault { get; }
        [DataSourceProperty]
        public bool IsDropdownCheckbox { get; }

        [DataSourceProperty]
        public SelectorVMWrapper DropdownValue
        {
            get => _selectorVMWrapper ??= new SelectorVMWrapper(IsDropdown && PropertyReference.Value is { } val
                ? SettingsUtils.GetSelector(val)
                : MCMSelectorVM<MCMSelectorItemVM>.Empty);
            set
            {
                if (IsDropdown && DropdownValue != value)
                {
                    // TODO
                    URS.Do(new ComplexReferenceTypeAction<object>(PropertyReference, selector =>
                    {
                        //selector.ItemList = DropdownValue.ItemList;
                        if (selector is not null)
                        {
                            var wrapper = new SelectedIndexWrapper(selector);
                            wrapper.SelectedIndex = DropdownValue.SelectedIndex;
                        }
                    }, selector =>
                    {
                        //selector.ItemList = DropdownValue.ItemList;
                        if (selector is not null)
                        {
                            var wrapper = new SelectedIndexWrapper(selector);
                            wrapper.SelectedIndex = DropdownValue.SelectedIndex;
                        }
                    }));
                    OnPropertyChanged(nameof(DropdownValue));
                }
            }
        }

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