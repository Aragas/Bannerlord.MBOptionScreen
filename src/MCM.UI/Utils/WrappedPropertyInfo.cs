using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MCM.UI.Utils
{
    // https://github.com/BUTR/Bannerlord.UIExtenderEx/blob/rewrite/Bannerlord.UIExtenderEx/ViewModels/WrappedPropertyInfo.cs
    internal sealed class WrappedPropertyInfo : PropertyInfo
    {
        private readonly object _instance;
        private readonly PropertyInfo _propertyInfoImplementation;
        private readonly Action? _onSet;

        public WrappedPropertyInfo(PropertyInfo actualPropertyInfo, object instance, Action? onSet = null)
        {
            _propertyInfoImplementation = actualPropertyInfo;
            _instance = instance;
            _onSet = onSet;
        }

        public override PropertyAttributes Attributes => _propertyInfoImplementation.Attributes;
        public override bool CanRead => _propertyInfoImplementation.CanRead;
        public override bool CanWrite => _propertyInfoImplementation.CanWrite;
        public override IEnumerable<CustomAttributeData> CustomAttributes => _propertyInfoImplementation.CustomAttributes;
        public override Type? DeclaringType => _propertyInfoImplementation.DeclaringType;
        public override MethodInfo? GetMethod => _propertyInfoImplementation.GetMethod;
        public override MemberTypes MemberType => _propertyInfoImplementation.MemberType;
        public override int MetadataToken => _propertyInfoImplementation.MetadataToken;
        public override Module Module => _propertyInfoImplementation.Module;
        public override string Name => _propertyInfoImplementation.Name;
        public override Type PropertyType => _propertyInfoImplementation.PropertyType;
        public override Type? ReflectedType => _propertyInfoImplementation.ReflectedType;
        public override MethodInfo? SetMethod => _propertyInfoImplementation.SetMethod;

        public override MethodInfo[] GetAccessors(bool nonPublic) => _propertyInfoImplementation.GetAccessors(nonPublic)
            .Select(m => new WrappedMethodInfo(m, _instance))
            .Cast<MethodInfo>()
            .ToArray();
        public override object? GetConstantValue() => _propertyInfoImplementation.GetConstantValue();
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _propertyInfoImplementation.GetCustomAttributes(attributeType, inherit);
        public override object[] GetCustomAttributes(bool inherit) => _propertyInfoImplementation.GetCustomAttributes(inherit);
        public override IList<CustomAttributeData> GetCustomAttributesData() => _propertyInfoImplementation.GetCustomAttributesData();
        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            var getMethod = _propertyInfoImplementation.GetGetMethod(nonPublic);
            return getMethod is null ? null! : new WrappedMethodInfo(getMethod, _instance);
        }
        public override ParameterInfo[] GetIndexParameters() => _propertyInfoImplementation.GetIndexParameters();
        public override Type[] GetOptionalCustomModifiers() => _propertyInfoImplementation.GetOptionalCustomModifiers();
        public override object? GetRawConstantValue() => _propertyInfoImplementation.GetRawConstantValue();
        public override Type[] GetRequiredCustomModifiers() => _propertyInfoImplementation.GetRequiredCustomModifiers();
        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            var setMethod = _propertyInfoImplementation.GetSetMethod(nonPublic);
            return setMethod is null ? null! : new WrappedMethodInfo(setMethod, _instance);
        }
        public override object? GetValue(object? obj, object?[]? index) => _propertyInfoImplementation.GetValue(_instance, index);
        public override object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture) =>
            _propertyInfoImplementation.GetValue(_instance, invokeAttr, binder, index, culture);
        public override bool IsDefined(Type attributeType, bool inherit) => _propertyInfoImplementation.IsDefined(attributeType, inherit);
        public override void SetValue(object? obj, object? value, object?[]? index)
        {
            _propertyInfoImplementation.SetValue(_instance, value, index);
            _onSet?.Invoke();
        }

        public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
        {
            _propertyInfoImplementation.SetValue(_instance, value, invokeAttr, binder, index, culture);
            _onSet?.Invoke();
        }

        public override string? ToString() => _propertyInfoImplementation.ToString();
        public override bool Equals(object? obj) => obj switch
        {
            WrappedPropertyInfo proxy => _propertyInfoImplementation.Equals(proxy._propertyInfoImplementation),
            PropertyInfo propertyInfo => _propertyInfoImplementation.Equals(propertyInfo),
            _ => _propertyInfoImplementation.Equals(obj)
        };
        public override int GetHashCode() => _propertyInfoImplementation.GetHashCode();
    }
}