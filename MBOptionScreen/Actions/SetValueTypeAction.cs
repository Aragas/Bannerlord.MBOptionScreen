namespace MBOptionScreen.Actions
{

    public sealed class SetValueTypeAction<T> : IAction
        where T : struct
    {
        public Ref? Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetValueTypeAction(Ref context, T value)
        {
            Context = context;
            Value = value!;
            Original = Context.Value;
        }

        public void DoAction() => Context!.Value = Value;
        public void UndoAction() => Context!.Value = Original;
    }
}