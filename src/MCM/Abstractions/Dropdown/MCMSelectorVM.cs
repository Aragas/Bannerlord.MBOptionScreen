using HarmonyLib;

using MCM.Extensions;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;

namespace MCM.Abstractions.Dropdown
{
    /// <summary>
    /// TODO:
    /// </summary>
    /// <typeparam name="TSelectorItemVM"><see cref="ViewModel"/> that accepts the objects passed by .ctor/Refresh</typeparam>
    public class MCMSelectorVM<TSelectorItemVM> : ViewModel where TSelectorItemVM : ViewModel
    {
        private delegate bool CanBeSelectedDelegate(TSelectorItemVM instance);

        private static readonly CanBeSelectedDelegate _canBeSelectedDelegate;

        static MCMSelectorVM()
        {
            var methodInfo = AccessTools.Property(typeof(TSelectorItemVM), "CanBeSelected")?.GetMethod;
            _canBeSelectedDelegate = methodInfo is not null
                ? AccessTools3.GetDelegate<CanBeSelectedDelegate>(methodInfo)!
                : _ => false;
        }

        public static MCMSelectorVM<TSelectorItemVM> Empty => new(null);

        protected Action<MCMSelectorVM<TSelectorItemVM>>? _onChange;
        private MBBindingList<TSelectorItemVM> _itemList = new();
        protected int _selectedIndex = -1;
        private TSelectorItemVM? _selectedItem;
        private bool _hasSingleItem;

        [DataSourceProperty]
        public MBBindingList<TSelectorItemVM> ItemList
        {
            get => _itemList;
            set
            {
                if (value != _itemList)
                {
                    _itemList = value;
                    OnPropertyChangedWithValue(value, nameof(ItemList));
                }
            }
        }

        [DataSourceProperty]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value != _selectedIndex)
                {
                    _selectedIndex = value;
                    OnPropertyChangedWithValue(value, nameof(SelectedIndex));
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
                if (value != _selectedItem)
                {
                    _selectedItem = value;
                    OnPropertyChangedWithValue(value, nameof(SelectedItem));
                }
            }
        }

        [DataSourceProperty]
        public bool HasSingleItem
        {
            get => _hasSingleItem;
            set
            {
                if (value != _hasSingleItem)
                {
                    _hasSingleItem = value;
                    OnPropertyChangedWithValue(value, nameof(HasSingleItem));
                }
            }
        }

        public MCMSelectorVM(Action<MCMSelectorVM<TSelectorItemVM>>? onChange)
        {
            HasSingleItem = true;
            _onChange = onChange;
        }
        public MCMSelectorVM(IEnumerable<object> list, int selectedIndex, Action<MCMSelectorVM<TSelectorItemVM>>? onChange)
        {
            Refresh(list, selectedIndex, onChange);
        }


        /// <inheritdoc/>
        public override void RefreshValues()
        {
            base.RefreshValues();

            _itemList.ApplyActionOnAllItems(x => x.RefreshValues());
        }

        public void Refresh(IEnumerable<object> list, int selectedIndex, Action<MCMSelectorVM<TSelectorItemVM>>? onChange)
        {
            ItemList.Clear();

            _selectedIndex = -1;

            // Check that TSelectorItemVM has a constructor that accepts the object

            foreach (var obj in list)
            {
                if (Activator.CreateInstance(typeof(TSelectorItemVM), obj) is TSelectorItemVM val)
                    ItemList.Add(val);
            }

            HasSingleItem = ItemList.Count <= 1;

            _onChange = onChange;

            SelectedIndex = selectedIndex;
        }

        public TSelectorItemVM? GetCurrentItem()
        {
            if (_itemList.Count > 0 && SelectedIndex >= 0 && SelectedIndex < _itemList.Count)
            {
                return _itemList[SelectedIndex];
            }
            return null;
        }

        public void SetOnChangeAction(Action<MCMSelectorVM<TSelectorItemVM>> onChange) => _onChange = onChange;

        public void AddItem(TSelectorItemVM item)
        {
            ItemList.Add(item);
            HasSingleItem = ItemList.Count <= 1;
        }

        public void ExecuteRandomize()
        {
            if (ItemList.Any(i => _canBeSelectedDelegate(i)))
            {
                var randomElement = ItemList.Where(i => _canBeSelectedDelegate(i)).GetRandomElementInefficiently();
                SelectedIndex = ItemList.IndexOf(randomElement);
            }
        }

        public void ExecuteSelectNextItem()
        {
            if (ItemList.Count > 0)
            {
                for (var num = (SelectedIndex + 1) % ItemList.Count; num != SelectedIndex; num = (num + 1) % ItemList.Count)
                {
                    if (_canBeSelectedDelegate(ItemList[num]))
                    {
                        SelectedIndex = num;
                        return;
                    }
                }
            }
        }

        public void ExecuteSelectPreviousItem()
        {
            if (ItemList.Count > 0)
            {
                for (var num = SelectedIndex - 1 >= 0 ? SelectedIndex - 1 : ItemList.Count - 1; num != SelectedIndex; num = num - 1 >= 0 ? num - 1 : ItemList.Count - 1)
                {
                    if (_canBeSelectedDelegate(ItemList[num]))
                    {
                        SelectedIndex = num;
                        return;
                    }
                }
            }
        }
    }
}