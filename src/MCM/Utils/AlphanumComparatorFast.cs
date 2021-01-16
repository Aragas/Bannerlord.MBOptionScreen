using System.Collections;
using System.Collections.Generic;

namespace MCM.Utils
{
    public class AlphanumComparatorFast : IComparer<string?>, IComparer
    {
        public int Compare(object? x, object? y) => Compare(x as string, y as string);
        public int Compare(string? s1, string? s2) => 0;
    }
}