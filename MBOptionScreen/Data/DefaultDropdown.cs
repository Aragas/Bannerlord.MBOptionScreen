using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;

namespace MBOptionScreen.Data
{
    public class DefaultDropdown<T> : List<T>, IDropdownProvider, IEqualityComparer<DefaultDropdown<T>>
    {
        public static DefaultDropdown<T> Empty => new DefaultDropdown<T>(Array.Empty<T>(), 0);

        private SelectorVM<SelectorItemVM> selector;
        private int _selectedIndex;
        public SelectorVM<SelectorItemVM> Selector
        {
            get { return selector; }
            set
            {
                if (selector != value)
                {
                    selector = value;
                    selector.SetOnChangeAction(OnSelectionChanged);
                }
            }
        }

        private void OnSelectionChanged(SelectorVM<SelectorItemVM> obj)
        {
            _selectedIndex = obj.SelectedIndex;
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

        public DefaultDropdown(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            Selector = new SelectorVM<SelectorItemVM>(this.Select(x => x?.ToString() ?? "ERROR"), selectedIndex, OnSelectionChanged);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        public bool Equals(DefaultDropdown<T> x, DefaultDropdown<T> y) => x.SelectedIndex == y.SelectedIndex;
        public int GetHashCode(DefaultDropdown<T> obj) => obj.SelectedIndex;

        public override int GetHashCode() => GetHashCode(this);
        public override bool Equals(object obj)
        {
            if (obj is DefaultDropdown<T> dropdown)
                return Equals(this, dropdown);

            return base.Equals(obj);

        }
    }
}