using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MCM.Common.Ref
{
    /// <summary>
    /// Wraps a class property for MCM to get/set its value
    /// </summary>
    public class PropertyRef : IRef, IEquatable<PropertyRef>
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        public PropertyInfo PropertyInfo { get; }
        public object Instance { get; }

        /// <inheritdoc/>
        public Type Type => PropertyInfo.PropertyType;
        /// <inheritdoc/>
        public object? Value
        {
            get => PropertyInfo.GetValue(Instance);
            set
            {
                if (PropertyInfo.CanWrite)
                {
                    PropertyInfo.SetValue(Instance, value);
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public PropertyRef(PropertyInfo propInfo, object instance)
        {
            PropertyInfo = propInfo;
            Instance = instance;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool Equals(PropertyRef? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return PropertyInfo.Equals(other.PropertyInfo) && Instance.Equals(other.Instance);
        }
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PropertyRef) obj);
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = 269;
            hash = (hash * 47) + PropertyInfo.GetHashCode();
            hash = (hash * 47) + Instance.GetHashCode();
            return hash;
        }

        public static bool operator ==(PropertyRef? left, PropertyRef? right) => Equals(left, right);
        public static bool operator !=(PropertyRef? left, PropertyRef? right) => !Equals(left, right);
    }
}