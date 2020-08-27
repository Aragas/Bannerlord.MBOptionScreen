using System;
using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class TypedKeyComparer<T, TKey> : KeyComparer<T>
    {
        private readonly Func<T, TKey> keySelector;
        private readonly IComparer<TKey> keyComparer;

        public TypedKeyComparer(Func<T, TKey> keySelector, IComparer<TKey> keyComparer)
        {
            this.keySelector = keySelector;
            this.keyComparer = keyComparer;
        }

        public bool Descending { get; set; }

        public override int Compare(T x, T y)
        {
            var key1 = keySelector(x);
            var key2 = keySelector(y);
            return Descending ? keyComparer.Compare(key2, key1) : keyComparer.Compare(key1, key2);
        }
    }
}