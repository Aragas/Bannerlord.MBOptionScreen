using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace MCM.Abstractions.Dropdown
{
    // For Game's Dropdown implementation
    internal sealed class DropdownSelectorItemVM : DropdownSelectorItemVM<string>
    {
        public DropdownSelectorItemVM(string value) : base(value) { }
    }

    internal class DropdownSelectorItemVM<T> : DropdownSelectorItemVMBase where T : class
    {
        protected readonly T _object;

        [DataSourceProperty]
        public override string StringItem => _object.ToString();

        [DataSourceProperty]
        public override HintViewModel Hint { get; } = new HintViewModel(string.Empty);

        public DropdownSelectorItemVM(T @object)
        {
            _object = @object;
        }

        public override string ToString() => StringItem;
    }
}