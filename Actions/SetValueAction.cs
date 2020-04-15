using MBOptionScreen.Interfaces;

namespace MBOptionScreen.Actions
{
    public class SetValueAction<T> : IAction where T : struct
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

        public void Do() => Context.Value = Value;
        public void Undo() => Context.Value = Original;
    }
}
