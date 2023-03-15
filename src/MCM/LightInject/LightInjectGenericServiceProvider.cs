using BUTR.DependencyInjection;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MCM.LightInject
{
    internal class LightInjectGenericServiceProvider : IGenericServiceProvider
    {
        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object x, object y) => ReferenceEquals(x, y);
            int IEqualityComparer<object>.GetHashCode(object obj) => obj.GetHashCode();
        }
        private static readonly ReferenceEqualityComparer Comparer = new();

        private delegate TService OfTypeDelegate<TService>(IEnumerable<object> enumerable);
        private static readonly ConcurrentDictionary<Type, object> OfTypeCache = new();

        private readonly IServiceContainer _serviceContainer;

        public LightInjectGenericServiceProvider(IServiceContainer serviceContainer) => _serviceContainer = serviceContainer;

        /// <inheritdoc />
        public IGenericServiceProviderScope CreateScope() => new LightInjectGenericServiceProviderScope(_serviceContainer.BeginScope());

        public TService? GetService<TService>() where TService : class
        {
            var value = _serviceContainer.GetInstance<TService>();
            // A nasty behaviour quirk where LightInject will return the same instance several times
            var type = typeof(TService);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) && value is IEnumerable enumerable)
            {
                var ofType = (OfTypeDelegate<TService>) OfTypeCache.GetOrAdd(typeof(TService), x =>
                    typeof(Enumerable).GetMethod("OfType").MakeGenericMethod(x.GenericTypeArguments[0]).CreateDelegate(typeof(OfTypeDelegate<TService>)));
                return ofType(enumerable.Cast<object>().Distinct(Comparer));
            }
            return value;
        }

        public void Dispose() => _serviceContainer.Dispose();
    }
}