using System;
using System.Collections.Generic;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.Abstractions.Data
{
    public class DefaultDropdown<T> : List<T>, IDropdownProvider, IEqualityComparer<DefaultDropdown<T>>
    {
        public static DefaultDropdown<T> Empty => new(Array.Empty<T>(), 0);

        public SelectorVM<SelectorItemVM> Selector { get => null!; set { } }

        public int SelectedIndex { get => -1; set { } }
        public T SelectedValue { get => default!; set { } }

        public DefaultDropdown(IEnumerable<T> values, int selectedIndex) : base(values) { }

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