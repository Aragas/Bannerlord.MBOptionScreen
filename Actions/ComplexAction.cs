using MBOptionScreen.Interfaces;

using System;

namespace MBOptionScreen.Actions
{
    public class ComplexAction<T> : IAction
    {
        public Ref Context { get; }
        public object Value { get; }
        public Action<T> DoFunction { get; }
        public Action<T> UndoFunction { get; }

        public ComplexAction(T value, Action<T> doFunction, Action<T> undoFunction)
        {
            Value = value;
            DoFunction = doFunction;
            UndoFunction = undoFunction;
        }

        public void Do() => DoFunction?.Invoke((T) Value);
        public void Undo() => UndoFunction?.Invoke((T) Value);
    }
}
