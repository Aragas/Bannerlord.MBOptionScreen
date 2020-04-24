using System;
using System.Reflection;
using System.Runtime.Serialization;

using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace MBOptionScreen.Utils
{
    internal static class ApplicationVersionUtils
    {
        private static ConstructorInfo ApplicationVersionConstructorV1 { get; } =
            typeof(ApplicationVersion).GetConstructor(new Type[]
            {
                typeof(ApplicationVersionType),
                typeof(int),
                typeof(int),
                typeof(int),
                typeof(int)
            });
        private static ConstructorInfo ApplicationVersionConstructorV2 { get; } =
            typeof(ApplicationVersion).GetConstructor(new Type[]
            {
                        typeof(ApplicationVersionType),
                        typeof(int),
                        typeof(int),
                        typeof(int)
            });
        private static PropertyInfo ApplicationVersionTypeProperty { get; } =
            typeof(ApplicationVersion).GetProperty("ApplicationVersionType");
        private static PropertyInfo MajorProperty { get; } =
            typeof(ApplicationVersion).GetProperty("Major");
        private static PropertyInfo MinorProperty { get; } =
            typeof(ApplicationVersion).GetProperty("Minor");
        private static PropertyInfo RevisionProperty { get; } =
            typeof(ApplicationVersion).GetProperty("Revision");

        public static bool TryParse(string versionAsString, out ApplicationVersion version)
        {
            version = default;

            var array = versionAsString.Split('.');
            if (array.Length != 3 && array.Length != 4)
                return false;

            var applicationVersionType = ApplicationVersion.ApplicationVersionTypeFromString(array[0][0].ToString());
            if(!int.TryParse(array[0].Substring(1), out var major))
                return false;
            if (!int.TryParse(array[1], out var minor))
                return false;
            if (!int.TryParse(array[2], out var revision))
                return false;

            version = Create(applicationVersionType, major, minor, revision);
            return true;
        }

        /// <summary>
        /// e1.0.11 didn't had ChangeSet.
        /// It may be an overkill for such a minor game version, but why not.
        /// </summary>
        private static ApplicationVersion Create(ApplicationVersionType applicationVersionType, int major, int minor, int revision)
        {
            if (ApplicationVersionConstructorV1 != null)
                return (ApplicationVersion) ApplicationVersionConstructorV1.Invoke(new object[] { applicationVersionType, major, minor, revision, 0 });

            if (ApplicationVersionConstructorV2 != null)
                return (ApplicationVersion) ApplicationVersionConstructorV2.Invoke(new object[] { applicationVersionType, major, minor, revision });

            // Fallback
            var version = (ApplicationVersion) FormatterServices.GetUninitializedObject(typeof(ApplicationVersion));
            ApplicationVersionTypeProperty?.SetValue(version, applicationVersionType);
            MajorProperty?.SetValue(version, major);
            MinorProperty?.SetValue(version, minor);
            RevisionProperty?.SetValue(version, revision);
            return version;
        }

        public static bool IsSameOverride(this ApplicationVersion @this, ApplicationVersion other)
        {
            return @this.ApplicationVersionType == other.ApplicationVersionType &&
                   @this.Major == other.Major &&
                   @this.Minor == other.Minor &&
                   @this.Revision == other.Revision;
        }

        public static ApplicationVersion GameVersion() => TryParse(Managed.GetVersionStr(), out var v) ? v : default;
    }
}