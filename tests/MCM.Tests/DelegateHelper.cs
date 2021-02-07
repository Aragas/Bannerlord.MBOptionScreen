using Bannerlord.ButterLib.Common.Helpers;

using System.Collections.Generic;
using System.Reflection;

namespace MCM.Tests
{
    public static class DelegateHelper
    {
        public delegate bool MockedGetBasePathDelegate(ref string result);
        public static MethodInfo GetMethodInfo(this MockedGetBasePathDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetLoadedModulesDelegate(ref List<ExtendedModuleInfo> list);
        public static MethodInfo GetMethodInfo(MockedGetLoadedModulesDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetModulesNamesDelegate(ref string[] __result);
        public static MethodInfo GetMethodInfo(MockedGetModulesNamesDelegate @delegate) => @delegate.Method;
    }
}