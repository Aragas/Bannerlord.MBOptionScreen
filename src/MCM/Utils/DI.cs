using MCM.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Utils
{
    public static class DynamicDI
    {

    }

    public static class StaticDI
    {
        public static void Update() { }
    }

    public static class DI
    {
        public static IEnumerable<Assembly> Filter(this IEnumerable<Assembly> enumerable) => enumerable
            .Where(a => !a.FullName.StartsWith("mscorlib"))
            .Where(a => !a.FullName.StartsWith("System"));
        public static IEnumerable<Assembly> FilterLegacy(this IEnumerable<Assembly> enumerable) => enumerable
            .Where(a => !a.FullName.StartsWith("MBOptionScreen"));
        public static IEnumerable<T> Parallel<T>(this IEnumerable<T> enumerable) => enumerable
            .AsParallel().AsOrdered().WithExecutionMode(ParallelExecutionMode.ForceParallelism);
        public static IEnumerable<Type> GetAllTypes() => AppDomain.CurrentDomain.GetAssemblies()
            .Filter()
            .FilterLegacy()
            .Where(a => !a.IsDynamic)
            .SelectMany(a => a.GetTypes());


        public static IEnumerable<TBase> GetBaseImplementations<TBase, TWrapper>(params object[] args)
            where TBase : class, IDependencyBase
            where TWrapper : TBase, IWrapper => Enumerable.Empty<TBase>();

        public static IEnumerable<TBase> GetBaseImplementations<TBase>(params object[] args)
            where TBase : class, IDependencyBase => Enumerable.Empty<TBase>();

        public static TBase? GetImplementation<TBase>(params object[] args)
            where TBase : class, IDependency => null;
        public static object? GetImplementation(Type baseType, params object[] args) => null;

        public static TBase? GetImplementation<TBase, TWrapper>(params object[] args)
            where TBase : class, IDependency
            where TWrapper : TBase, IWrapper => null;
        public static object? GetImplementation(Type baseType, Type wrapperType, params object[] args) => null;
    }
}