using HarmonyLib;

using MCM.Abstractions;
using MCM.UI.Extensions;
using MCM.UI.Utils;

using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using TaleWorlds.Library;

namespace MCM.UI.Data
{
    /// <summary>
    /// A complex wrapper that replaces any Selector ViewModel, just needs the right properties
    /// </summary>
    public class SelectorVMWrapper : ViewModel, IWrapper
    {
        public object Object { get; }
        private PropertyInfo? SelectedIndexProperty { get; }
        public bool IsCorrect { get; }

        public int SelectedIndex
        {
            get => (int) SelectedIndexProperty!.GetValue(Object);
            set => SelectedIndexProperty?.SetValue(Object, value);
        }

        public SelectorVMWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            SelectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));

            IsCorrect = SelectedIndexProperty != null;

            // Copy Object properties
            var field = AccessTools.Field(typeof(ViewModel), "_propertyInfos");
            var propsObject = field.GetValue(Object) as Dictionary<string, PropertyInfo> ?? new Dictionary<string, PropertyInfo>();
            var propsThis = field.GetValue(this) as Dictionary<string, PropertyInfo> ?? new Dictionary<string, PropertyInfo>();
            foreach (var (key, value) in propsObject)
            {
                if (propsThis.ContainsKey(key))
                    propsThis[key] = new WrappedPropertyInfo(value, Object, () => OnPropertyChanged(value.Name));
                else
                    propsThis.Add(key, new WrappedPropertyInfo(value, Object, () => OnPropertyChanged(value.Name)));
            }

            // Trigger OnPropertyChanged from Object
            if (Object is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
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
                viewModel.OnFinalize();

            base.OnFinalize();
        }
    }
}