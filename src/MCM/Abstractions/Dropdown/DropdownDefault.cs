using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Dropdown
{
    /// <summary>
    /// Uses Game's ViewModels
    /// </summary>
    public sealed class DropdownDefault<T> : List<T>, IEqualityComparer<DropdownDefault<T>>
    {
        public static DropdownDefault<T> Empty => new(Enumerable.Empty<T>(), 0);

        internal SelectorVM<SelectorItemVM> Selector { get; set; }

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

        public DropdownDefault(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            var select = this.Select(x => new TextObject(x?.ToString() ?? "ERROR").ToString());
            Selector = new SelectorVM<SelectorItemVM>(select, selectedIndex, null);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        /// <inheritdoc/>
        public bool Equals(DropdownDefault<T>? x, DropdownDefault<T>? y) => x?.SelectedIndex == y?.SelectedIndex;
        /// <inheritdoc/>
        public int GetHashCode(DropdownDefault<T> obj) => obj.SelectedIndex;

        /// <inheritdoc/>
        public override int GetHashCode() => GetHashCode(this);
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is DropdownDefault<T> dropdown)
                return Equals(this, dropdown);

            return ReferenceEquals(this, obj);
        }
    }
}