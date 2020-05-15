using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Ref
{
    public class ProxyRef<T> : IRef
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
    }
}