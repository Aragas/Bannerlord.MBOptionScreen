using System;
using System.Collections.Generic;
using System.Linq;

using MCM.Abstractions.Ref;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace MCM.Abstractions.Data
{
    public class MCMSelectorVM<TSelectorItemVM> : ViewModel where TSelectorItemVM : MCMSelectorItemVM
    {
        private Action<MCMSelectorVM<TSelectorItemVM>>? _onChange;
        private MBBindingList<TSelectorItemVM> _itemList = new MBBindingList<TSelectorItemVM>(); 
        private int _selectedIndex = -1;
        private TSelectorItemVM? _selectedItem;
        private bool _hasSingleItem;

        [DataSourceProperty]
        public MBBindingList<TSelectorItemVM> ItemList 
        {
            get => _itemList;
            set
            {
                if (!Equals(value, _itemList))
                {
                    _itemList = value;
                    OnPropertyChanged(nameof(ItemList));
                }
            }
        }

        [DataSourceProperty]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (!value.Equals(_selectedIndex))
                {
                    _selectedIndex = value;
                    OnPropertyChanged(nameof(SelectedIndex));
                    SelectedItem = GetCurrentItem();
                    _onChange?.Invoke(this);
                }
            }
        }

        [DataSourceProperty]
        public TSelectorItemVM? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!Equals(value, _selectedItem))
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        [DataSourceProperty]
        public bool HasSingleItem
        {
            get => _hasSingleItem;
            set
            {
                if (!value.Equals(_hasSingleItem))
                {
                    _hasSingleItem = value;
                    OnPropertyChanged(nameof(HasSingleItem));
                }
            }
        }


        public MCMSelectorVM(int selectedIndex, Action<MCMSelectorVM<TSelectorItemVM>>? onChange)
        {
            HasSingleItem = true;
            _onChange = onChange;
        }
        public MCMSelectorVM(IEnumerable<IRef> list, int selectedIndex, Action<MCMSelectorVM<TSelectorItemVM>>? onChange)
        {
            Refresh(list, selectedIndex, onChange);
        }


        public override void RefreshValues()
        {
            base.RefreshValues();

            _itemList.ApplyActionOnAllItems(x => x.RefreshValues());
        }

        public void Refresh(IEnumerable<IRef> list, int selectedIndex, Action<MCMSelectorVM<TSelectorItemVM>>? onChange)
        {
            ItemList.Clear();

            _selectedIndex = -1;

            foreach (var @ref in list)
                ItemList.Add((TSelectorItemVM) Activator.CreateInstance(typeof(TSelectorItemVM), @ref));

            HasSingleItem = ItemList.Count <= 1;

            _onChange = onChange;

            SelectedIndex = selectedIndex;
        }

        public TSelectorItemVM? GetCurrentItem()
        {
            if (_itemList != null && _itemList.Count > 0 && SelectedIndex >= 0 && SelectedIndex < _itemList.Count)
            {
                return _itemList[SelectedIndex];
            }
            return null;
        }

        public void SetOnChangeAction(Action<MCMSelectorVM<TSelectorItemVM>> onChange) => _onChange = onChange;

        public void AddItem(TSelectorItemVM item)
        {
            ItemList.Add(item);
            HasSingleItem = (ItemList.Count <= 1);
        }

        public void ExecuteRandomize()
        {
            if (ItemList != null && (ItemList.Count(i => i.CanBeSelected) > 0))
            {
                var randomElement = (ItemList.Where(i => i.CanBeSelected)).GetRandomElement();
                SelectedIndex = ItemList.IndexOf(randomElement);
            }
        }

        public void ExecuteSelectNextItem()
        {
            if (ItemList != null && ItemList.Count > 0)
            {
                for (var num = (SelectedIndex + 1) % ItemList.Count; num != SelectedIndex; num = (num + 1) % ItemList.Count)
                {
                    if (ItemList[num].CanBeSelected)
                    {
                        SelectedIndex = num;
                        return;
                    }
                }
            }
        }

        public void ExecuteSelectPreviousItem()
        {
            if (ItemList != null && ItemList.Count > 0)
            {
                for (var num = (SelectedIndex - 1 >= 0) ? (SelectedIndex - 1) : (ItemList.Count - 1); num != SelectedIndex; num = ((num - 1 >= 0) ? (num - 1) : (ItemList.Count - 1)))
                {
                    if (ItemList[num].CanBeSelected)
                    {
                        SelectedIndex = num;
                        return;
                    }
                }
            }
        }
    }
}