using HarmonyLib;

using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.Abstractions.Data
{
    public class DropdownWrapper : IDropdownProvider, IWrapper
    {
        public object Object { get; }
        private PropertyInfo? SelectorProperty { get; }
        private PropertyInfo? SelectedIndexProperty { get; }
        public bool IsCorrect { get; }

        public SelectorVM<SelectorItemVM> Selector
        {
            get =>(SelectorVM<SelectorItemVM>) SelectorProperty!.GetValue(Object);
            set => SelectorProperty?.SetValue(Object, value);
        }
        public int SelectedIndex
        {
            get => (int) SelectedIndexProperty!.GetValue(Object);
            set => SelectedIndexProperty?.SetValue(Object, value);
        }

        public DropdownWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            SelectorProperty = AccessTools.Property(type, nameof(Selector));
            SelectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));

            IsCorrect = SelectorProperty != null && SelectedIndexProperty != null;
        }
    }
}