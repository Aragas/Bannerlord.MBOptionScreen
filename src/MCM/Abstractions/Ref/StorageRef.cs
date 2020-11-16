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
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public StorageRef(object? value)
        {
            _value = value;
            Type = value?.GetType() ?? typeof(object); // TODO
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}