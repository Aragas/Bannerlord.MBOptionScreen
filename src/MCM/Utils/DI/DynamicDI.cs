using Bannerlord.ButterLib.Common.Helpers;

using MCM.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace MCM.Utils
{
    public static class DynamicDI
    {
        private static ApplicationVersion Version { get; } = ApplicationVersionUtils.GameVersion() ?? default; // TODO

        internal static IEnumerable<TBase> GetBaseImplementations<TBase, TWrapper>(params object[] args)
            where TBase : class, IDependencyBase
            where TWrapper : TBase, IWrapper
        {
            var baseTypes = TaleWorlds.Core.Extensions.DistinctBy(DI.GetAllTypes()
                .Where(t => t.IsAbstract || t.IsInterface)
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TBase), false)), t => t.FullName);

            foreach (var baseType in baseTypes)
            {
                if (baseType == typeof(TWrapper))
                    continue;

                var implementation = GetImplementation(baseType, typeof(TWrapper), args);
                if (implementation is TBase @base)
                    yield return @base;
            }
        }

        internal static IEnumerable<TBase> GetBaseImplementations<TBase>(params object[] args)
            where TBase : class, IDependencyBase
        {
            var baseTypes = TaleWorlds.Core.Extensions.DistinctBy(DI.GetAllTypes()
                .Where(t => t.IsAbstract || t.IsInterface)
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TBase), false)), t => t.FullName);

            foreach (var baseType in baseTypes)
            {
                var wrapper = baseType.Assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .FirstOrDefault(t => typeof(IWrapper).IsAssignableFrom(t) && baseType.IsAssignableFrom(t));
                if (wrapper == null)
                    continue;

                var implementation = GetImplementation(baseType, wrapper, Version, args);
                if (implementation is TBase @base)
                    yield return @base;
            }
        }

        internal static TBase? GetImplementation<TBase>(params object[] args)
            where TBase : class, IDependency
            => GetImplementation(typeof(TBase), args) as TBase;
        internal static object? GetImplementation(Type baseType, params object[] args)
        {
            var types = DI.GetAllTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, baseType));
            var tuple = VersionUtils.GetLastImplementation(Version, types);
            return tuple != null ? Activator.CreateInstance(tuple?.Type, args) : null;
        }

        internal static TBase? GetImplementation<TBase, TWrapper>(params object[] args)
            where TBase : class, IDependency
            where TWrapper : TBase, IWrapper
            => GetImplementation(typeof(TBase), typeof(TWrapper), args) as TBase;
        internal static object? GetImplementation(Type baseType, Type wrapperType, params object[] args)
        {
            var types = DI.GetAllTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, baseType));
            return GetImplementationRecursive(baseType, wrapperType, args, types);
        }

        /// <summary>
        /// Foolproofing. Custom implementations can be wrapped, but we can't be sure if they are correct.
        /// Do checking until we find the first correct one
        /// </summary>
        private static object? GetImplementationRecursive(Type baseType, Type wrapperType, object[] args, IEnumerable<Type> types)
        {
            var tuple = VersionUtils.GetLastImplementation(Version, types);
            if (tuple == null || tuple?.Type == null)
                return null;

            var obj = Activator.CreateInstance(tuple?.Type, args);
            if (baseType.IsAssignableFrom(tuple?.Type))
                return obj;
            else
            {
                var wrapperObject = Activator.CreateInstance(wrapperType, obj);
                return wrapperObject is IWrapper wrapper && wrapper.IsCorrect
                    ? wrapperObject
                    : GetImplementationRecursive(baseType, wrapperType, args, types.Where(t => t != tuple?.Type));
            }
        }
    }
}