using TaleWorlds.Core.ViewModelCollection;

namespace MBOptionScreen.Data
{
    public interface IDropdownProvider
    {
        SelectorVM<SelectorItemVM> Selector { get; set; }

        int SelectedIndex { get; set; }
    }
}