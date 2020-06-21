using MCM.Abstractions.Ref;

using System;
using System.Collections.Generic;

namespace MCM.Abstractions.Data
{
    internal class CheckboxDropdown : List<IRef>, ICheckboxDropdown, IDropdownProvider, IEqualityComparer<CheckboxDropdown>
    {
        public static CheckboxDropdown Empty => new CheckboxDropdown(Array.Empty<IRef>(), 0);

        private MCMSelectorVM<MCMSelectorItemVM<bool>> _selector;
        private int _selectedIndex;

        public MCMSelectorVM<MCMSelectorItemVM<bool>> Selector
        {
            get => _selector;
            set
            {
                if (_selector != value)
                {
                    _selector = value;
                    _selector.SetOnChangeAction(OnSelectionChanged);
                }
            }
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    Selector.SelectedIndex = _selectedIndex;
                }
            }
        }

        public CheckboxDropdown(IList<IRef> values, int selectedIndex) : base(values)
        {
            _selector = new MCMSelectorVM<MCMSelectorItemVM<bool>>(values, selectedIndex, OnSelectionChanged);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        private void OnSelectionChanged(MCMSelectorVM<MCMSelectorItemVM<bool>> obj) => _selectedIndex = obj.SelectedIndex;

        public bool Equals(CheckboxDropdown x, CheckboxDropdown y) => x.SelectedIndex == y.SelectedIndex;
        public int GetHashCode(CheckboxDropdown obj) => obj.SelectedIndex;

        public override int GetHashCode() => GetHashCode(this);
        public override bool Equals(object obj)
        {
            if (obj is CheckboxDropdown dropdown)
                return Equals(this, dropdown);

            return base.Equals(obj);
        }
    }
}