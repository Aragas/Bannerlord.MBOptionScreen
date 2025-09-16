﻿
using System.Collections;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ComparerExtensions;

/// <summary>
/// Compares items such that they are always equal.
/// </summary>
/// <typeparam name="T">The type of the items to compare.</typeparam>
internal sealed class NullComparer<T> : IComparer<T>, IComparer
{
    private NullComparer()
    {
    }

    /// <summary>
    /// Gets the default instance of a NullComparer.
    /// </summary>
    public static NullComparer<T> Default { get; } = new();

    /// <summary>
    /// Always returns zero, indicating that the two values are equal.
    /// </summary>
    /// <param name="x">The first value.</param>
    /// <param name="y">The second value.</param>
    /// <returns>Zero, indicating that the two values are equal.</returns>
    public int Compare(T x, T y) => 0;

    int IComparer.Compare(object? x, object? y) => x is T x1 && y is T y1 ? Compare(x1, y1) : 0;
}