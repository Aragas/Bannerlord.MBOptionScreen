using MCM.UI.Utils;

using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace MCM.UI.Dropdown
{
    // For Game's Dropdown implementation
    public sealed class DropdownSelectorItemVM : DropdownSelectorItemVM<string>
    {
        public DropdownSelectorItemVM(string value) : base(value) { }
    }

    public class DropdownSelectorItemVM<T> : DropdownSelectorItemVMBase where T : class
    {
        public T OriginalItem { get; }

        [DataSourceProperty]
        public override string StringItem => OriginalItem.ToString() ?? "ERROR";

        [DataSourceProperty]
        public override HintViewModel? Hint { get; } = new HintViewModel();

        public DropdownSelectorItemVM(T @object)
        {
            OriginalItem = @object;
        }

        public override string ToString() => StringItem;
    }
}