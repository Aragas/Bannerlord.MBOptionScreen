using MCM.Common;
using MCM.UI.Utils;

using TaleWorlds.Library;

namespace MCM.UI.Dropdown
{
    internal sealed class DropdownRefSelectorItemVM : DropdownSelectorItemVMBase
    {
        private readonly IRef _ref;

        [DataSourceProperty]
        public override string StringItem => _ref.Value?.ToString() ?? "ERROR";

        [DataSourceProperty]
        public override ViewModel? Hint { get; } = HintViewModelUtils.Create();

        public DropdownRefSelectorItemVM(IRef @ref)
        {
            _ref = @ref;
        }

        public override string ToString() => _ref.Value?.ToString() ?? "ERROR";
    }
}