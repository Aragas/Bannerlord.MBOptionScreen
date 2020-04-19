using System;

using TaleWorlds.Library;

namespace MBOptionScreen.Attributes
{
    public abstract class BaseVersionAttribute : Attribute
    {
        public ApplicationVersion GameVersion { get; }
        public int ImplementationVersion { get; }

        protected BaseVersionAttribute(string gameVersion, int implementationVersion)
        {
            GameVersion = ApplicationVersionParser.TryParse(gameVersion, out var v) ? v : default;
            ImplementationVersion = implementationVersion;
        }
    }
}