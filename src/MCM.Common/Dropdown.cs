using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MCM.Common
{
    public sealed class Dropdown<T> : List<T>, IEqualityComparer<Dropdown<T>>, INotifyPropertyChanged, ICloneable
    {
        public static Dropdown<T> Empty => new(Enumerable.Empty<T>(), 0);

        private int _selectedIndex;
        private T _selectedValue;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int SelectedIndex { get => _selectedIndex; set => SetField(ref _selectedIndex, value, nameof(SelectedIndex)); }

        public T SelectedValue
        {
            get => this[SelectedIndex];
            set
            {
                if (SetField(ref _selectedValue, value, nameof(SelectedValue)))
                {
                    var index = IndexOf(value);
                    if (index == -1)
                        return;
                    SelectedIndex = index;
                }
            }
        }

        public Dropdown(IEnumerable<T> values, int selectedIndex) : base(values)
        {
            SelectedIndex = selectedIndex;
            if (SelectedIndex != 0 && SelectedIndex >= Count)
                Debug.Fail($"Invalid 'SelectedIndex' set for Dropdown! {selectedIndex}, max is {Count}");
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool SetField<TVal>(ref TVal field, TVal value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<TVal>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
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

        /// <inheritdoc />
        public object Clone() => new Dropdown<T>(this, SelectedIndex);
    }
}