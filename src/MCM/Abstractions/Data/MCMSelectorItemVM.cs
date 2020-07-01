using MCM.Abstractions.Ref;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Data
{
    public class MCMSelectorItemVM : ViewModel
    {
        private string _stringItem = string.Empty;
        private bool _canBeSelected = true;
        private TextObject? _hintObj;
        private HintViewModel? _hint;

        public IRef Ref { get; }

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

        public MCMSelectorItemVM(IRef @ref)
        {
            Ref = @ref;
            RefreshValues();
        }
        public MCMSelectorItemVM(IRef @ref, TextObject hint)
        {
            Ref = @ref;
            _hintObj = hint;
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            _stringItem = Ref.Value.ToString();

            if (_hintObj != null)
                _hint = new HintViewModel(_hintObj.ToString());
        }
    }

    public class MCMSelectorItemVM<T> : MCMSelectorItemVM
    {
        private T _value;

        [DataSourceProperty]
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public MCMSelectorItemVM(IRef @ref) : base(@ref)
        {
            _value = (T) Ref.Value;
        }
        public MCMSelectorItemVM(IRef @ref, TextObject hint) : base(@ref, hint)
        {
            _value = (T) Ref.Value;
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            _value = (T) Ref.Value;
        }
    }
}