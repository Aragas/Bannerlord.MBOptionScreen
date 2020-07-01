using HarmonyLib;

using MCM.Abstractions;

using System.Reflection;

namespace MCM.UI.Data
{
    /// <summary>
    /// A simple wrapper that is used to get SelectedIndex
    /// </summary>
    public class SelectorWrapper : IWrapper
    {
        public object Object { get; }
        private PropertyInfo? SelectedIndexProperty { get; }
        public bool IsCorrect { get; }

        public int SelectedIndex
        {
            get => (int) SelectedIndexProperty!.GetValue(Object);
            set => SelectedIndexProperty?.SetValue(Object, value);
        }

        public SelectorWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            SelectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));

            IsCorrect = SelectedIndexProperty != null;
        }
    }
}