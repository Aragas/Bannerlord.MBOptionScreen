using HarmonyLib;

using MCM.Utils;

using System;
using System.Reflection;

using TaleWorlds.Library;

namespace MCM.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class VersionAttributeWrapper : Attribute
    {
        private readonly object _object;
        private PropertyInfo GameVersionProperty { get; }
        private PropertyInfo ImplementationVersionProperty { get; }

        public ApplicationVersion GameVersion => GameVersionProperty.GetValue(_object) as ApplicationVersion? ?? ApplicationVersion.Empty;
        public int ImplementationVersion => ImplementationVersionProperty.GetValue(_object) as int? ?? 0;

        public VersionAttributeWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            GameVersionProperty = AccessTools.Property(type, nameof(GameVersion));
            ImplementationVersionProperty = AccessTools.Property(type, nameof(ImplementationVersion));
        }
    }

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