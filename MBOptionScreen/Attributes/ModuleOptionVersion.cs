using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ModuleOptionVersionAttribute : BaseVersionAttribute
    {
        public ModuleOptionVersionAttribute(string gameVersion, int implementationVersion) : base(gameVersion, implementationVersion) { }
    }
}