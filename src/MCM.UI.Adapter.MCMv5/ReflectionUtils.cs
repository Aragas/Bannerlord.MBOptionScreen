using System;
using System.Linq;

namespace MCM.UI.Adapter.MCMv5
{
    internal static class ReflectionUtils
    {
        public static bool ImplementsOrImplementsEquivalent(Type type, string? fullBaseTypeName, bool includeBase = true)
        {
            var typeToCheck = includeBase ? type : type.BaseType;

            while (typeToCheck is { })
            {
                if (string.Equals(typeToCheck.FullName, fullBaseTypeName, StringComparison.Ordinal))
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            return type.GetInterfaces().Any(x => (includeBase || type != x) && string.Equals(x.FullName, fullBaseTypeName, StringComparison.Ordinal));
        }
        public static bool ImplementsEquivalentInterface(Type type, string? fullBaseTypeName)
        {
            var typeToCheck = type;

            while (typeToCheck is { })
            {
                if (type.GetInterfaces().Any(x => string.Equals(x.FullName, fullBaseTypeName, StringComparison.Ordinal)))
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            return false;
        }
    }
}