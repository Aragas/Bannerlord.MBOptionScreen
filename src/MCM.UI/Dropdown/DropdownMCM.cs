using MCM.Common;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Localization;

namespace MCM.UI.Dropdown
{
    /// <summary>
    /// Uses MCM's ViewModels
    /// </summary>
    public sealed class DropdownMCM : List<object>, IEqualityComparer<DropdownMCM>
    {
        public static DropdownMCM Empty => new(Enumerable.Empty<object>(), 0);

        public MCMSelectorVM<DropdownSelectorItemVM, string> Selector { get; private set; }

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

        public object SelectedValue
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

        public DropdownMCM(IRef @ref) : this(@ref.Value is IEnumerable<object> x ? x : Enumerable.Empty<object>(), new SelectedIndexWrapper(@ref.Value).SelectedIndex) { }
        public DropdownMCM(IEnumerable<object> values, int selectedIndex) : base(values)
        {
            var select = this.Select(x => new TextObject(x?.ToString() ?? "ERROR").ToString());
            Selector = new MCMSelectorVM<DropdownSelectorItemVM, string>(select, selectedIndex, null);

            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        /// <inheritdoc/>
        public bool Equals(DropdownMCM? x, DropdownMCM? y) => x?.SelectedIndex == y?.SelectedIndex;
        /// <inheritdoc/>
        public int GetHashCode(DropdownMCM obj) => obj.SelectedIndex;

        /// <inheritdoc/>
        public override int GetHashCode() => GetHashCode(this);
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is DropdownMCM dropdown)
                return Equals(this, dropdown);

            return ReferenceEquals(this, obj);
        }
    }
}