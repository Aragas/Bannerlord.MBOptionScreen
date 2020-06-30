﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Ref
{
    /// <summary>
    /// A broader wrapper. Uses functions for getting/setting the value
    /// </summary>
    public class ProxyRef<T> : IRef, IEquatable<ProxyRef<T>>
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly Func<T> _getter;
        private readonly Action<T>? _setter;

        public Type Type => typeof(T);
        public object Value
        {
            get => _getter()!;
            set
            {
                if (_setter != null)
                {
                    _setter((T) value);
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public ProxyRef(Func<T> getter, Action<T>? setter)
        {
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
            _setter = setter;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool Equals(ProxyRef<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _getter.Equals(other._getter) && Equals(_setter, other._setter);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProxyRef<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_getter.GetHashCode() * 397) ^ (_setter != null ? _setter.GetHashCode() : 0);
            }
        }
    }
}
