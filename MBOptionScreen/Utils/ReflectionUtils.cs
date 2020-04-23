using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;

namespace MBOptionScreen.Utils
{
    internal static class ReflectionUtils
    {
        /// <summary>
        /// Return implementations that are directly castable to <TBase> or wraps them into a wrapper
        /// </summary>
        public static IEnumerable<TBase> GetImplementations<TBase, TWrapper>(ApplicationVersion version, params object[] args)
            where TWrapper : TBase
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => ImplementsOrImplementsEquivalent(t, typeof(TBase)));
            foreach (var match in AttributeHelper.GetMultiple(version, types))
            {
                var obj = Activator.CreateInstance(match.Key, args);
                if (typeof(TBase).IsAssignableFrom(match.Key))
                    yield return (TBase) obj;
                else
                    yield return (TBase) Activator.CreateInstance(typeof(TWrapper), new object[] { obj });
            }
        }


        /// <summary>
        /// Return implementations from any assembly.
        /// Do not attept to cast to this assembly base type without providing a wrapper.
        /// </summary>
        public static object GetImplementation(ApplicationVersion version, string typeName, params object[] args)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => ImplementsOrImplementsEquivalent(t, typeName));
            var tuple = AttributeHelper.Get(version, types);
            return Activator.CreateInstance(tuple.Type, args);
        }
        /// <summary>
        /// Return implementations that are directly castable to <TBase>, so withing this assembly.
        /// </summary>
        public static TBase GetImplementation<TBase>(ApplicationVersion version, params object[] args)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => Implements(t, typeof(TBase)));
            var tuple = AttributeHelper.Get(version, types);
            return (TBase)Activator.CreateInstance(tuple.Type, args);
        }
        /// <summary>
        /// Return implementations that are directly castable to <TBase> or wraps them into a wrapper
        /// </summary>
        public static TBase GetImplementation<TBase, TWrapper>(ApplicationVersion version, params object[] args)
            where TWrapper : TBase
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => ImplementsOrImplementsEquivalent(t, typeof(TBase)));
            var tuple = AttributeHelper.Get(version, types);

            var obj = Activator.CreateInstance(tuple.Type, args);
            if (typeof(TBase).IsAssignableFrom(tuple.Type))
                return (TBase)obj;
            else
                return (TBase)Activator.CreateInstance(typeof(TWrapper), new object[] { obj });
        }

        public static bool ImplementsOrImplementsEquivalent(Type type, Type baseType) =>
            ImplementsOrImplementsEquivalent(type, baseType.FullName);
        public static bool ImplementsOrImplementsEquivalent(Type type, string fullBaseTypeName)
        {
            var typeToCheck = type.BaseType;
            while (typeToCheck != null)
            {
                if (typeToCheck.FullName == fullBaseTypeName)
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            return type.GetInterfaces().Any(i => i.FullName == fullBaseTypeName);
        }

        public static bool Implements(Type type, Type baseType) => baseType.IsAssignableFrom(type);
    }
}