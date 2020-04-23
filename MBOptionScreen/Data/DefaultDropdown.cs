using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;

namespace MBOptionScreen.Data
{
    public class DefaultDropdown<T> : List<T>, IDropdownProvider
    {
        public static DefaultDropdown<T> Empty => new DefaultDropdown<T>(Array.Empty<T>(), 0);

        private int _selectedIndex;

        public SelectorVM<SelectorItemVM> Selector { get; }

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
            _selectedIndex = selectedIndex;

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();

            Selector = new SelectorVM<SelectorItemVM>(this.Select(x => x?.ToString() ?? "ERROR"), selectedIndex, OnSelectionChanged);
        }

        private void OnSelectionChanged(SelectorVM<SelectorItemVM> obj)
        {
            SelectedIndex = obj.SelectedIndex;
        }
    }
}