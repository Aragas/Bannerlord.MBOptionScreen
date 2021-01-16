using TaleWorlds.Core.ViewModelCollection;

namespace MCM.Abstractions.Data
{
    public class DropdownWrapper : IDropdownProvider, IWrapper
    {
        public object Object { get; }
        public bool IsCorrect { get; }

        public SelectorVM<SelectorItemVM> Selector { get => null!; set { } }
        public int SelectedIndex { get => 0; set { } }

        public DropdownWrapper(object @object) { }
    }
}