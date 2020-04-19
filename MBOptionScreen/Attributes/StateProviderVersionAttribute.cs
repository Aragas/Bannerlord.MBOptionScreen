using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class StateProviderVersionAttribute : BaseVersionAttribute
    {
        public StateProviderVersionAttribute(string gameVersion, int implementationVersion) : base(gameVersion, implementationVersion) { }
    }
}