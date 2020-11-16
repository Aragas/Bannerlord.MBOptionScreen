using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using MCM.Abstractions;
using MCM.UI.Utils;

using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using TaleWorlds.Library;

namespace MCM.UI.Data
{
    public class ViewModelWrapper : ViewModel, IWrapper
    {
        /// <inheritdoc/>
        public object Object { get; }

        protected ViewModelWrapper(object @object, bool clearOriginalProperties = true)
        {
            Object = @object;

            // Copy Object properties
            var field = AccessTools.Field(typeof(ViewModel), "_propertyInfos");
            var propsObject = field.GetValue(Object) as Dictionary<string, PropertyInfo> ?? new Dictionary<string, PropertyInfo>();
            var propsThis = field.GetValue(this) as Dictionary<string, PropertyInfo> ?? new Dictionary<string, PropertyInfo>();
            if (clearOriginalProperties)
                propsThis.Clear(); // clear properties
            foreach (var (key, value) in propsObject)
            {
                propsThis[key] = new WrappedPropertyInfo(value, Object, () =>
                    {
                        OnPropertyChangedWithValue(value.GetValue(Object), value.Name);
                        OnPropertyChanged(value.Name);
                    });
            }

            // Trigger OnPropertyChanged from Object
            if (Object is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += OnPropertyChangedEventHandler;
            if (Object is ViewModel viewModel)
                viewModel.PropertyChangedWithValue += OnPropertyChangedWithValueEventHandler;
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