﻿using MCM.Common;

using System;

namespace MCM.UI.Actions;

internal sealed class ComplexReferenceTypeAction<T> : IAction where T : class
{
    public IRef Context { get; }
    public object? Value
    {
        get
        {
            DoFunction(Context.Value as T);
            return Context.Value;
        }
    }
    public object? Original
    {
        get
        {
            UndoFunction(Context.Value as T);
            return Context.Value;
        }
    }
    private Action<T?> DoFunction { get; }
    private Action<T?> UndoFunction { get; }

    /// <summary>
    /// new ProxyRef(() => Value, null)
    /// </summary>
    public ComplexReferenceTypeAction(IRef context, Action<T?> doFunction, Action<T?> undoFunction)
    {
        Context = context;
        DoFunction = doFunction;
        UndoFunction = undoFunction;
    }

    public void DoAction() => Context.Value = Value;
    public void UndoAction() => Context.Value = Original;
}