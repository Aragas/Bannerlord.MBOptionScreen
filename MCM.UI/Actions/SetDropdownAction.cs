using TaleWorlds.Core.ViewModelCollection;

namespace MCM.UI.Actions
{
    // TODO: Not tested, reference is stored. Copy?
    internal sealed class SetDropdownAction : IAction
    {
        public Ref? Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetDropdownAction(Ref context, SelectorVM<SelectorItemVM> value)
        {
            Context = context;
            Value = value;
            Original = Context.Value;
        }

        public void DoAction() => Context!.Value = Value;
        public void UndoAction() => Context!.Value = Original;

        private struct SelectorStorage
        {
            public SelectorStorage(SelectorVM<SelectorItemVM> selector)
            {

            }
        }
    }
}