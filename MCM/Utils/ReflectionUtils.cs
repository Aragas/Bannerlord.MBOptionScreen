using System;
using System.Linq;

namespace MCM.Utils
{
    public static class ReflectionUtils
    {
        public static bool ImplementsOrImplementsEquivalent(Type type, Type baseType, bool includeBase = true)
        {
            if (baseType.IsAssignableFrom(includeBase ? type : type.BaseType))
                return true;
            return ImplementsOrImplementsEquivalent(type, $"{baseType.Namespace}.{baseType.Name}", includeBase);
            //return ImplementsOrImplementsEquivalent(type, (baseType.IsGenericType ? $"{baseType.Namespace}.{baseType.Name}" : baseType.FullName), includeBase);
        }

        public static bool ImplementsOrImplementsEquivalent(Type type, string fullBaseTypeName, bool includeBase = true)
        {
            var typeToCheck = includeBase ? type : type.BaseType;
            
            while (typeToCheck != null)
            {
                //if (string.Equals((typeToCheck.IsGenericType ? $"{typeToCheck.Namespace}.{typeToCheck.Name}" : typeToCheck.FullName), fullBaseTypeName, StringComparison.Ordinal)) 
                if (string.Equals($"{typeToCheck.Namespace}.{typeToCheck.Name}", fullBaseTypeName, StringComparison.Ordinal))
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            //return type.GetInterfaces().Any(t => (includeBase || type != t) && string.Equals((t.IsGenericType ? $"{t.Namespace}.{t.Name}" : t.FullName), fullBaseTypeName, StringComparison.Ordinal));
            return type.GetInterfaces().Any(t => (includeBase || type != t) && string.Equals($"{t.Namespace}.{t.Name}", fullBaseTypeName, StringComparison.Ordinal));
        }

        public static bool Implements(Type type, Type baseType) => baseType.IsAssignableFrom(type);
    }
}