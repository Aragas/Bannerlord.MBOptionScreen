using System;
using System.Collections;
using System.Collections.Generic;

namespace ComparerExtensions
{
    /// <summary>
    /// Compares values by comparing their extracted key values.
    /// </summary>
    /// <typeparam name="T">The type being compared.</typeparam>
    internal abstract class KeyComparer<T> : IComparer<T>, IComparer
    {
        /// <summary>
        /// Initializes a new instance of a KeyComparer.
        /// </summary>
        protected KeyComparer()
        {
        }

        /// <summary>
        /// Compares the given values by key.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// An integer representing the relationship between the two values.
        /// Negative values indicate that the first value was less than the second.
        /// Positive values indicate that the first value was greater than the second.
        /// Zero indicates that the first and second values were equal.
        /// </returns>
        public abstract int Compare(T x, T y);

        int IComparer.Compare(object x, object y) => Compare((T) x, (T) y);

        /// <summary>
        /// Creates a new KeyComparer that sorts using the results of the key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function that selects a key.</param>
        /// <returns>A KeyComparer using the key selector.</returns>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        public static KeyComparer<T> OrderBy<TKey>(Func<T, TKey> keySelector)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return new TypedKeyComparer<T, TKey>(keySelector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Creates a new KeyComparer that sorts using the results of the key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function that selects a key.</param>
        /// <param name="keyComparer">The comparer to use to compare keys.</param>
        /// <returns>A KeyComparer using the key selector.</returns>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static KeyComparer<T> OrderBy<TKey>(Func<T, TKey> keySelector, IComparer<TKey> keyComparer)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (keyComparer == null)
            {
                throw new ArgumentNullException(nameof(keyComparer));
            }

            return new TypedKeyComparer<T, TKey>(keySelector, keyComparer);
        }

        /// <summary>
        /// Creates a new KeyComparer that sorts using the results of the key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function that selects a key.</param>
        /// <param name="keyComparison">The comparison delegate to use to compare keys.</param>
        /// <returns>A KeyComparer using the key selector.</returns>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static KeyComparer<T> OrderBy<TKey>(Func<T, TKey> keySelector, Func<TKey, TKey, int> keyComparison)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (keyComparison == null)
            {
                throw new ArgumentNullException(nameof(keyComparison));
            }

            var keyComparer = ComparisonWrapper<TKey>.GetComparer(keyComparison);
            return new TypedKeyComparer<T, TKey>(keySelector, keyComparer);
        }

        /// <summary>
        /// Creates a new KeyComparer that sorts using the results of the key selector, in descending order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function that selects a key.</param>
        /// <returns>A KeyComparer using the key selector.</returns>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        public static KeyComparer<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return new TypedKeyComparer<T, TKey>(keySelector, Comparer<TKey>.Default) { Descending = true };
        }

        /// <summary>
        /// Creates a new KeyComparer that sorts using the results of the key selector, in descending order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function that selects a key.</param>
        /// <param name="keyComparer">The comparer to use to compare keys.</param>
        /// <returns>A KeyComparer using the key selector.</returns>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static KeyComparer<T> OrderByDescending<TKey>(Func<T, TKey> keySelector, IComparer<TKey> keyComparer)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (keyComparer == null)
            {
                throw new ArgumentNullException(nameof(keyComparer));
            }

            return new TypedKeyComparer<T, TKey>(keySelector, keyComparer) { Descending = true };
        }

        /// <summary>
        /// Creates a new KeyComparer that sorts using the results of the key selector, in descending order.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function that selects a key.</param>
        /// <param name="keyComparison">The comparison delegate to use to compare keys.</param>
        /// <returns>A KeyComparer using the key selector.</returns>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static KeyComparer<T> OrderByDescending<TKey>(Func<T, TKey> keySelector,
            Func<TKey, TKey, int> keyComparison)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (keyComparison == null)
            {
                throw new ArgumentNullException(nameof(keyComparison));
            }

            var keyComparer = ComparisonWrapper<TKey>.GetComparer(keyComparison);
            return new TypedKeyComparer<T, TKey>(keySelector, keyComparer) { Descending = true };
        }
    }
}