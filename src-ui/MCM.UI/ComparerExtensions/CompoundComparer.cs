using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ComparerExtensions
{
    internal sealed class CompoundComparer<T> : Comparer<T>
    {
        private readonly List<IComparer<T>> _comparers;

        public static IComparer<T> GetComparer(IComparer<T> baseComparer, IComparer<T> nextComparer)
        {
            var comparer = new CompoundComparer<T>();
            comparer.AppendComparison(baseComparer);
            comparer.AppendComparison(nextComparer);
            return comparer.Normalize();
        }

        public CompoundComparer()
        {
            _comparers = [];
        }

        public void AppendComparison(IComparer<T> comparer)
        {
            if (comparer is NullComparer<T>)
            {
                return;
            }
            if (comparer is CompoundComparer<T> other)
            {
                _comparers.AddRange(other._comparers);
                return;
            }
            _comparers.Add(comparer);
        }

        public override int Compare(T x, T y)
        {
            foreach (var comparer in _comparers)
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
            if (_comparers.Count == 0)
            {
                return NullComparer<T>.Default;
            }
            if (_comparers.Count == 1)
            {
                return _comparers[0];
            }
            return this;
        }
    }
}