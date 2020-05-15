using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Ref
{
    public class PropertyRef : IRef
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
                PropertyInfo.SetValue(Instance, value);
                OnPropertyChanged(nameof(Value));
            }
        }

        public PropertyRef(PropertyInfo propInfo, object instance)
        {
            PropertyInfo = propInfo;
            Instance = instance;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}