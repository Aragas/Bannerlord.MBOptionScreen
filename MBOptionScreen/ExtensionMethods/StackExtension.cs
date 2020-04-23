using System.Collections.Generic;

namespace MBOptionScreen.ExtensionMethods
{
    public static class StackExtension
    {
        public static void AppendToTop<T>(this Stack<T> baseStack, Stack<T> toAppend)
        {
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