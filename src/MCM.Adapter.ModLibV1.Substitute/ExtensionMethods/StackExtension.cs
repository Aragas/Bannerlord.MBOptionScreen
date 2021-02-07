using System;
using System.Collections.Generic;

namespace ModLib
{
    public static class StackExtension
    {
        public static void AppendToTop<T>(this Stack<T> baseStack, Stack<T> toAppend)
        {
            if (toAppend == null) throw new ArgumentNullException(nameof(toAppend));
            if (baseStack == null) throw new ArgumentNullException(nameof(baseStack));
            if (toAppend.Count == 0)
                return;

            var array = toAppend.ToArray();
            for (var i = array.Length - 1; i >= 0; i--)
            {
                baseStack.Push(array[i]);
            }
        }
    }
}