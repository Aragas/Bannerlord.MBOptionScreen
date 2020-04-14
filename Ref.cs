using System;
using System.Reflection;

namespace ModLib
{
    public class Ref
    {
        private Func<object> getter;
        private Action<object> setter;
        private PropertyInfo propInfo = null;
        private object instance = null;

        public object Value
        {
            get
            {
                if (propInfo != null)
                    return propInfo.GetValue(instance);
                else
                    return getter();
            }
            set
            {
                if (propInfo != null)
                    propInfo.SetValue(instance, value);
                else
                    setter(value);
            }
        }

        public Ref(Func<object> getter, Action<object> setter)
        {
            this.getter = getter ?? throw new ArgumentNullException("getter");
            this.setter = setter;
        }

        public Ref(Func<object> getter) : this(getter, null)
        {

        }

        public Ref(PropertyInfo propInfo, object instance)
        {
            this.propInfo = propInfo;
            this.instance = instance;
        }
    }
}
