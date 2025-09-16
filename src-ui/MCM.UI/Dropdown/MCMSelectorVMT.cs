using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.UI.Dropdown;

internal class MCMSelectorVM<TSelectorItemVM, TSelectorItemVMValueType> : MCMSelectorVM<TSelectorItemVM>
    where TSelectorItemVM : MCMSelectorItemVM<TSelectorItemVMValueType>
    where TSelectorItemVMValueType : class
{
    public MCMSelectorVM(IEnumerable<TSelectorItemVMValueType> list, int selectedIndex)
    {
        Refresh(list, selectedIndex);
    }

    public void Refresh(IEnumerable<TSelectorItemVMValueType> list, int selectedIndex)
    {
        ItemList.Clear();

        _selectedIndex = -1;

        foreach (var @ref in list)
        {
            if (Activator.CreateInstance(typeof(TSelectorItemVM), @ref) is TSelectorItemVM val)
                ItemList.Add(val);
        }

        HasSingleItem = ItemList.Count <= 1;

        SelectedIndex = selectedIndex;
    }
}