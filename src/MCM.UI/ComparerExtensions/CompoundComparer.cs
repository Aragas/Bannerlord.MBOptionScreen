using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class CompoundComparer<T> : Comparer<T>
    {
        private readonly List<IComparer<T>> comparers;

        public static IComparer<T> GetComparer(IComparer<T> baseComparer, IComparer<T> nextComparer)
        {
            var comparer = new CompoundComparer<T>();
            comparer.AppendComparison(baseComparer);
            comparer.AppendComparison(nextComparer);
            return comparer.Normalize();
        }

        public CompoundComparer()
        {
            comparers = new List<IComparer<T>>();
        }

        public void AppendComparison(IComparer<T> comparer)
        {
            if (comparer is NullComparer<T>)
            {
                return;
            }
            if (comparer is CompoundComparer<T> other)
            {
                comparers.AddRange(other.comparers);
                return;
            }
            comparers.Add(comparer);
        }

        public override int Compare(T x, T y)
        {
            foreach (var comparer in comparers)
            {
                var result = comparer.Compare(x, y);
                if (result != 0)
                {
                    return result;
                }
            }
            return 0;
        }

        public IComparer<T> Normalize()
        {
            if (comparers.Count == 0)
            {
                return NullComparer<T>.Default;
            }
            if (comparers.Count == 1)
            {
                return comparers[0];
            }
            return this;
        }
    }
}