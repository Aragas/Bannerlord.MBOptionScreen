using TaleWorlds.Core.ViewModelCollection;

namespace MCM.Abstractions.Data
{
    public interface IDropdownProvider
    {
        SelectorVM<SelectorItemVM> Selector { get; set; }

        int SelectedIndex { get; set; }
    }
}