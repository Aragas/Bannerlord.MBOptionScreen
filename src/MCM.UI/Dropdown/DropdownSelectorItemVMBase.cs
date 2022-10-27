using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace MCM.UI.Dropdown
{
    public abstract class DropdownSelectorItemVMBase : ViewModel
    {
        [DataSourceProperty]
        public bool CanBeSelected { get; set; } = true;

        [DataSourceProperty]
        public abstract string StringItem { get; }

        [DataSourceProperty]
        public abstract HintViewModel? Hint { get; }
    }
}