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

        /// <summary>Gets the reflection information for a method by searching the type and all its super types</summary>
        /// <param name="type">The class/type where the method is declared</param>
        /// <param name="name">The name of the method (case sensitive)</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="generics">Optional list of types that define the generic version of the method</param>
        /// <returns>A method or null when type/name is null or when the method cannot be found</returns>
        ///
        public static MethodInfo? Method(Type type, string name, Type[]? parameters = null, Type[]? generics = null)
        {
            if (type == null)
            {
                //if (Harmony.DEBUG)
                //    FileLog.Log("AccessTools.Method: type is null");
                return null;
            }
            if (name == null)
            {
                //if (Harmony.DEBUG)
                //    FileLog.Log("AccessTools.Method: name is null");
                return null;
            }
            MethodInfo result;
            var modifiers = Array.Empty<ParameterModifier>();
            if (parameters == null)
            {
                try
                {
                    result = FindIncludingBaseTypes(type, t => t.GetMethod(name, All));
                }
                catch (AmbiguousMatchException ex)
                {
                    result = FindIncludingBaseTypes(type, t => t.GetMethod(name, All, null, Array.Empty<Type>(), modifiers));

                    if (result == null)
                    {
                        throw new AmbiguousMatchException($"Ambiguous match in Harmony patch for {type}:{name}." + ex);
                    }
                }
            }
            else
            {
                result = FindIncludingBaseTypes(type, t => t.GetMethod(name, All, null, parameters, modifiers));
            }

            if (result == null)
            {
                //if (Harmony.DEBUG)
                //    FileLog.Log($"AccessTools.Method: Could not find method for type {type} and name {name} and parameters {parameters?.Description()}");
                return null;
            }

            if (generics != null) result = result.MakeGenericMethod(generics);
            return result;
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

        /// <summary>Gets the reflection information for a field by searching the type and all its super types</summary>
        /// <param name="type">The class/type where the field is defined</param>
        /// <param name="name">The name of the field (case sensitive)</param>
        /// <returns>A field or null when type/name is null or when the field cannot be found</returns>
        ///
        public static FieldInfo? Field(Type type, string name)
        {
            if (type == null)
            {
                //if (Harmony.DEBUG)
                //    FileLog.Log("AccessTools.Field: type is null");
                return null;
            }
            if (name == null)
            {
                //if (Harmony.DEBUG)
                //    FileLog.Log("AccessTools.Field: name is null");
                return null;
            }
            var field = FindIncludingBaseTypes(type, t => t.GetField(name, All));
            //if (field == null && Harmony.DEBUG)
            //    FileLog.Log($"AccessTools.Field: Could not find field for type {type} and name {name}");
            return field;
        }
    }
}