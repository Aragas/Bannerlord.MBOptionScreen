using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Library;

namespace MCM.Tests
{
    public static class DelegateHelper
    {
        public delegate bool MockedGetBasePathDelegate(ref string __result);
        public static MethodInfo GetMethodInfo(this MockedGetBasePathDelegate @delegate) => @delegate.Method;

        public delegate bool MockedGetLoadedModulesDelegate(ref List<ModuleInfo> list);
        public static MethodInfo GetMethodInfo(MockedGetLoadedModulesDelegate @delegate) => @delegate.Method;
    }
}