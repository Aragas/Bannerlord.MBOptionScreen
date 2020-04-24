using MBOptionScreen.Utils;

using System;

using TaleWorlds.Library;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class VersionAttribute : Attribute
    {
        public ApplicationVersion GameVersion { get; }
        public int ImplementationVersion { get; }

        public VersionAttribute(string gameVersion, int implementationVersion)
        {
            GameVersion = ApplicationVersionUtils.TryParse(gameVersion, out var v) ? v : default;
            ImplementationVersion = implementationVersion;
        }
    }
}