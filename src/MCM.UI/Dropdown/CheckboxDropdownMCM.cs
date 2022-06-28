/*
using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.UI.Dropdown
{
    /// <summary>
    /// Postponed to v5
    /// </summary>
    internal sealed class CheckboxDropdownMCM<T> : List<T>, IEqualityComparer<CheckboxDropdownMCM<T>>
        where T : class
    {
        public static CheckboxDropdownMCM<T> Empty => new(Enumerable.Empty<T>(), 0);

        private MCMSelectorVM<CheckboxDropdownSelectorItemVM<T>, T> _selector;
        private int _selectedIndex;

        internal MCMSelectorVM<CheckboxDropdownSelectorItemVM<T>, T> Selector
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

        public CheckboxDropdownMCM(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            _selector = new MCMSelectorVM<CheckboxDropdownSelectorItemVM<T>, T>(values, selectedIndex, OnSelectionChanged);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        private void OnSelectionChanged(MCMSelectorVM<CheckboxDropdownSelectorItemVM<T>> obj) => _selectedIndex = obj.SelectedIndex;

        /// <inheritdoc/>
        public bool Equals(CheckboxDropdownMCM<T>? x, CheckboxDropdownMCM<T>? y) => x?.SelectedIndex == y?.SelectedIndex;
        /// <inheritdoc/>
        public int GetHashCode(CheckboxDropdownMCM<T> obj) => obj.SelectedIndex;

        /// <inheritdoc/>
        public override int GetHashCode() => GetHashCode(this);
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is CheckboxDropdownMCM<T> dropdown)
                return Equals(this, dropdown);

            return ReferenceEquals(this, obj);
        }
    }
}
*/