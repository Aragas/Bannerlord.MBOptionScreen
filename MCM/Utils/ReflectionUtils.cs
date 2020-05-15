using System;
using System.Linq;

namespace MCM.Utils
{
    public static class ReflectionUtils
    {
        public static bool ImplementsOrImplementsEquivalent(Type type, Type baseType, bool includeBase = true) =>
            ImplementsOrImplementsEquivalent(type, (baseType.IsGenericType ? $"{baseType.Namespace}.{baseType.Name}" : baseType.FullName), includeBase);
        public static bool ImplementsOrImplementsEquivalent(Type type, string fullBaseTypeName, bool includeBase = true)
        {
            var typeToCheck = includeBase ? type : type.BaseType;
            
            while (typeToCheck != null)
            {
                if ((typeToCheck.IsGenericType ? $"{typeToCheck.Namespace}.{typeToCheck.Name}" : typeToCheck.FullName) == fullBaseTypeName) 
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            return type.GetInterfaces().Any(t => (includeBase || type != t) && (t.IsGenericType ? $"{t.Namespace}.{t.Name}" : t.FullName) == fullBaseTypeName);
        }

        public static bool Implements(Type type, Type baseType) => baseType.IsAssignableFrom(type);
    }
}