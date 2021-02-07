using MCM.Abstractions.Common;
using MCM.Abstractions.Ref;

namespace MCM.UI.Actions
{
    internal sealed class SetSelectedIndexAction : IAction
    {
        private IRef SelectedIndexContext { get; }
        public IRef Context { get; }
        public object? Value { get; }
        public object? Original { get; }

        public SetSelectedIndexAction(IRef context, object value)
        {
            Context = context;

            SelectedIndexContext = new ProxyRef<int>(
                () => Context.Value is not null ? new SelectedIndexWrapper(Context.Value).SelectedIndex : 0,
                o =>
                {
                    if (Context.Value is not null)
                        new SelectedIndexWrapper(Context.Value).SelectedIndex = o;
                });
            Value = new SelectedIndexWrapper(value).SelectedIndex;
            Original = SelectedIndexContext.Value;
        }

        public void DoAction() => SelectedIndexContext.Value = Value;
        public void UndoAction() => SelectedIndexContext.Value = Original;
    }
}