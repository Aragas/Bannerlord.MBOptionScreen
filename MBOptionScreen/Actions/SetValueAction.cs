namespace MBOptionScreen.Actions
{
    public sealed class SetValueAction<T> : IAction
    {
        public Ref Context { get; }
        public object Value { get; }
        private  T Original { get; }

        public SetValueAction(Ref context, T value)
        {
            Context = context;
            Value = value;
            Original = (T) Context.Value;
        }

        public void DoAction() => Context.Value = Value;
        public void UndoAction() => Context.Value = Original;
    }
}