using System;

namespace MCM.Adapter.MCMv3.Extensions
{
    internal static class TypeExtensions
    {
        public static string FullName(this Type type) => $"{type.Namespace}.{type.Name}";
    }
}