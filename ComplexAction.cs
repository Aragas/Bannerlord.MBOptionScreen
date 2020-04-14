using ModLib.Interfaces;
using System;

namespace ModLib
{
    public class ComplexAction<T> : IAction
    {
        public Ref Context { get; private set; }

        public object Value { get; private set; }

        public Action<T> DoFunction { get; private set; }

        public Action<T> UndoFunction { get; private set; }

        public ComplexAction(T value, Action<T> doFunction, Action<T> undoFunction)
        {
            Value = value;
            DoFunction = doFunction;
            UndoFunction = undoFunction;
        }

        public void Do()
        {
            DoFunction?.Invoke((T)Value);
        }

        public void Undo()
        {
            UndoFunction?.Invoke((T)Value);
        }
    }
}
