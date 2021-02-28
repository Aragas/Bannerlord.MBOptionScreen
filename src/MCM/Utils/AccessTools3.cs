using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using static HarmonyLib.AccessTools;

namespace MCM.Utils
{
    public static class AccessTools3
    {
        public static FieldRef<TField>? StaticFieldRefAccess<TField>(FieldInfo? fieldInfo)
            => fieldInfo is null ? null : AccessTools.StaticFieldRefAccess<TField>(fieldInfo);

        public static FieldRef<object, TField>? FieldRefAccess<TField>(Type type, string fieldName)
        {
            var field = Field(type, fieldName);
            return field is null ? null : AccessTools.FieldRefAccess<object, TField>(field);
        }

        public static FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(string fieldName)
        {
            var field = typeof(TObject).GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
            return field is null ? null : AccessTools.FieldRefAccess<TObject, TField>(field);
        }

        public static TDelegate? GetDelegate<TDelegate>(ConstructorInfo? constructorInfo) where TDelegate : Delegate
            => ReflectionHelper.GetDelegate<TDelegate>(constructorInfo);

        public static TDelegate? GetConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(Constructor(type, parameters));

        public static TDelegate? GetDeclaredConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(DeclaredConstructor(type, parameters));

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type type, string method) where TDelegate : Delegate
            => GetDelegateObjectInstance<TDelegate>(Method(type, method));

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type type,
                                                                      string method,
                                                                      Type[]? parameters,
                                                                      Type[]? generics = null) where TDelegate : Delegate
            => GetDelegateObjectInstance<TDelegate>(Method(type, method, parameters, generics));

        internal static bool TryGetDelegateObjectInstance<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                     Type type,
                                                                     string method,
                                                                     Type[]? parameters = null,
                                                                     Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegateObjectInstance<TDelegate>(Method(type, method, parameters, generics))) is not null;

        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type type, string method) where TDelegate : Delegate
            => GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method));

        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type type,
                                                                              string method,
                                                                              Type[]? parameters,
                                                                              Type[]? generics = null) where TDelegate : Delegate
            => GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method, parameters, generics));

        internal static bool TryGetDeclaredDelegateObjectInstance<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                             Type type,
                                                                             string method,
                                                                             Type[]? parameters = null,
                                                                             Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method, parameters, generics))) is not null;

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
            => ReflectionHelper.GetDelegateObjectInstance<TDelegate>(methodInfo);

        public static TDelegate? GetDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
            => GetDelegate<TDelegate>(Method(type, method));

        public static TDelegate? GetDelegate<TDelegate>(Type type,
                                                        string method,
                                                        Type[]? parameters,
                                                        Type[]? generics = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(Method(type, method, parameters, generics));

        internal static bool TryGetDelegate<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                       Type type,
                                                       string method,
                                                       Type[]? parameters = null,
                                                       Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegate<TDelegate>(Method(type, method, parameters, generics))) is not null;

        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
            => GetDelegate<TDelegate>(DeclaredMethod(type, method));

        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type,
                                                                string method,
                                                                Type[]? parameters,
                                                                Type[]? generics = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(DeclaredMethod(type, method, parameters, generics));

        internal static bool TryGetDeclaredDelegate<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                               Type type,
                                                               string method,
                                                               Type[]? parameters = null,
                                                               Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegate<TDelegate>(DeclaredMethod(type, method, parameters, generics))) is not null;

        public static TDelegate? GetDelegate<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate
            => ReflectionHelper.GetDelegate<TDelegate>(methodInfo);

        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, string method) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method));

        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance,
                                                                   string method,
                                                                   Type[]? parameters,
                                                                   Type[]? generics = null) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method, parameters, generics));

        internal static bool TryGetDelegate<TDelegate, TInstance>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                  TInstance instance,
                                                                  string method,
                                                                  Type[]? parameters = null,
                                                                  Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = instance is null
                ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method, parameters, generics))) is not null;

        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance,
                                                                           string method) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method));

        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance,
                                                                           string method,
                                                                           Type[]? parameters,
                                                                           Type[]? generics = null) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method, parameters, generics));

        internal static bool TryGetDeclaredDelegate<TDelegate, TInstance>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                          TInstance instance,
                                                                          string method,
                                                                          Type[]? parameters = null,
                                                                          Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = instance is null
                ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method, parameters, generics))) is not null;

        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, MethodInfo? methodInfo) where TDelegate : Delegate
            => GetDelegate<TDelegate>(instance, methodInfo);

        public static TDelegate? GetDelegate<TDelegate>(object? instance, MethodInfo? methodInfo) where TDelegate : Delegate
            => instance is null || methodInfo is null ? null : Delegate.CreateDelegate(typeof(TDelegate), instance, methodInfo.Name) as TDelegate;
    }
}