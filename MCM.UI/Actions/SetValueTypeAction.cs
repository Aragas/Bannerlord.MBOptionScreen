namespace MCM.UI.Actions
{
    internal sealed class SetValueTypeAction<T> : IAction
        where T : struct
    {
        public IRef Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetValueTypeAction(IRef context, T value)
        {
            Context = context;
            Value = value;
            Original = Context.Value;
        }

        public void DoAction() => Context.Value = Value;
        public void UndoAction() => Context.Value = Original;
    }
}