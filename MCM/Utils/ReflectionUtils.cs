using System;
using System.Linq;

namespace MCM.Utils
{
    public static class ReflectionUtils
    {
        public static bool ImplementsOrImplementsEquivalent(Type type, Type baseType) =>
            ImplementsOrImplementsEquivalent(type, (baseType.IsGenericType ? $"{baseType.Namespace}.{baseType.Name}" : baseType.FullName));
        public static bool ImplementsOrImplementsEquivalent(Type type, string fullBaseTypeName)
        {
            var typeToCheck = type;
            while (typeToCheck != null)
            {
                if ((typeToCheck.IsGenericType ? $"{typeToCheck.Namespace}.{typeToCheck.Name}" : typeToCheck.FullName) == fullBaseTypeName) 
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            return type.GetInterfaces().Any(t => (t.IsGenericType ? $"{t.Namespace}.{t.Name}" : t.FullName) == fullBaseTypeName);
        }

        public static bool Implements(Type type, Type baseType) => baseType.IsAssignableFrom(type);
    }
}