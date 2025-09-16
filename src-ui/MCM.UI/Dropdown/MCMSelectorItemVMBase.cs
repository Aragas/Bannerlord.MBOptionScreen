using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace MCM.UI.Dropdown;

internal abstract class MCMSelectorItemVMBase : ViewModel
{
    protected string? _stringItem;
    protected bool _canBeSelected = true;
    protected HintViewModel? _hint;
    protected bool _isSelected;

    [DataSourceProperty]
    public bool CanBeSelected { get => _canBeSelected; set => SetField(ref _canBeSelected, value, nameof(CanBeSelected)); }

    [DataSourceProperty]
    public string? StringItem { get => _stringItem; set => SetField(ref _stringItem, value, nameof(StringItem)); }

    [DataSourceProperty]
    public HintViewModel? Hint { get => _hint; set => SetField(ref _hint, value, nameof(Hint)); }

    [DataSourceProperty]
    public bool IsSelected { get => _isSelected; set => SetField(ref _isSelected, value, nameof(IsSelected)); }
}