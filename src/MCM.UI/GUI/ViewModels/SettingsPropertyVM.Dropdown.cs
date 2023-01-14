using MCM.Abstractions;
using MCM.Common;
using MCM.UI.Actions;
using MCM.UI.Dropdown;
using MCM.UI.Utils;

using System.ComponentModel;
using System.Linq;

using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        private MCMSelectorVM<MCMSelectorItemVM<TextObject>, TextObject>? _selectorVMWrapper;

        [DataSourceProperty]
        public bool IsDropdown => IsDropdownDefault || IsDropdownCheckbox;
        [DataSourceProperty]
        public bool IsDropdownDefault => SettingType == SettingType.Dropdown && SettingsUtils.IsForTextDropdown(PropertyReference.Value);
        [DataSourceProperty]
        public bool IsDropdownCheckbox => SettingType == SettingType.Dropdown && SettingsUtils.IsForCheckboxDropdown(PropertyReference.Value);

        [DataSourceProperty]
        public MCMSelectorVM<MCMSelectorItemVM<TextObject>, TextObject> DropdownValue =>_selectorVMWrapper ??= IsDropdown
            ? new MCMSelectorVM<MCMSelectorItemVM<TextObject>, TextObject>(UISettingsUtils.GetDropdownValues(PropertyReference).Select(x => new TextObject(x.ToString())), new SelectedIndexWrapper(PropertyReference.Value).SelectedIndex)
            : MCMSelectorVM<MCMSelectorItemVM<TextObject>, TextObject>.Empty;

        private void DropdownValue_PropertyChanged(object? obj, PropertyChangedEventArgs args)
        {
            if (obj is not null && args.PropertyName == "SelectedIndex")
                URS.Do(new SetSelectedIndexAction(PropertyReference, obj));
        }
        private void DropdownValue_PropertyChangedWithValue(object obj, PropertyChangedWithValueEventArgs args)
        {
            if (args.PropertyName == "SelectedIndex")
                URS.Do(new SetSelectedIndexAction(PropertyReference, obj));
        }
    }
}