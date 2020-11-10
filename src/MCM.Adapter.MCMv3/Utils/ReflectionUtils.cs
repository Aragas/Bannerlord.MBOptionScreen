using MCM.Adapter.MCMv3.Extensions;

using System;
using System.Linq;

namespace MCM.Adapter.MCMv3.Utils
{
    internal static class ReflectionUtils
    {
        public static bool ImplementsOrImplementsEquivalent(Type type, Type baseType, bool includeBase = true)
        {
            if (baseType.IsAssignableFrom(includeBase ? type : type.BaseType))
                return true;
            return ImplementsOrImplementsEquivalent(type, baseType.FullName(), includeBase);
        }

        public static bool ImplementsOrImplementsEquivalent(Type type, string fullBaseTypeName, bool includeBase = true)
        {
            var typeToCheck = includeBase ? type : type.BaseType;

            while (typeToCheck is { })
            {
                if (string.Equals(typeToCheck.FullName(), fullBaseTypeName, StringComparison.Ordinal))
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            return type.GetInterfaces().Any(t => (includeBase || type != t) && string.Equals(t.FullName(), fullBaseTypeName, StringComparison.Ordinal));
        }
    }
}