using System;

namespace MCM.UI.Actions
{
    internal sealed class ComplexReferenceTypeAction<T> : IAction
        where T : class
    {
        public IRef? Context { get; }
        public object Value { get; }
        public object Original { get; }
        private Action<T> DoFunction { get; }
        private Action<T> UndoFunction { get; }

        public ComplexReferenceTypeAction(T value, Action<T> doFunction, Action<T> undoFunction)
        {
            Value = value;
            Original = value;
            DoFunction = doFunction;
            UndoFunction = undoFunction;
        }

        public void DoAction() => DoFunction?.Invoke((T)Value);
        public void UndoAction() => UndoFunction?.Invoke((T)Value);
    }
}