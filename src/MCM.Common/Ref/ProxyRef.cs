﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MCM.Common
{
    /// <summary>
    /// A broader wrapper. Uses functions for getting/setting the value
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class ProxyRef<T> : IRef, IEquatable<ProxyRef<T>>
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly Func<T> _getter;
        private readonly Action<T>? _setter;

        /// <inheritdoc/>
        public Type Type => typeof(T);
        /// <inheritdoc/>
        public object? Value
        {
            get => _getter();
            set
            {
                if (_setter is not null && value is T val)
                {
                    _setter(val);
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

        /// <inheritdoc/>
        public bool Equals(ProxyRef<T>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return _getter.Equals(other._getter) && Equals(_setter, other._setter);
        }
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProxyRef<T>) obj);
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = 269;
            hash = (hash * 47) + _getter.GetHashCode();
            if (_setter is not null)
                hash = (hash * 47) + _setter.GetHashCode();
            return hash;
        }
        public static bool operator ==(ProxyRef<T>? left, ProxyRef<T>? right) => Equals(left, right);
        public static bool operator !=(ProxyRef<T>? left, ProxyRef<T>? right) => !Equals(left, right);
    }
}