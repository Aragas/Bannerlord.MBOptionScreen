using System;
using System.Collections.Generic;

namespace ModLib
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Do<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null)
            {
                foreach (var item in enumerable)
                    action?.Invoke(item);
                return enumerable;
            }
            else
                return null;
        }
    }
}