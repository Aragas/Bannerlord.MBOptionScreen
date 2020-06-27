using HarmonyLib;

using System.Reflection;

namespace MCM.Abstractions.Data
{
    public class DropdownWrapper : IDropdownProvider, IWrapper
    {
        public object Object { get; }
        private PropertyInfo? SelectedIndexProperty { get; }
        public bool IsCorrect { get; }

        public int SelectedIndex
        {
            get => (int) SelectedIndexProperty!.GetValue(Object);
            set => SelectedIndexProperty?.SetValue(Object, value);
        }

        public DropdownWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            SelectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));

            IsCorrect = SelectedIndexProperty != null;
        }
    }
}