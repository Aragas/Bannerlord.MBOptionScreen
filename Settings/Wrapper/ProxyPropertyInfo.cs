using System;
using System.Globalization;
using System.Reflection;

namespace MBOptionScreen.Settings.Wrapper
{
    /// <summary>
    /// PropertyInfo Proxy that will redirect Get/Set to the actual class, not the wrapper that holds it.
    /// </summary>
    internal class ProxyPropertyInfo : PropertyInfo
    {
        private readonly PropertyInfo _propertyInfoImplementation;

        public ProxyPropertyInfo(PropertyInfo actualPropertyInfo)
        {
            _propertyInfoImplementation = actualPropertyInfo;
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return _propertyInfoImplementation.GetCustomAttributes(inherit);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return _propertyInfoImplementation.IsDefined(attributeType, inherit);
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            if (obj is SettingsWrapper settingsWrapper)
                return _propertyInfoImplementation.GetValue(settingsWrapper._object, invokeAttr, binder, index, culture);
            return null;
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            if (obj is SettingsWrapper settingsWrapper)
                _propertyInfoImplementation.SetValue(settingsWrapper._object, value, invokeAttr, binder, index, culture);
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return _propertyInfoImplementation.GetAccessors(nonPublic);
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return _propertyInfoImplementation.GetGetMethod(nonPublic);
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return _propertyInfoImplementation.GetSetMethod(nonPublic);
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            return _propertyInfoImplementation.GetIndexParameters();
        }

        public override string Name => _propertyInfoImplementation.Name;

        public override Type DeclaringType => _propertyInfoImplementation.DeclaringType;

        public override Type ReflectedType => _propertyInfoImplementation.ReflectedType;

        public override Type PropertyType => _propertyInfoImplementation.PropertyType;

        public override PropertyAttributes Attributes => _propertyInfoImplementation.Attributes;

        public override bool CanRead => _propertyInfoImplementation.CanRead;

        public override bool CanWrite => _propertyInfoImplementation.CanWrite;

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return _propertyInfoImplementation.GetCustomAttributes(attributeType, inherit);
        }
    }
}