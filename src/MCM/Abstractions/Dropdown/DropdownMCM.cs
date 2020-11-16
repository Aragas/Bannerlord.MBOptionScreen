using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Dropdown
{
    /// <summary>
    /// Uses MCM's ViewModels
    /// </summary>
    public sealed class DropdownMCM<T> : List<T>, IEqualityComparer<DropdownMCM<T>>
        where T : class
    {
        public static DropdownMCM<T> Empty => new DropdownMCM<T>(Enumerable.Empty<T>(), 0);

        private MCMSelectorVM<DropdownSelectorItemVM, string> _selector;
        private int _selectedIndex;

        internal MCMSelectorVM<DropdownSelectorItemVM, string> Selector
        {
            get
            {
                _selector.Refresh(this.Select(x => new TextObject(x?.ToString() ?? "ERROR").ToString()), SelectedIndex, OnSelectionChanged);
                return _selector;
            }
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

        public T SelectedValue
        {
            get => this[SelectedIndex];
            set
            {
                var index = IndexOf(value);
                if (index == -1)
                    return;
                SelectedIndex = index;
            }
        }

        public DropdownMCM(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            var select = this.Select(x => new TextObject(x?.ToString() ?? "ERROR").ToString());
            _selector = new MCMSelectorVM<DropdownSelectorItemVM, string>(select, selectedIndex, OnSelectionChanged);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        private void OnSelectionChanged(MCMSelectorVM<DropdownSelectorItemVM> obj) => _selectedIndex = obj.SelectedIndex;

        /// <inheritdoc/>
        public bool Equals(DropdownMCM<T>? x, DropdownMCM<T>? y) => x?.SelectedIndex == y?.SelectedIndex;
        /// <inheritdoc/>
        public int GetHashCode(DropdownMCM<T> obj) => obj.SelectedIndex;

        /// <inheritdoc/>
        public override int GetHashCode() => GetHashCode(this);
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is DropdownMCM<T> dropdown)
                return Equals(this, dropdown);

            return ReferenceEquals(this, obj);
        }
    }
}