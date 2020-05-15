using MCM.Abstractions.Ref;

using System;

namespace MCM.UI.Actions
{
    internal sealed class ComplexValueTypeAction<T> : IAction where T : struct
    {
        public IRef Context { get; }
        public object Value => DoFunction((T)Context.Value);
        public object Original => UndoFunction((T)Context.Value);
        private Func<T, T> DoFunction { get; }
        private Func<T, T> UndoFunction { get; }

        /// <summary>
        /// new ProxyRef(() => ValueType, o => ValueType = o)
        /// </summary>
        public ComplexValueTypeAction(IRef context, Func<T, T> doFunction, Func<T, T> undoFunction)
        {
            Context = context;
            DoFunction = doFunction;
            UndoFunction = undoFunction;
        }

        public void DoAction() => Context.Value = Value;
        public void UndoAction() => Context.Value = Original;
    }
}