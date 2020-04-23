using TaleWorlds.Core.ViewModelCollection;

namespace MBOptionScreen.Data
{
    public interface IDropdownProvider
    {
        SelectorVM<SelectorItemVM> Selector { get; }

        int SelectedIndex { get; set; }
    }
}