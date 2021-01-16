using System;
using System.Reflection;

namespace HarmonyLib
{
    internal static class AccessTools
    {
        public const BindingFlags All =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.GetField |
            BindingFlags.SetField |
            BindingFlags.GetProperty |
            BindingFlags.SetProperty;


        /// <summary>Applies a function going up the type hierarchy and stops at the first non null result</summary>
        /// <typeparam name="T">Result type of func()</typeparam>
        /// <param name="type">The class/type to start with</param>
        /// <param name="func">The evaluation function returning T</param>
        /// <returns>Returns the first non null result or <c>default(T)</c> when reaching the top level type object</returns>
        ///
        public static T FindIncludingBaseTypes<T>(Type type, Func<Type, T> func) where T : class
        {
            while (true)
            {
                var result = func(type);
#pragma warning disable RECS0017
                if (result != null) return result;
#pragma warning restore RECS0017
                if (type == typeof(object)) return default!;
                type = type!.BaseType;
            }
        }

        /// <summary>Gets the reflection information for a constructor by searching the type and all its super types</summary>
        /// <param name="type">The class/type where the constructor is declared</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="searchForStatic">Optional parameters to only consider static constructors</param>
        /// <returns>A constructor info or null when type is null or when the method cannot be found</returns>
        ///
        public static ConstructorInfo? Constructor(Type type, Type[]? parameters = null, bool searchForStatic = false)
        {
            if (type == null)
            {
                //if (Harmony.DEBUG)
                //    FileLog.Log("AccessTools.ConstructorInfo: type is null");
                return null;
            }
            if (parameters == null) parameters = Type.EmptyTypes;
            var flags = searchForStatic ? All & ~BindingFlags.Instance : All & ~BindingFlags.Static;
            return FindIncludingBaseTypes(type, t => t.GetConstructor(flags, null, parameters, Array.Empty<ParameterModifier>()));
        }

        /// <summary>Gets the reflection information for a property by searching the type and all its super types</summary>
        /// <param name="type">The class/type</param>
        /// <param name="name">The name</param>
        /// <returns>A property or null when type/name is null or when the property cannot be found</returns>
        ///
        public static PropertyInfo? Property(Type type, string name)
        {
            if (type == null)
            {
                //if (Harmony.DEBUG)
                //    FileLog.Log("AccessTools.Property: type is null");
                return null;
            }
            if (name == null)
            {
                //if (Harmony.DEBUG)
                //    FileLog.Log("AccessTools.Property: name is null");
                return null;
            }
            var property = FindIncludingBaseTypes(type, t => t.GetProperty(name, All));
            //if (property == null && Harmony.DEBUG)
            //    FileLog.Log($"AccessTools.Property: Could not find property for type {type} and name {name}");
            return property;
        }
    }
}