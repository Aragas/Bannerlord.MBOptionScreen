using System;

namespace MCM.Implementation.MCMv3.Extensions
{
    internal static class TypeExtensions
    {
        public static string FullName(this Type type) => $"{type.Namespace}.{type.Name}";
    }
}