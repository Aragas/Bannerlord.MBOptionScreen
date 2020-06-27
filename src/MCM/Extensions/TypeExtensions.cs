using System;

namespace MCM.Extensions
{
    internal static class TypeExtensions
    {
        public static string FullName(this Type type) => $"{type.Namespace}.{type.Name}";
    }
}