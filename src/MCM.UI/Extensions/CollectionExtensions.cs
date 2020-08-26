using System.Collections.Generic;
using System.Linq;

namespace MCM.UI.Extensions
{
    internal static class CollectionExtensions
    {
        public static IEnumerable<T> Parallel<T>(this IEnumerable<T> enumerable) => enumerable
            .AsParallel().AsOrdered().WithExecutionMode(ParallelExecutionMode.ForceParallelism);
    }
}