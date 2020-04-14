using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ResourceInjectorVersionAttribute : BaseVersionAttribute
    {
        public ResourceInjectorVersionAttribute(string gameVersion, int implementationVersion) : base(gameVersion, implementationVersion) { }
    }
}