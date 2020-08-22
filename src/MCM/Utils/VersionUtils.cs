using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;

using MCM.Abstractions;
using MCM.Abstractions.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Library;

namespace MCM.Utils
{
    public static class VersionUtils
    {
        private static IEnumerable<IVersion> GetVersions(MemberInfo memberInfo) => memberInfo.GetCustomAttributes()
            .Where(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(VersionAttribute)))
            .Select(a => new VersionAttributeWrapper(a));

        private static IVersion? GetLatestMatching(IEnumerable<IVersion> versions, ApplicationVersion gameVersion) =>
            versions.Where(a => a.GameVersion.IsSameWithoutRevision(gameVersion))
                .OrderByDescending(a => a.ImplementationVersion)
                .ThenByDescending(a => a.GameVersion, new ApplicationVersionComparer())
                .FirstOrDefault();
        private static IVersion? GetLatestMatchingMajorMinor(IEnumerable<IVersion> versions, ApplicationVersion gameVersion) =>
            versions.Where(a => a.GameVersion.Major == gameVersion.Major && a.GameVersion.Minor == gameVersion.Minor)
                .OrderByDescending(a => a.ImplementationVersion)
                .ThenByDescending(a => a.GameVersion, new ApplicationVersionComparer())
                .FirstOrDefault();
        private static IVersion? GetLatest(IEnumerable<IVersion> versions) =>
            versions.OrderByDescending(a => a.ImplementationVersion)
                .ThenByDescending(a => a.GameVersion, new ApplicationVersionComparer())
                .FirstOrDefault();

        public static (Type Type, IVersion Version)? GetLastImplementation(ApplicationVersion version, IEnumerable<Type> types)
        {
            var attributes = types.ToDictionary(type => type, GetVersions);
            var latestImplementation = default((Type Type, IVersion Attribute));

            foreach (var (key, value) in attributes)
            {
                var latestTypeVersion = GetLatestMatching(value, version);
                if (latestTypeVersion == null ||
                    (latestImplementation.Attribute != null &&
                    latestImplementation.Attribute.ImplementationVersion >= latestTypeVersion.ImplementationVersion))
                    continue;

                latestImplementation.Type = key;
                latestImplementation.Attribute = latestTypeVersion;
            }
            if (latestImplementation.Type != null)
                return latestImplementation;


            foreach (var (key, value) in attributes) // no matching game version, using the latest major.minor.ANY
            {
                var latestTypeVersion = GetLatestMatchingMajorMinor(value, version);
                if (latestTypeVersion == null ||
                    (latestImplementation.Attribute != null &&
                    latestImplementation.Attribute.ImplementationVersion >= latestTypeVersion.ImplementationVersion))
                    continue;

                latestImplementation.Type = key;
                latestImplementation.Attribute = latestTypeVersion;
            }
            if (latestImplementation.Type != null)
                return latestImplementation;


            foreach (var (key, value) in attributes) // no matching major.minor game version, using the latest
            {
                var latestTypeVersion = GetLatest(value);
                if (latestTypeVersion == null ||
                    (latestImplementation.Attribute != null &&
                    latestImplementation.Attribute.ImplementationVersion >= latestTypeVersion.ImplementationVersion))
                    continue;

                latestImplementation.Type = key;
                latestImplementation.Attribute = latestTypeVersion;
            }
            return latestImplementation;
        }

        private class ApplicationVersionComparer : IComparer<ApplicationVersion>
        {
            public int Compare(ApplicationVersion x, ApplicationVersion y)
            {
                if (x.IsSameWithRevision(y))
                    return 0;
                else
                    return
                        x.ApplicationVersionType != y.ApplicationVersionType ? (x.ApplicationVersionType > y.ApplicationVersionType ? 1 : -1) :
                        x.Major != y.Major ? (x.Major > y.Major ? 1 : -1) :
                        x.Minor != y.Minor ? (x.Minor > y.Minor ? 1 : -1) :
                        x.Revision != y.Revision ? (x.Revision > y.Revision ? 1 : -1) :
                        0;
            }
        }
    }
}