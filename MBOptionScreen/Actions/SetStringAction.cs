namespace MBOptionScreen.Actions
{
    public sealed class SetStringAction : IAction
    {
        public Ref? Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetStringAction(Ref context, string value)
        {
            Context = context;
            Value = value!;
            Original = Context.Value;
        }

        public void DoAction() => Context!.Value = Value;
        public void UndoAction() => Context!.Value = Original;
    }
}