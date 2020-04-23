using System;
using System.Collections.Generic;

namespace MBOptionScreen.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Applies the action to all elements in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action?.Invoke(item);
                yield return item;
            }
        }
    }
}