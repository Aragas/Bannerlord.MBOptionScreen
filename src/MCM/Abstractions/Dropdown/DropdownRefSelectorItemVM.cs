using MCM.Abstractions.Ref;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace MCM.Abstractions.Dropdown
{
    internal sealed class DropdownRefSelectorItemVM : DropdownSelectorItemVMBase
    {
        private readonly IRef _ref;

        [DataSourceProperty]
        public override string StringItem => _ref.Value?.ToString() ?? "ERROR";

        [DataSourceProperty]
        public override HintViewModel Hint { get; } = new HintViewModel(string.Empty);

        public DropdownRefSelectorItemVM(IRef @ref)
        {
            _ref = @ref;
        }

        public override string ToString() => _ref.Value?.ToString() ?? "ERROR";
    }
}