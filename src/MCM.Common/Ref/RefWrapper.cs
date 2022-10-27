using HarmonyLib.BUTR.Extensions;

using System;
using System.ComponentModel;

namespace MCM.Common
{
    /// <summary>
    /// Wrapper around any type that implements <see cref="IRef"/>.
    /// We don't use casting because it might not be safe.
    /// </summary>
    public class RefWrapper : IRef, IWrapper
    {
        private delegate Type GetTypeDelegate();
        private delegate object GetValueDelegate();
        private delegate void SetValueDelegate(object value);

        private readonly GetTypeDelegate? _getTypeDelegate;
        private readonly GetValueDelegate? _getValueDelegate;
        private readonly SetValueDelegate? _setValueDelegate;

        /// <inheritdoc/>
        public object Object { get; }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }
        /// <inheritdoc/>
        public Type Type => _getTypeDelegate!.Invoke();
        /// <inheritdoc/>
        public object? Value
        {
            get => _getValueDelegate!.Invoke();
            set
            {
                if (_setValueDelegate is not null && value is not null)
                    _setValueDelegate(value);
            }
        }

        public RefWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            _getTypeDelegate = AccessTools2.GetPropertyGetterDelegate<GetTypeDelegate>(@object, type, nameof(Type));
            _getValueDelegate = AccessTools2.GetPropertyGetterDelegate<GetValueDelegate>(@object, type, nameof(Value));
            _setValueDelegate = AccessTools2.GetPropertySetterDelegate<SetValueDelegate>(@object, type, nameof(Value));
        }
    }
}