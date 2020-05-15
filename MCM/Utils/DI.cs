using MCM.Abstractions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace MCM.Utils
{
    /// <summary>
    /// Yea, another custom Dependency Injection implementation
    /// I don't want to bring any external dependencies for now, so this is a somewhat
    /// not that bad measure
    /// </summary>
    public static class DI
    {
        private static IEnumerable<Assembly> FilterLegacy(this IEnumerable<Assembly> enumerable) => enumerable
            .Where(a => !a.IsDynamic)
            .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"));


        public static IEnumerable<TBase> GetBaseInterfaceImplementations<TBase, TWrapper>(ApplicationVersion? version = null, params object[] args)
            where TWrapper : TBase
        {
            if (version == null)
                version = ApplicationVersionUtils.GameVersion();

            var implementations = AppDomain.CurrentDomain.GetAssemblies()
                .FilterLegacy()
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TBase), false))
                .DistinctBy(t => t.FullName);

            foreach (var implementation in implementations)
            {
                if(implementation == typeof(TWrapper))
                    continue;

                var instance = GetImplementation(implementation, typeof(TWrapper), version.Value, args);
                if (instance is TBase @base)
                    yield return @base;
            }
        }

        /// <summary>
        /// Searches for interfaces that implements the <TBase> interface.
        /// Basically, the <TBase> interface is only a marker.
        /// This way if I declare IModLibSettingsContainer it will be ensured that only one implementation
        /// of the interface will be returned
        /// </summary>
        public static IEnumerable<TBase> GetBaseInterfaceImplementations<TBase>(ApplicationVersion? version = null, params object[] args)
        {
            if (version == null)
                version = ApplicationVersionUtils.GameVersion();

            var interfaces = AppDomain.CurrentDomain.GetAssemblies()
                .FilterLegacy()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsInterface && ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TBase), false))
                .DistinctBy(t => t.FullName);

            foreach (var @interface in interfaces)
            {
                var wrapper = @interface.Assembly.GetTypes()
                    .FirstOrDefault(t => typeof(IWrapper).IsAssignableFrom(t) && @interface.IsAssignableFrom(t));
                if (wrapper == null)
                    continue;

                var implementation = GetImplementation(@interface, wrapper, version.Value, args);
                if (implementation is TBase @base)
                    yield return @base;
            }
        }

        /// <summary>
        /// Return implementations that are directly castable to <TBase>, so withing this assembly.
        /// </summary>
        public static TBase? GetImplementation<TBase>(ApplicationVersion? version = null, params object[] args)
            where TBase : class
            => GetImplementation(typeof(TBase), version, args) as TBase;
        public static object? GetImplementation(Type type, ApplicationVersion? version = null, params object[] args)
        {
            if (version == null)
                version = ApplicationVersionUtils.GameVersion();

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .FilterLegacy()
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, type));
            var tuple = AttributeUtils.Get(version.Value, types);
            return tuple != null ? Activator.CreateInstance(tuple?.Type, args) : null;
        }

        /// <summary>
        /// Return implementations that are directly castable to <TBase> or wraps them into a wrapper
        /// </summary>
        public static TBase? GetImplementation<TBase, TWrapper>(ApplicationVersion? version = null, params object[] args)
            where TBase : class
            where TWrapper : TBase, IWrapper
            => GetImplementation(typeof(TBase), typeof(TWrapper), version, args) as TBase;
        public static object? GetImplementation(Type baseType, Type wrapperType, ApplicationVersion? version = null, params object[] args)
        {
            if (version == null)
                version = ApplicationVersionUtils.GameVersion();

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .FilterLegacy()
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, baseType))
                .ToList();
            return GetImplementationRecursive(baseType, wrapperType, version.Value, args, types);
        }

        /// <summary>
        /// Foolproofing. Custom implementations can be wrapped, but we can't be sure if they are correct.
        /// Do checking until we find the first correct one
        /// </summary>
        private static object? GetImplementationRecursive(Type baseType, Type wrapperType, ApplicationVersion version, object[] args, IEnumerable<Type> types)
        {
            var list = types.ToList();
            var tuple = AttributeUtils.Get(version, list);
            if (tuple == null)
                return null;

            var obj = Activator.CreateInstance(tuple?.Type, args);
            if (baseType.IsAssignableFrom(tuple?.Type))
                return obj;
            else
            {
                var wrapperObject = Activator.CreateInstance(wrapperType, obj);
                return wrapperObject is IWrapper wrapper && wrapper.IsCorrect
                    ? wrapperObject
                    : GetImplementationRecursive(baseType, wrapperType, version, args, list.Where(t => t != tuple?.Type));
            }
        }
    }
}