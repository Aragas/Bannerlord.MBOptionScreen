using MCM.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;

namespace MCM.Utils
{
    public static class StaticDI
    {
        static StaticDI()
        {
            Update();
        }

        private static IEnumerable<Type> GetInterfaces(this Type type, bool includeInherited) => includeInherited || type.BaseType == null
            ? type.GetInterfaces()
            : type.GetInterfaces().Except(type.BaseType.GetInterfaces());

        private static ApplicationVersion Version { get; } = ApplicationVersionUtils.GameVersion();
        private static Dictionary<string, Type> LatestImplementations { get; } = new Dictionary<string, Type>();
        private static Dictionary<string, List<string>> DependencyBases { get; } = new Dictionary<string, List<string>>();
        private static Dictionary<string, Type> DependencyBaseLatestImplementations { get; } = new Dictionary<string, Type>();

        public static void Update()
        {
            UpdateDependencies();
            UpdateDependencyBases();
        }
        private static void UpdateDependencies()
        {
            var interfaces = DI.GetAllTypes()
                .Where(t => t.IsAbstract || t.IsInterface)
                .Where(t => t.GetInterfaces(false).Any(i => i.FullName == typeof(IDependency).FullName));

            foreach (var @interface in interfaces)
            {
                var types = DI.GetAllTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, @interface));
                var tuple = VersionUtils.GetLastImplementation(Version, types);
                if (tuple != null && tuple.Value.Type != null)
                    LatestImplementations.Add(@interface.FullName, tuple.Value.Type);
            }
        }
        private static void UpdateDependencyBases()
        {
            foreach (var @base in DI.GetAllTypes().Where(t => t.IsAbstract || t.IsInterface))
            {
                if (@base.IsInterface || @base.BaseType == typeof(object))
                {
                    // Interface implements an interface that implements IDependencyBase
                    foreach (var type1 in @base.GetInterfaces(false).Where(i => i.FullName != typeof(IDependencyBase).FullName))
                    {
                        if (type1.GetInterfaces().Any(i => i.FullName == typeof(IDependencyBase).FullName))
                        {
                            if (!DependencyBases.ContainsKey(type1.FullName))
                                DependencyBases.Add(type1.FullName, new List<string>());

                            DependencyBases[type1.FullName].Add(@base.FullName);
                            break;
                        }
                    }
                }
                else
                {
                    if (@base.BaseType.GetInterfaces(false).Any(i => i.FullName == typeof(IDependencyBase).FullName))
                    {
                        if (!DependencyBases.ContainsKey(@base.BaseType.FullName))
                            DependencyBases.Add(@base.BaseType.FullName, new List<string>());

                        DependencyBases[@base.BaseType.FullName].Add(@base.FullName);
                    }
                }
            }

            var types = DI.GetAllTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
            foreach (var pair in DependencyBases)
            {
                foreach (var type in pair.Value)
                {
                    var tuple = VersionUtils.GetLastImplementation(Version, types.Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, type, false)));
                    if (tuple != null && tuple.Value.Type != null)
                    {
                        if (!DependencyBaseLatestImplementations.ContainsKey(type))
                            DependencyBaseLatestImplementations.Add(type, tuple.Value.Type);
                        else
                            DependencyBaseLatestImplementations[type] = tuple.Value.Type;
                    }
                }
            }
        }

        public static IEnumerable<TBase> GetBaseImplementations<TBase, TWrapper>(params object[] args) 
            where TBase : class, IDependencyBase
            where TWrapper : TBase, IWrapper
        {
            if (!DependencyBases.ContainsKey(typeof(TBase).FullName)) yield break;

            foreach (var type in DependencyBases[typeof(TBase).FullName].Where(t => DependencyBaseLatestImplementations.ContainsKey(t)))
            {
                var implementationType = DependencyBaseLatestImplementations[type];
                if (typeof(TBase).IsAssignableFrom(implementationType))
                    yield return (TBase) Activator.CreateInstance(implementationType, args);
                else
                {
                    var implementation = GetImplementation(implementationType, typeof(TWrapper), Version, args);
                    if (implementation is TBase @base)
                        yield return @base;
                }
            }
        }

        public static IEnumerable<TBase> GetBaseImplementations<TBase>(params object[] args)
            where TBase : class, IDependencyBase
        {
            if (!DependencyBases.ContainsKey(typeof(TBase).FullName)) yield break;

            foreach (var type in DependencyBases[typeof(TBase).FullName].Where(t => DependencyBaseLatestImplementations.ContainsKey(t)))
            {
                var latestImplementationType = DependencyBaseLatestImplementations[type];
                if (typeof(TBase).IsAssignableFrom(latestImplementationType))
                    yield return (TBase) Activator.CreateInstance(latestImplementationType, args);
                else
                {
                    var wrapper = latestImplementationType.Assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract)
                        .FirstOrDefault(t => typeof(IWrapper).IsAssignableFrom(t) && latestImplementationType.IsAssignableFrom(t));
                    if (wrapper == null) continue;

                    var implementation = GetImplementation(latestImplementationType, wrapper, Version, args);
                    if (implementation is TBase @base)
                        yield return @base;
                }
            }
        }

        public static TBase? GetImplementation<TBase>(params object[] args)
            where TBase : class, IDependency =>
            GetImplementation(typeof(TBase), args) as TBase;
        public static object? GetImplementation(Type baseType, params object[] args)
        {
            if (!LatestImplementations.ContainsKey(baseType.FullName)) return null;

            var latestImplementationType = LatestImplementations[baseType.FullName];
            return Activator.CreateInstance(latestImplementationType, args);
        }

        public static TBase? GetImplementation<TBase, TWrapper>(params object[] args)
            where TBase : class, IDependency
            where TWrapper : TBase, IWrapper
            => GetImplementation(typeof(TBase), typeof(TWrapper), args) as TBase;
        public static object?  GetImplementation(Type baseType, Type wrapperType, params object[] args)
        {
            if (!LatestImplementations.ContainsKey(baseType.FullName)) return null;
            
            var latestImplementationType = LatestImplementations[baseType.FullName];
            var obj = Activator.CreateInstance(latestImplementationType, args);
            return baseType.IsAssignableFrom(latestImplementationType)
                ? obj
                : Activator.CreateInstance(wrapperType, obj) is IWrapper wrapperObject && wrapperObject.IsCorrect
                    ? wrapperObject
                    : null;
        }
    }
}