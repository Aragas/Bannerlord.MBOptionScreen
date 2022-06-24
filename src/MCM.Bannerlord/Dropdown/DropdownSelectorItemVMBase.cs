using TaleWorlds.Library;

namespace MCM.Common.Dropdown
{
    public abstract class DropdownSelectorItemVMBase : ViewModel
    {
        [DataSourceProperty]
        public bool CanBeSelected { get; set; } = true;

        [DataSourceProperty]
        public abstract string StringItem { get; }

        [DataSourceProperty]
        public abstract ViewModel? Hint { get; }
    }
}