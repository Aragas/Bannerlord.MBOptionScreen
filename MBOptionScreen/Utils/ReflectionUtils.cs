using System;
using System.Linq;

namespace MBOptionScreen.Utils
{
    internal static class ReflectionUtils
    {
        public static bool ImplementsOrImplementsEquivalent(Type type, Type baseType) => ImplementsOrImplementsEquivalent(type, baseType.FullName);

        public static bool ImplementsOrImplementsEquivalent(Type type, string fullBaseTypeName)
        {
            var typeToCheck = type;
            while (typeToCheck != null)
            {
                if (typeToCheck.FullName == fullBaseTypeName || (typeToCheck.IsGenericType && $"{typeToCheck.Namespace}.{typeToCheck.Name}" == fullBaseTypeName))
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            return type.GetInterfaces().Any(i => i.FullName == fullBaseTypeName);
        }

        public static bool Implements(Type type, Type baseType) => baseType.IsAssignableFrom(type);
    }
}