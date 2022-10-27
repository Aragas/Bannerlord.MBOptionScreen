using System;
using System.Collections.Generic;

using TaleWorlds.Library;

namespace MCM.UI.Dropdown
{
    public class MCMSelectorVM<TSelectorItemVM, TSelectorItemVMValueType> : MCMSelectorVM<TSelectorItemVM>
        where TSelectorItemVM : ViewModel //DropdownSelectorItemVM<TSelectorItemVMValueType>
        where TSelectorItemVMValueType : class
    {
        public MCMSelectorVM(Action<MCMSelectorVM<TSelectorItemVM>>? onChange) : base(onChange) { }

        public MCMSelectorVM(IEnumerable<TSelectorItemVMValueType> list, int selectedIndex, Action<MCMSelectorVM<TSelectorItemVM>>? onChange) : base(onChange)
        {
            Refresh(list, selectedIndex, onChange);
        }

        public void Refresh(IEnumerable<TSelectorItemVMValueType> list, int selectedIndex, Action<MCMSelectorVM<TSelectorItemVM>>? onChange)
        {
            ItemList.Clear();

            _selectedIndex = -1;

            foreach (var @ref in list)
            {
                if (Activator.CreateInstance(typeof(TSelectorItemVM), @ref) is TSelectorItemVM val)
                    ItemList.Add(val);
            }

            HasSingleItem = ItemList.Count <= 1;

            _onChange = onChange;

            SelectedIndex = selectedIndex;
        }
    }
}