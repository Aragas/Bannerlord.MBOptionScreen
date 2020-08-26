using System.Reflection;

namespace MCM.Tests
{
    public static class DelegateHelper
    {
        public delegate bool MockedGetBasePathDelegate(ref string __result);
        public static MethodInfo GetMethodInfo(this MockedGetBasePathDelegate @delegate) => @delegate.Method;
    }
}