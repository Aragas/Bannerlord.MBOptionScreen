using MBOptionScreen.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace MBOptionScreen.Utils
{
    internal static class AttributeUtils
    {
        // Rewrite
        public static (Type Type, ApplicationVersion GameVersion, int ImplementationVersion) Get(ApplicationVersion version, IEnumerable<Type>? types = null)
        {
            types ??= AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.DefinedTypes);

            var attributes = types
                .Where(t => t.GetCustomAttributes().Any(a => a.GetType().FullName == typeof(VersionAttribute).FullName))
                .ToDictionary(
                    k => k,
                    v => v.GetCustomAttributes()
                        .Where(a => a.GetType().FullName == typeof(VersionAttribute).FullName)
                        .Select(a => new VersionAttributeWrapper(a)));

            (Type Type, VersionAttributeWrapper Attribute) maxMatching = default;
            foreach (var pair in attributes)
            {
                var maxFound = pair.Value
                    .Where(a => a.GameVersion.IsSameOverride(version))
                    .DefaultIfEmpty()
                    .MaxBy(a => a?.ImplementationVersion);

                if (maxFound == null)
                    continue;

                if (maxMatching.Attribute == null)
                {
                    maxMatching.Type = pair.Key;
                    maxMatching.Attribute = maxFound;
                }

                if (maxMatching.Attribute.ImplementationVersion < maxFound.ImplementationVersion)
                {
                    maxMatching.Type = pair.Key;
                    maxMatching.Attribute = maxFound;
                }
            }

            if (maxMatching.Type == null) // no matching game version, using the latest major.minor.ANY
            {
                foreach (var pair in attributes)
                {
                    var maxFound = pair.Value
                        .Where(a => a.GameVersion.Major == version.Major && a.GameVersion.Minor == version.Minor)
                        .OrderByDescending(a => a.ImplementationVersion)
                        .ThenByDescending(a => a.GameVersion, new ApplicationVersionComparer())
                        .FirstOrDefault();

                    if (maxFound == null)
                        continue;

                    if (maxMatching.Attribute == null)
                    {
                        maxMatching.Type = pair.Key;
                        maxMatching.Attribute = maxFound;
                    }

                    if (maxMatching.Attribute.ImplementationVersion < maxFound.ImplementationVersion)
                    {
                        maxMatching.Type = pair.Key;
                        maxMatching.Attribute = maxFound;
                    }
                }
            }

            if (maxMatching.Type == null) // no matching major.minor game version, using the latest
            {
                foreach (var pair in attributes)
                {
                    var maxFound = pair.Value
                        .OrderByDescending(a => a.ImplementationVersion)
                        .ThenByDescending(a => a.GameVersion, new ApplicationVersionComparer())
                        .FirstOrDefault();

                    if (maxFound == null)
                        continue;

                    if (maxMatching.Attribute == null)
                    {
                        maxMatching.Type = pair.Key;
                        maxMatching.Attribute = maxFound;
                    }

                    if (maxMatching.Attribute.ImplementationVersion < maxFound.ImplementationVersion)
                    {
                        maxMatching.Type = pair.Key;
                        maxMatching.Attribute = maxFound;
                    }
                }
            }

            return (maxMatching.Type, maxMatching.Attribute.GameVersion, maxMatching.Attribute.ImplementationVersion);
        }

        public static Dictionary<Type, (ApplicationVersion GameVersion, int ImplementationVersion)> GetMultiple(ApplicationVersion version, IEnumerable<Type>? types = null)
        {
            types ??= AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes());

            var attributes = types
                .Where(t => t.GetCustomAttributes().Any(a => a.GetType().FullName == typeof(VersionAttribute).FullName))
                .ToDictionary(
                    k => k,
                    v => v.GetCustomAttributes()
                        .Where(a => a.GetType().FullName == typeof(VersionAttribute).FullName)
                        .Select(a => new VersionAttributeWrapper(a)));

            var allMatching = new Dictionary<Type, (ApplicationVersion GameVersion, int ImplementationVersion)>();
            foreach (var pair in attributes)
            {
                var maxFound = pair.Value
                    .Where(a => a.GameVersion.IsSameOverride(version))
                    .DefaultIfEmpty()
                    .MaxBy(a => a?.ImplementationVersion);

                if (maxFound == null)
                    continue;

                if (allMatching.ContainsKey(pair.Key))
                {
                    if (allMatching[pair.Key].ImplementationVersion < maxFound.ImplementationVersion)
                    {
                        allMatching[pair.Key] = (maxFound.GameVersion, maxFound.ImplementationVersion);
                    }
                }
                else
                {
                    allMatching[pair.Key] = (maxFound.GameVersion, maxFound.ImplementationVersion);
                }
            }

            return allMatching;
        }

        private class ApplicationVersionComparer : IComparer<ApplicationVersion>
        {
            public int Compare(ApplicationVersion x, ApplicationVersion y)
            {
                if (x.IsSameOverride(y))
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