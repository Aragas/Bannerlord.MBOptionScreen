using Bannerlord.BUTR.Shared.Extensions;
using Bannerlord.BUTR.Shared.Utils;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.Common;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using TaleWorlds.Library;

using WrappedPropertyInfo = MCM.UI.Utils.WrappedPropertyInfo;

namespace MCM.UI.ViewModelWrappers
{
    public class ViewModelWrapper : ViewModel, IWrapper
    {
        private static readonly AccessTools.FieldRef<object, Dictionary<string, PropertyInfo>>? PropertyInfosField =
            AccessTools2.FieldRefAccess<Dictionary<string, PropertyInfo>>("TaleWorlds.Library.ViewModel:_propertyInfos");


        private delegate Dictionary<string, PropertyInfo> GetPropertiesDelegate(object instance);
        private delegate Dictionary<string, MethodInfo> GetMethodsDelegate(object instance);

        private static readonly AccessTools.FieldRef<object, object>? PropertiesAndMethods =
            AccessTools2.FieldRefAccess<object>("TaleWorlds.Library.ViewModel:_propertiesAndMethods");

        private static readonly GetPropertiesDelegate? GetProperties =
            AccessTools2.GetDeclaredPropertyGetterDelegate<GetPropertiesDelegate>("TaleWorlds.Library.ViewModel+DataSourceTypeBindingPropertiesCollection:Properties");
        private static readonly GetMethodsDelegate? GetMethods =
            AccessTools2.GetDeclaredPropertyGetterDelegate<GetMethodsDelegate>("TaleWorlds.Library.ViewModel+DataSourceTypeBindingPropertiesCollection:Methods");

        private static readonly AccessTools.FieldRef<IDictionary>? CachedViewModelProperties =
            AccessTools2.StaticFieldRefAccess<IDictionary>("TaleWorlds.Library.ViewModel:_cachedViewModelProperties");

        /// <inheritdoc/>
        public object Object { get; }

        protected ViewModelWrapper(object @object, bool clearOriginalProperties = true)
        {
            Object = @object;

            CopyProperties(clearOriginalProperties);
            CopyMethods(clearOriginalProperties);

            // Trigger OnPropertyChanged from Object
            if (Object is IViewModel viewModel)
                viewModel.PropertyChangedWithValue += OnPropertyChangedWithValueEventHandler;
            else if (Object is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += OnPropertyChangedEventHandler;
        }

        private void CopyProperties(bool clearOriginalProperties)
        {
            if (PropertyInfosField is not null && PropertyInfosField(Object) is { } propsObject && PropertyInfosField(this) is { } propsThis)
            {
                if (clearOriginalProperties) propsThis.Clear(); // clear properties

                foreach (var (key, value) in propsObject)
                {
                    propsThis[key] = new WrappedPropertyInfo(value, Object, () =>
                    {
                        OnPropertyChangedWithValue(value.GetValue(Object), value.Name);
                        OnPropertyChanged(value.Name);
                    });
                }
            }

            if (PropertiesAndMethods?.Invoke(Object) is { } storageObject && PropertiesAndMethods(this) is { } storageThis)
            {
                if (GetProperties?.Invoke(storageObject) is { } propsObject2 && GetProperties(storageThis) is { } propsThis2 && CachedViewModelProperties is not null)
                {
                    // TW caches the properties, since we modify each VM individually, we need to copy them
                    var type = GetType();
                    var staticStorage = CachedViewModelProperties() is { } dict2 && dict2.Contains(type) ? dict2[type] : null;
                    var staticPropsDict = staticStorage is not null ? GetProperties(staticStorage) : null;
                    if (propsThis2 == staticPropsDict)
                        propsThis2 = new(propsThis2);

                    if (clearOriginalProperties) propsThis2.Clear(); // clear properties

                    foreach (var (key, value) in propsObject2)
                    {
                        propsThis2[key] = new WrappedPropertyInfo(value, Object, () =>
                        {
                            OnPropertyChangedWithValue(value.GetValue(Object), value.Name);
                            OnPropertyChanged(value.Name);
                        });
                    }
                }
            }
        }

        private void CopyMethods(bool clearOriginalProperties)
        {
            if (PropertiesAndMethods?.Invoke(Object) is { } storageObject && PropertiesAndMethods(this) is { } storageThis)
            {
                if (GetMethods?.Invoke(storageObject) is { } methodsObject && GetMethods(storageThis) is { } methodsThis && CachedViewModelProperties is not null)
                {
                    // TW caches the methods, since we modify each VM individually, we need to copy them
                    var type = GetType();
                    var staticStorage = CachedViewModelProperties() is { } dict2 && dict2.Contains(type) ? dict2[type] : null;
                    var staticMethodDict = staticStorage is not null ? GetMethods(staticStorage) : null;
                    if (methodsThis == staticMethodDict)
                        methodsThis = new(methodsThis);

                    if (clearOriginalProperties) methodsThis.Clear(); // clear properties

                    foreach (var (key, value) in methodsObject)
                    {
                        methodsThis[key] = new WrappedMethodInfo(value, Object);
                    }
                }
            }
        }

        private void OnPropertyChangedEventHandler(object? sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }
        private void OnPropertyChangedWithValueEventHandler(object? sender, PropertyChangedWithValueEventArgs args)
        {
            OnPropertyChangedWithValue(args.Value, args.PropertyName);
        }

        public override void RefreshValues()
        {
            if (Object is ViewModel viewModel)
                viewModel.RefreshValues();

            base.RefreshValues();
        }

        public override void OnFinalize()
        {
            if (Object is ViewModel viewModel)
            {
                viewModel.PropertyChanged -= OnPropertyChangedEventHandler;
                viewModel.PropertyChangedWithValue -= OnPropertyChangedWithValueEventHandler;

                viewModel.OnFinalize();
            }

            base.OnFinalize();
        }
    }
}