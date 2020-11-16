using MCM.Abstractions.Ref;

namespace MCM.UI.Actions
{
    internal sealed class SetStringAction : IAction
    {
        public IRef Context { get; }
        public object? Value { get; }
        public object? Original { get; }

        public SetStringAction(IRef context, string value)
        {
            Context = context;
            Value = value;
            Original = Context.Value;
        }

        public void DoAction() => Context.Value = Value;
        public void UndoAction() => Context.Value = Original;
    }
}