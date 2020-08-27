using MCM.Abstractions.Common;
using MCM.Abstractions.Ref;

namespace MCM.UI.Actions
{
    internal sealed class SetSelectedIndexAction : IAction
    {
        private IRef DropdownContext { get; }
        public IRef Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetSelectedIndexAction(IRef context, object value)
        {
            DropdownContext = context;

            Context = new ProxyRef<int>(
                () => new SelectedIndexWrapper(DropdownContext.Value).SelectedIndex,
                o => new SelectedIndexWrapper(DropdownContext.Value).SelectedIndex = o);
            Value = new SelectedIndexWrapper(value).SelectedIndex;
            Original = Context.Value;
        }

        public void DoAction() => Context.Value = Value;
        public void UndoAction() => Context.Value = Original;
    }
}