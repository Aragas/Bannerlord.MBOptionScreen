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
        public static DropdownMCM<T> Empty => new(Enumerable.Empty<T>(), 0);

        internal MCMSelectorVM<DropdownSelectorItemVM, string> Selector { get; private set; }

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

        public DropdownMCM(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            var select = this.Select(x => new TextObject(x?.ToString() ?? "ERROR").ToString());
            Selector = new MCMSelectorVM<DropdownSelectorItemVM, string>(select, selectedIndex, null);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

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