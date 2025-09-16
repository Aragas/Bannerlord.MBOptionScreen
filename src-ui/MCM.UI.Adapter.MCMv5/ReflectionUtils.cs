using System;
using System.Linq;

namespace MCM.UI.Adapter.MCMv5;

internal static class ReflectionUtils
{
    public static bool ImplementsOrImplementsEquivalent(Type type, string? fullBaseTypeName, bool includeBase = true)
    {
        if (fullBaseTypeName is null)
            return false;

        var typeToCheck = includeBase ? type : type.BaseType;

        while (typeToCheck is { })
        {
            if (typeToCheck.FullName?.EndsWith(fullBaseTypeName, StringComparison.Ordinal) == true)
                return true;

            typeToCheck = typeToCheck.BaseType;
        }

        return type.GetInterfaces().Any(x => (includeBase || type != x) && string.Equals(x.FullName, fullBaseTypeName, StringComparison.Ordinal));
    }
    public static bool ImplementsEquivalentInterface(Type type, string? fullBaseTypeName)
    {
        if (fullBaseTypeName is null)
            return false;

        var typeToCheck = type;

        while (typeToCheck is { })
        {
            if (type.GetInterfaces().Any(x => x.FullName?.EndsWith(fullBaseTypeName, StringComparison.Ordinal) == true))
                return true;

            typeToCheck = typeToCheck.BaseType;
        }

        return false;
    }
}