extern alias v4;

using Bannerlord.BUTR.Shared.ModuleInfoExtended;

using System.Collections.Generic;
using System.Reflection;

using v4::Bannerlord.ModuleManager;

namespace MCM.Tests
{
    internal static class DelegateHelper
    {
        public delegate bool MockedGetBasePathDelegate(ref string result);
        public static MethodInfo GetMethodInfo(this MockedGetBasePathDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetLoadedModulesDelegate(ref IEnumerable<ModuleInfoExtended> list);
        public static MethodInfo GetMethodInfo(MockedGetLoadedModulesDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetModulesNamesDelegate(ref string[] __result);
        public static MethodInfo GetMethodInfo(MockedGetModulesNamesDelegate @delegate) => @delegate.Method;
    }
}