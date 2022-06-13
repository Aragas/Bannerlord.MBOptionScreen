using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Common.Wrappers;

using System;

using TaleWorlds.Library;

namespace MCM.Abstractions.Common.ViewModelWrappers
{
    /// <summary>
    /// A complex wrapper that replaces any Selector ViewModel, just needs the right properties
    /// </summary>
    public class SelectorVMWrapper : ViewModelWrapper
    {
        private delegate IMBBindingList GetItemListDelegate();
        private delegate void SetItemListDelegate(IMBBindingList value);
        private delegate bool GetHasSingleItemDelegate();
        private delegate void SetHasSingleItemDelegate(bool value);
        private delegate object GetSelectedItemDelegate();
        private delegate void SetSelectedItemDelegate(object value);
        private delegate void SetOnChangeActionDelegate(object value);

        private readonly GetItemListDelegate? _getItemList;
        private readonly SetItemListDelegate? _setItemList;
        private readonly GetHasSingleItemDelegate? _getHasSingleItem;
        private readonly SetHasSingleItemDelegate? _setHasSingleItem;
        private readonly GetSelectedItemDelegate? _getSelectedItem;
        private readonly SetSelectedItemDelegate? _setSelectedItem;
        private readonly SetOnChangeActionDelegate? _setOnChangeAction;

        public int SelectedIndex
        {
            get => new SelectedIndexWrapper(Object).SelectedIndex;
            set
            {
                var selectedIndexWrapper = new SelectedIndexWrapper(Object);
                selectedIndexWrapper.SelectedIndex = value;
            }
        }

        public IMBBindingList ItemList
        {
            get => _getItemList?.Invoke() ?? default!;
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

        public SelectorVMWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            _getItemList = AccessTools2.GetDeclaredPropertyGetterDelegate<GetItemListDelegate>(@object, type, nameof(ItemList));
            _setItemList = AccessTools2.GetDeclaredPropertySetterDelegate<SetItemListDelegate>(@object, type, nameof(ItemList));
            _getHasSingleItem = AccessTools2.GetDeclaredPropertyGetterDelegate<GetHasSingleItemDelegate>(@object, type, nameof(HasSingleItem));
            _setHasSingleItem = AccessTools2.GetDeclaredPropertySetterDelegate<SetHasSingleItemDelegate>(@object, type, nameof(HasSingleItem));
            _getSelectedItem = AccessTools2.GetDeclaredPropertyGetterDelegate<GetSelectedItemDelegate>(@object, type, nameof(SelectedItem));
            _setSelectedItem = AccessTools2.GetDeclaredPropertySetterDelegate<SetSelectedItemDelegate>(@object, type, nameof(SelectedItem));
            _setOnChangeAction = AccessTools2.GetDeclaredDelegate<SetOnChangeActionDelegate>(@object, type, nameof(SetOnChangeAction));
        }

        public void SetOnChangeAction(Action<SelectorVMWrapper>? onPresetsSelectorChange)
        {
            void SelectorSetOnChangeAction(object selectorVM)
            {
                if (onPresetsSelectorChange is not null)
                {
                    onPresetsSelectorChange(this);
                }
            }

            if (_setOnChangeAction is not null)
            {
                _setOnChangeAction((Action<object>)((x) => SelectorSetOnChangeAction(x)));
            }
        }
    }
}