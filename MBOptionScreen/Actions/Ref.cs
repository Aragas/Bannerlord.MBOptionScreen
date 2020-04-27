using System;
using System.Reflection;

namespace MBOptionScreen.Actions
{
    public class Ref
    {
        private readonly Func<object> _getter;
        private readonly Action<object> _setter;
        public readonly PropertyInfo _propInfo;
        public readonly object _instance;

        public object Value
        {
            get
            {
                if (_propInfo != null)
                    return _propInfo.GetValue(_instance);
                else
                    return _getter();
            }
            set
            {
                if (_propInfo != null)
                    _propInfo.SetValue(_instance, value);
                else
                    _setter(value);
            }
        }

        public Ref(Func<object> getter, Action<object> setter)
        {
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
            _setter = setter;
        }

        public Ref(Func<object> getter) : this(getter, null)
        {

        }

        public Ref(PropertyInfo propInfo, object instance)
        {
            _propInfo = propInfo;
            _instance = instance;
        }
    }
}