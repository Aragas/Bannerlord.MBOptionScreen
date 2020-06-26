using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Ref
{
    /// <summary>
    /// Wraps a class property for MCM to get/set its value
    /// </summary>
    public class PropertyRef : IRef, IEquatable<PropertyRef>
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public PropertyInfo PropertyInfo { get; }
        public object Instance { get; }

        public Type Type => PropertyInfo.PropertyType;
        public object Value
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PropertyRef) obj);
        }
        public bool Equals(PropertyRef? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return PropertyInfo.Equals(other.PropertyInfo) && Instance.Equals(other.Instance);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (PropertyInfo.GetHashCode() * 397) ^ Instance.GetHashCode();
            }
        }
    }
}
