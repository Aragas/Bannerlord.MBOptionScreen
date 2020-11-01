using System;
using System.Collections.Generic;

namespace ComparerExtensions
{
    /// <summary>
    /// Provides methods for composing comparisons of a given type.
    /// </summary>
    internal static class ComparerExtensions
    {
        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the comparer.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="comparer">The comparer to use if two items compare as equal using the base comparer.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The comparer is null.</exception>
        public static IComparer<T> ThenBy<T>(this IComparer<T> baseComparer, IComparer<T> comparer)
        {
            if (baseComparer is null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the comparison.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="comparison">The comparison to use if two items compare as equal using the base comparer.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The comparison delegate is null.</exception>
        public static IComparer<T> ThenBy<T>(this IComparer<T> baseComparer, Func<T, T, int> comparison)
        {
            if (baseComparer is null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            if (comparison is null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }
            var wrapper = ComparisonWrapper<T>.GetComparer(comparison);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, wrapper);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        public static IComparer<T> ThenBy<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector)
        {
            if (baseComparer is null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderBy(keySelector);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <param name="keyComparer">The comparer to use to compare the keys.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static IComparer<T> ThenBy<T, TKey>(
            this IComparer<T> baseComparer,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer)
        {
            if (baseComparer is null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderBy(keySelector, keyComparer);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <param name="keyComparison">The comparison delegate to use to compare the keys.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static IComparer<T> ThenBy<T, TKey>(
            this IComparer<T> baseComparer,
            Func<T, TKey> keySelector,
            Func<TKey, TKey, int> keyComparison)
        {
            if (baseComparer is null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderBy(keySelector, keyComparison);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison, in descending order.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        public static IComparer<T> ThenByDescending<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector)
        {
            if (baseComparer is null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderByDescending(keySelector);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison, in descending order.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <param name="keyComparer">The comparer to use to compare the keys.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static IComparer<T> ThenByDescending<T, TKey>(
            this IComparer<T> baseComparer,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer)
        {
            if (baseComparer is null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderByDescending(keySelector, keyComparer);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison, in descending order.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <param name="keyComparison">The comparison delegate to use to compare the keys.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static IComparer<T> ThenByDescending<T, TKey>(
            this IComparer<T> baseComparer,
            Func<T, TKey> keySelector,
            Func<TKey, TKey, int> keyComparison)
        {
            if (baseComparer is null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderByDescending(keySelector, keyComparison);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Converts the given comparison function to a comparer object.
        /// </summary>
        /// <typeparam name="T">The type of the items that are compared.</typeparam>
        /// <param name="comparison">The comparison function to convert.</param>
        /// <returns>A new comparer that wraps the comparison function.</returns>
        /// <exception cref="System.ArgumentNullException">The comparison function is null.</exception>
        public static IComparer<T> ToComparer<T>(this Func<T, T, int> comparison)
        {
            if (comparison is null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }
            return ComparisonWrapper<T>.GetComparer(comparison);
        }
    }
}