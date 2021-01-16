using System;
using Bannerlord.BUTR.Shared.Helpers;
using TaleWorlds.Library;

namespace MCM.Abstractions.Attributes
{
    /// <summary>
    /// Declares that the implementation works on the versions specified
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class VersionAttribute : Attribute, IVersion
    {
        public ApplicationVersion GameVersion { get; }
        public int ImplementationVersion { get; }

        public VersionAttribute(string gameVersion, int implementationVersion)
        {
            GameVersion = default;
            ImplementationVersion = implementationVersion;
        }
    }
}