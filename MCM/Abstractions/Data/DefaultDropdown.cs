using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.Abstractions.Data
{
    public class DefaultDropdown<T> : List<T>, IDropdownProvider, IEqualityComparer<DefaultDropdown<T>>
    {
        public static DefaultDropdown<T> Empty => new DefaultDropdown<T>(Array.Empty<T>(), 0);

        private SelectorVM<SelectorItemVM> _selector;
        public SelectorVM<SelectorItemVM> Selector
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

        private void OnSelectionChanged(SelectorVM<SelectorItemVM> obj)
        {

        }

        public int SelectedIndex
        {
            get => Selector.SelectedIndex;
            set
            {
                if (Selector.SelectedIndex != value)
                {
                    Selector.SelectedIndex = value;
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
            _selector = new SelectorVM<SelectorItemVM>(this.Select(x => x?.ToString() ?? "ERROR"), selectedIndex, OnSelectionChanged);

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