using System;

namespace MBOptionScreen.Actions
{
    public sealed class ComplexAction<T> : IAction
    {
        public Ref Context { get; }
        public object Value { get; }
        public Action<T> DoFunction { get; }
        public Action<T> UndoFunction { get; }

        public ComplexAction(T value, Action<T> doFunction, Action<T> undoFunction)
        {
            Value = value!;
            DoFunction = doFunction;
            UndoFunction = undoFunction;
        }

        public void DoAction() => DoFunction?.Invoke((T) Value);
        public void UndoAction() => UndoFunction?.Invoke((T) Value);
    }
}