using System;

namespace MCM.UI.Actions
{
    internal class ProxyRef : IRef
    {
        private readonly Func<object> _getter;
        private readonly Action<object>? _setter;
        public object Value { get => _getter(); set => _setter?.Invoke(value); }

        public ProxyRef(Func<object> getter, Action<object>? setter)
        {
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
            _setter = setter;
        }
    }
}