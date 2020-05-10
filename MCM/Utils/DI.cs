﻿using MCM.Abstractions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        /// <summary>
        /// Return implementations that are directly castable to <TBase> or wraps them into a wrapper
        /// </summary>
        public static IEnumerable<TBase> GetImplementations<TBase, TWrapper>(ApplicationVersion version, params object[] args)
            where TBase : class
            where TWrapper : TBase, IWrapper
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TBase)));
            foreach (var match in AttributeUtils.GetMultiple(version, types))
            {
                var obj = Activator.CreateInstance(match.Key, args);
                if (typeof(TBase).IsAssignableFrom(match.Key))
                    yield return (TBase)obj;
                else
                {
                    var wrapper = (TWrapper) Activator.CreateInstance(typeof(TWrapper), obj);
                    if (wrapper.IsCorrect)
                        yield return wrapper;
                }
            }
        }


        /// <summary>
        /// Return implementations from any assembly.
        /// Do not attept to cast to this assembly base type without providing a wrapper.
        /// </summary>
        public static object? GetImplementation(ApplicationVersion version, string typeName, params object[] args)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeName));
            var tuple = AttributeUtils.Get(version, types);
            return tuple != null ? Activator.CreateInstance(tuple?.Type, args) : null;
        }

        /// <summary>
        /// Return implementations that are directly castable to <TBase>, so withing this assembly.
        /// </summary>
        public static TBase? GetImplementation<TBase>(ApplicationVersion version, params object[] args)
            where TBase : class
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.Implements(t, typeof(TBase)));
            var tuple = AttributeUtils.Get(version, types);
            return tuple != null ? Activator.CreateInstance(tuple?.Type, args) as TBase : null;
        }

        /// <summary>
        /// Return implementations that are directly castable to <TBase> or wraps them into a wrapper
        /// </summary>
        public static TBase? GetImplementation<TBase, TWrapper>(ApplicationVersion version, params object[] args)
            where TBase : class
            where TWrapper : TBase, IWrapper
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TBase)))
                .ToList();
            return GetImplementationRecursive<TBase, TWrapper>(version, args, types);
        }

        /// <summary>
        /// Foolproofing. Custom implementations can be wrapped, but we can't be sure if they are correct.
        /// Do checking until we find the first correct one
        /// </summary>
        private static TBase? GetImplementationRecursive<TBase, TWrapper>(ApplicationVersion version, object[] args, IEnumerable<Type> types)
            where TBase : class
            where TWrapper : TBase, IWrapper
        {
            var tuple = AttributeUtils.Get(version, types);
            if (tuple == null)
                return null;

            var obj = Activator.CreateInstance(tuple?.Type, args);
            if (typeof(TBase).IsAssignableFrom(tuple?.Type))
                return (TBase) obj;
            else
            {
                var wrapper = (TWrapper) Activator.CreateInstance(typeof(TWrapper), obj);
                return wrapper.IsCorrect
                    ? wrapper
                    : GetImplementationRecursive<TBase, TWrapper>(version, args, types.Where(t => t != tuple?.Type));
            }
        }
    }
}