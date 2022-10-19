using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Dropdown;

using System;

using TaleWorlds.Library;

namespace MCM.Abstractions.Common.ViewModelWrappers
{
    /// <summary>
    /// A complex wrapper that replaces any Selector ViewModel, just needs the right properties
    /// </summary>
    public class SelectorVMWrapper : ViewModelWrapper
    {
        private delegate int GetSelectedIndexDelegate();
        private delegate void SetSelectedIndexDelegate(int value);
        private delegate IMBBindingList GetItemListDelegate();
        private delegate void SetItemListDelegate(IMBBindingList? value);
        private delegate bool GetHasSingleItemDelegate();
        private delegate void SetHasSingleItemDelegate(bool value);
        private delegate object GetSelectedItemDelegate();
        private delegate void SetSelectedItemDelegate(object value);
        private delegate void SetOnChangeActionDelegate(object value);

        private readonly GetSelectedIndexDelegate? _getSelectedIndexDelegate;
        private readonly SetSelectedIndexDelegate? _setSelectedIndexDelegate;
        private readonly GetItemListDelegate? _getItemList;
        private readonly SetItemListDelegate? _setItemList;
        private readonly GetHasSingleItemDelegate? _getHasSingleItem;
        private readonly SetHasSingleItemDelegate? _setHasSingleItem;
        private readonly GetSelectedItemDelegate? _getSelectedItem;
        private readonly SetSelectedItemDelegate? _setSelectedItem;
        private readonly SetOnChangeActionDelegate? _setOnChangeAction;

        public int SelectedIndex
        {
            get => _getSelectedIndexDelegate?.Invoke() ?? -1;
            set => _setSelectedIndexDelegate?.Invoke(value);
        }

        public IMBBindingList? ItemList
        {
            get => _getItemList?.Invoke();
            set => _setItemList?.Invoke(value);
        }

        public bool HasSingleItem
        {
            get => _getHasSingleItem?.Invoke() ?? false;
            set => _setHasSingleItem?.Invoke(value);
        }

        public object SelectedItem
        {
            get => _getSelectedItem?.Invoke() ?? default!;
            set => _setSelectedItem?.Invoke(value);
        }

        public SelectorVMWrapper(object? @object) : base(@object ?? MCMSelectorVM<MCMSelectorItemVM>.Empty)
        {
            @object ??= MCMSelectorVM<MCMSelectorItemVM>.Empty;
            var type = @object.GetType();

            _getSelectedIndexDelegate = AccessTools2.GetPropertyGetterDelegate<GetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex));
            _setSelectedIndexDelegate = AccessTools2.GetPropertySetterDelegate<SetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex));
            _getItemList = AccessTools2.GetPropertyGetterDelegate<GetItemListDelegate>(@object, type, nameof(ItemList));
            _setItemList = AccessTools2.GetPropertySetterDelegate<SetItemListDelegate>(@object, type, nameof(ItemList));
            _getHasSingleItem = AccessTools2.GetPropertyGetterDelegate<GetHasSingleItemDelegate>(@object, type, nameof(HasSingleItem));
            _setHasSingleItem = AccessTools2.GetPropertySetterDelegate<SetHasSingleItemDelegate>(@object, type, nameof(HasSingleItem));
            _getSelectedItem = AccessTools2.GetPropertyGetterDelegate<GetSelectedItemDelegate>(@object, type, nameof(SelectedItem));
            _setSelectedItem = AccessTools2.GetPropertySetterDelegate<SetSelectedItemDelegate>(@object, type, nameof(SelectedItem));
            _setOnChangeAction = AccessTools2.GetDelegate<SetOnChangeActionDelegate>(@object, type, nameof(SetOnChangeAction));
        }

        public void SetOnChangeAction(Action<SelectorVMWrapper>? onChangeAction)
        {
            void SelectorSetOnChangeAction(object selectorVM)
            {
                onChangeAction?.Invoke(this);
            }

            _setOnChangeAction?.Invoke((Action<object>) SelectorSetOnChangeAction);
        }
    }
}