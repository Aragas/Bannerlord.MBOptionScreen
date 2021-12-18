using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Ref
{
    public sealed class StorageRef : IRef
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Type Type { get; }

        private object? _value;
        public object? Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public StorageRef(object? value)
        {
            _value = value;
            Type = value?.GetType() ?? throw new Exception("Value can't be null!");
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public sealed class StorageRef<T> : IRef
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Type Type { get; }

        private T? _value;
        public object? Value
        {
            get => _value;
            set
            {
                if (value is T val && !Equals(_value, val))
                {
                    _value = val;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public StorageRef(T? value)
        {
            _value = value;
            Type = value?.GetType() ?? typeof(T);
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}