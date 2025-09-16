
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ComparerExtensions;

internal sealed class TypedKeyComparer<T, TKey> : KeyComparer<T>
{
    private readonly Func<T, TKey> _keySelector;
    private readonly IComparer<TKey> _keyComparer;

    public TypedKeyComparer(Func<T, TKey> keySelector, IComparer<TKey> keyComparer)
    {
        _keySelector = keySelector;
        _keyComparer = keyComparer;
    }

    public bool Descending { get; set; }

    public override int Compare(T x, T y)
    {
        var key1 = _keySelector(x);
        var key2 = _keySelector(y);
        return Descending ? _keyComparer.Compare(key2, key1) : _keyComparer.Compare(key1, key2);
    }
}