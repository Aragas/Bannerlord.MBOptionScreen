using MCM.Abstractions;
using MCM.Common;
using MCM.UI.Actions;
using MCM.UI.Dropdown;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        private MCMSelectorVM<DropdownSelectorItemVM<TextObject>, TextObject>? _selectorVMWrapper;

        [DataSourceProperty]
        public bool IsDropdown => IsDropdownDefault || IsDropdownCheckbox;
        [DataSourceProperty]
        public bool IsDropdownDefault => SettingType == SettingType.Dropdown && SettingsUtils.IsForTextDropdown(PropertyReference.Value);
        [DataSourceProperty]
        public bool IsDropdownCheckbox => SettingType == SettingType.Dropdown && SettingsUtils.IsForCheckboxDropdown(PropertyReference.Value);

        [DataSourceProperty]
        public MCMSelectorVM<DropdownSelectorItemVM<TextObject>, TextObject> DropdownValue => _selectorVMWrapper ??= new MCMSelectorVM<DropdownSelectorItemVM<TextObject>, TextObject>(
            (PropertyReference.Value as IEnumerable<object> ?? Enumerable.Empty<object>()).Select(x => new TextObject(x.ToString())),
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