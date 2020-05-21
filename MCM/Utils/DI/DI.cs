using MCM.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Utils
{
    /// <summary>
    /// Yea, another custom Dependency Injection implementation
    /// I don't want to bring any external dependencies for now, so this is a somewhat
    /// not that bad measure
    /// </summary>
    public static class DI
    {
        public static IEnumerable<Assembly> Filter(this IEnumerable<Assembly> enumerable) => enumerable
            .Where(a => !a.FullName.StartsWith("mscorlib"))
            .Where(a => !a.FullName.StartsWith("System"));
        public static IEnumerable<Assembly> FilterLegacy(this IEnumerable<Assembly> enumerable) => enumerable
            .Where(a => !a.FullName.StartsWith("MBOptionScreen"));
        public static IEnumerable<T> Parallel<T>(this IEnumerable<T> enumerable) => enumerable
            .AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism);
        public static IEnumerable<Type> GetAllTypes() => AppDomain.CurrentDomain.GetAssemblies()
            .Filter()
            .FilterLegacy()
            .SelectMany(a => a.GetTypes());


        public static IEnumerable<TBase> GetBaseImplementations<TBase, TWrapper>(params object[] args)
            where TBase : class, IDependencyBase
            where TWrapper : TBase, IWrapper
        {
            return StaticDI.GetBaseImplementations<TBase, TWrapper>(args);
        }

        /// <summary>
        /// Searches for interfaces that implements the <TBase> interface.
        /// Basically, the <TBase> interface is only a marker.
        /// This way if I declare IModLibSettingsContainer it will be ensured that only one implementation
        /// of the interface will be returned
        /// </summary>
        public static IEnumerable<TBase> GetBaseImplementations<TBase>(params object[] args)
            where TBase : class, IDependencyBase
        {
            return StaticDI.GetBaseImplementations<TBase>(args);
        }

        /// <summary>
        /// Return implementations that are directly castable to <TBase>, so withing this assembly.
        /// </summary>
        public static TBase? GetImplementation<TBase>(params object[] args)
            where TBase : class, IDependency
            => GetImplementation(typeof(TBase), args) as TBase;
        public static object? GetImplementation(Type baseType, params object[] args)
        {
            return StaticDI.GetImplementation(baseType, args);
        }

        /// <summary>
        /// Return implementations that are directly castable to <TBase> or wraps them into a wrapper
        /// </summary>
        public static TBase? GetImplementation<TBase, TWrapper>(params object[] args)
            where TBase : class, IDependency
            where TWrapper : TBase, IWrapper
            => GetImplementation(typeof(TBase), typeof(TWrapper), args) as TBase;
        public static object? GetImplementation(Type baseType, Type wrapperType, params object[] args)
        {
            return StaticDI.GetImplementation(baseType, wrapperType, args);
        }
    }
}