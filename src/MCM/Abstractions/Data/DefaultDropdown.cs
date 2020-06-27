using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Data
{
    public class DefaultDropdown<T> : List<T>, IDefaultDropdown, IDropdownProvider, IEqualityComparer<DefaultDropdown<T>>
    {
        public static DefaultDropdown<T> Empty => new DefaultDropdown<T>(Array.Empty<T>(), 0);

        private SelectorVM<SelectorItemVM> _selector;
        private int _selectedIndex;

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

        public DefaultDropdown(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            var select = this.Select(x => new TextObject(x?.ToString() ?? "ERROR").ToString());
            _selector = new SelectorVM<SelectorItemVM>(select, selectedIndex, OnSelectionChanged);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        private void OnSelectionChanged(SelectorVM<SelectorItemVM> obj) => _selectedIndex = obj.SelectedIndex;

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