using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace MCM.UI.Dropdown
{
    internal class MCMSelectorVM<TSelectorItemVM> : ViewModel where TSelectorItemVM : ViewModel
    {
        private delegate bool CanBeSelectedDelegate(TSelectorItemVM instance);
        private static readonly CanBeSelectedDelegate _canBeSelectedDelegate =
            AccessTools2.GetPropertyGetterDelegate<CanBeSelectedDelegate>(typeof(TSelectorItemVM), "CanBeSelected") ?? (_ => false);

        private delegate void SetIsSelectedDelegate(object instance, bool value);
        private static readonly SetIsSelectedDelegate _setIsSelectedDelegate =
            AccessTools2.GetPropertySetterDelegate<SetIsSelectedDelegate>(typeof(TSelectorItemVM), "IsSelected") ?? ((_, _) => { });


        public static MCMSelectorVM<TSelectorItemVM> Empty => new();

        protected int _selectedIndex = -1;
        private TSelectorItemVM? _selectedItem;
        private bool _hasSingleItem;

        [DataSourceProperty]
        public MBBindingList<TSelectorItemVM> ItemList { get; } = new();

        [DataSourceProperty]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (SetField(ref _selectedIndex, value, nameof(SelectedIndex)))
                {
                    if (SelectedItem != null)
                        _setIsSelectedDelegate(SelectedItem, false);

                    SelectedItem = GetCurrentItem();
                    if (SelectedItem != null)
                        _setIsSelectedDelegate(SelectedItem, true);
                }
            }
        }

        [DataSourceProperty]
        public TSelectorItemVM? SelectedItem { get => _selectedItem; set => SetField(ref _selectedItem, value, nameof(SelectedItem)); }

        [DataSourceProperty]
        public bool HasSingleItem { get => _hasSingleItem; set => SetField(ref _hasSingleItem, value, nameof(HasSingleItem)); }

        public MCMSelectorVM()
        {
            HasSingleItem = true;
        }
        public MCMSelectorVM(IEnumerable<object> list, int selectedIndex, Action<MCMSelectorVM<TSelectorItemVM>>? onChange)
        {
            Refresh(list, selectedIndex, onChange);
        }


        /// <inheritdoc/>
        public override void RefreshValues()
        {
            base.RefreshValues();

            ItemList.ApplyActionOnAllItems(x => x.RefreshValues());
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

            SelectedIndex = selectedIndex;
        }

        public TSelectorItemVM? GetCurrentItem()
        {
            if (ItemList.Count > 0 && SelectedIndex >= 0 && SelectedIndex < ItemList.Count)
            {
                return ItemList[SelectedIndex];
            }
            return null;
        }

        public void AddItem(TSelectorItemVM item)
        {
            ItemList.Add(item);
            HasSingleItem = ItemList.Count <= 1;
        }

        public void ExecuteRandomize()
        {
            if (ItemList.GetRandomElementWithPredicate(i => _canBeSelectedDelegate(i)) is { } element)
            {
                SelectedIndex = ItemList.IndexOf(element);
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