using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SettingsStorageVersionAttribute : BaseVersionAttribute
    {
        public SettingsStorageVersionAttribute(string gameVersion, int implementationVersion) : base(gameVersion, implementationVersion) { }
    }
}