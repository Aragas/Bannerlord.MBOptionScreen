using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Common
{
    public sealed class Dropdown<T> : List<T>, IEqualityComparer<Dropdown<T>>
    {
        public static Dropdown<T> Empty => new(Enumerable.Empty<T>(), 0);

        public int SelectedIndex { get; set; }

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

        public Dropdown(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            SelectedIndex = selectedIndex;
            if (SelectedIndex != 0 && SelectedIndex >= Count)
                throw new Exception();
        }

        /// <inheritdoc/>
        public bool Equals(Dropdown<T>? x, Dropdown<T>? y) => x?.SelectedIndex == y?.SelectedIndex;
        /// <inheritdoc/>
        public int GetHashCode(Dropdown<T> obj) => obj.SelectedIndex;

        /// <inheritdoc/>
        public override int GetHashCode() => GetHashCode(this);
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is Dropdown<T> dropdown)
                return Equals(this, dropdown);

            return ReferenceEquals(this, obj);
        }
    }
}