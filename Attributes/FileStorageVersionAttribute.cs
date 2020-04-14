using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class FileStorageVersionAttribute : BaseVersionAttribute
    {
        public FileStorageVersionAttribute(string gameVersion, int implementationVersion) : base(gameVersion, implementationVersion) { }
    }
}