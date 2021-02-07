/*
using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace MCM.Abstractions.Dropdown
{
    internal sealed class CheckboxDropdownSelectorItemVM<T> : DropdownSelectorItemVM<T> where T : class
    {
        private delegate bool GetIsSelectedDelegate();
        private delegate void SetIsSelectedDelegate(bool value);

        private readonly GetIsSelectedDelegate _getIsSelectedDelegate;
        private readonly SetIsSelectedDelegate _setIsSelectedDelegate;

        [DataSourceProperty]
        public override string StringItem { get; }

        [DataSourceProperty]
        public override HintViewModel Hint { get; }

        [DataSourceProperty]
        public bool IsSelected
        {
            get => _getIsSelectedDelegate();
            set
            {
                if (value != _getIsSelectedDelegate())
                {
                    _setIsSelectedDelegate(value);
                    OnPropertyChangedWithValue(value, nameof(IsSelected));
                }
            }
        }

        public CheckboxDropdownSelectorItemVM(T @object) : base(@object)
        {
            var isSelectedProperty = AccessTools.Property(typeof(T), "IsSelected");
            if (isSelectedProperty is not null)
            {
                _getIsSelectedDelegate = AccessTools2.GetDelegate<GetIsSelectedDelegate>(@object, isSelectedProperty.GetMethod)!;
                _setIsSelectedDelegate = AccessTools2.GetDelegate<SetIsSelectedDelegate>(@object, isSelectedProperty.SetMethod)!;
            }
            else
            {
                _getIsSelectedDelegate = () => false;
                _setIsSelectedDelegate = _ => { };
            }

            StringItem = AccessTools.Property(typeof(T), "Name")?.GetValue(@object) is string name ? name : @object.ToString() ?? "ERROR";

            Hint = AccessTools.Property(typeof(T), "HintText")?.GetValue(@object) is string hintText ? new HintViewModel(hintText) : new HintViewModel(string.Empty);
        }
    }
}
*/