using MCM.Common.Ref;
using MCM.Common.Utils;

using TaleWorlds.Library;

namespace MCM.Common.Dropdown
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