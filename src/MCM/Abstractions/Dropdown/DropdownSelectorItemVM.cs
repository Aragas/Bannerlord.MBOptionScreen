﻿using MCM.Utils;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace MCM.Abstractions.Dropdown
{
    // For Game's Dropdown implementation
    internal sealed class DropdownSelectorItemVM : DropdownSelectorItemVM<string>
    {
        public DropdownSelectorItemVM(string value) : base(value) { }
    }

    internal class DropdownSelectorItemVM<T> : DropdownSelectorItemVMBase where T : class
    {
        protected readonly T Object;

        [DataSourceProperty]
        public override string StringItem => Object.ToString() ?? "ERROR";

        [DataSourceProperty]
        public override HintViewModel Hint { get; } = HintViewModelUtils.Create();

        public DropdownSelectorItemVM(T @object)
        {
            Object = @object;
        }

        public override string ToString() => StringItem;
    }
}