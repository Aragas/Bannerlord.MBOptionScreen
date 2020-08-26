using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Dropdown
{
    public class MCMSelectorItemVM : ViewModel
    {
        private string _stringItem = string.Empty;
        private bool _canBeSelected = true;
        private TextObject? _hintObj;
        private HintViewModel? _hint;

        public object Element { get; }

        [DataSourceProperty]
        public string StringItem
        {
            get => _stringItem;
            set
            {
                if (value != _stringItem)
                {
                    _stringItem = value;
                    OnPropertyChanged(nameof(StringItem));
                }
            }
        }

        [DataSourceProperty]
        public bool CanBeSelected
        {
            get => _canBeSelected;
            set
            {
                if (value != _canBeSelected)
                {
                    _canBeSelected = value;
                    OnPropertyChanged(nameof(CanBeSelected));
                }
            }
        }

        [DataSourceProperty]
        public HintViewModel? Hint
        {
            get => _hint;
            set
            {
                if (!Equals(value, _hint))
                {
                    _hint = value;
                    OnPropertyChanged(nameof(Hint));
                }
            }
        }

        public MCMSelectorItemVM(object element)
        {
            Element = element;
            RefreshValues();
        }
        public MCMSelectorItemVM(object element, TextObject hint)
        {
            Element = element;
            _hintObj = hint;
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            _stringItem = Element.ToString();

            if (_hintObj != null)
                _hint = new HintViewModel(_hintObj.ToString());
        }
    }
}