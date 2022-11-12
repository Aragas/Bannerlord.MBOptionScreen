namespace MCM.UI.Dropdown
{
    public sealed class MCMSelectorItemVM : MCMSelectorItemVM<string>
    {
        public MCMSelectorItemVM(string value) : base(value) { }
    }

    public class MCMSelectorItemVM<T> : MCMSelectorItemVMBase where T : class
    {
        public T OriginalItem { get; }

        public MCMSelectorItemVM(T @object)
        {
            OriginalItem = @object;
            RefreshValues();
        }

        public override string? ToString() => StringItem;

        public override void RefreshValues()
        {
            base.RefreshValues();
            _stringItem = OriginalItem.ToString() ?? "ERROR";
        }
    }
}