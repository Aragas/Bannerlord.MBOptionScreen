using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Dropdown
{
    // Uses Game's ViewModels
    public class DropdownDefault<T> : List<T>, IEqualityComparer<DropdownDefault<T>>
    {
        public static DropdownDefault<T> Empty => new DropdownDefault<T>(Enumerable.Empty<T>(), 0);

        private SelectorVM<SelectorItemVM> _selector;
        private int _selectedIndex;

        internal SelectorVM<SelectorItemVM> Selector
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

        public DropdownDefault(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            var select = this.Select(x => new TextObject(x?.ToString() ?? "ERROR").ToString());
            _selector = new SelectorVM<SelectorItemVM>(select, selectedIndex, OnSelectionChanged);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        private void OnSelectionChanged(SelectorVM<SelectorItemVM> obj) => _selectedIndex = obj.SelectedIndex;

        /// <inheritdoc/>
        public bool Equals(DropdownDefault<T> x, DropdownDefault<T> y) => x.SelectedIndex == y.SelectedIndex;
        /// <inheritdoc/>
        public int GetHashCode(DropdownDefault<T> obj) => obj.SelectedIndex;

        /// <inheritdoc/>
        public override int GetHashCode() => GetHashCode(this);
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is DropdownDefault<T> dropdown)
                return Equals(this, dropdown);

            return base.Equals(obj);
        }
    }
}