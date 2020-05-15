using MCM.Abstractions.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace MCM.Utils
{
    public static class AttributeUtils
    {
        // Rewrite
        public static (Type Type, ApplicationVersion GameVersion, int ImplementationVersion)? GetLastImplementation(ApplicationVersion version, IEnumerable<Type>? types = null)
        {
            types ??= AppDomain.CurrentDomain.GetAssemblies()
                .Parallel()
                .Filter()
                .FilterLegacy()
                .SelectMany(a => a.GetTypes());

            var attributes = new Dictionary<Type, IEnumerable<VersionAttributeWrapper>>();
            foreach (var type in types.AsParallel())
            {
                var attr = type.GetCustomAttributes()
                    .Where(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(VersionAttribute)))
                    .Select(a => new VersionAttributeWrapper(a))
                    .ToList();
                if (attr.Count > 0)
                    attributes.Add(type, attr);
            }

            (Type Type, VersionAttributeWrapper Attribute) maxMatching = default;
            foreach (var pair in attributes)
            {
                var maxFound = TaleWorlds.Core.Extensions.MaxBy(pair.Value
                    .Where(a => a.GameVersion.IsSameOverride(version))
                    .DefaultIfEmpty(), a => a?.ImplementationVersion);

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

            return maxMatching != default ? (maxMatching.Type!, maxMatching.Attribute.GameVersion, maxMatching.Attribute.ImplementationVersion) : default;
        }

        public static Dictionary<Type, (ApplicationVersion GameVersion, int ImplementationVersion)> GetMultiple(ApplicationVersion version, IEnumerable<Type>? types = null)
        {
            types ??= AppDomain.CurrentDomain.GetAssemblies()
                .Parallel()
                .Filter()
                .FilterLegacy()
                .SelectMany(a => a.GetTypes());

            var attributes = new Dictionary<Type, IEnumerable<VersionAttributeWrapper>>();
            foreach (var type in types.AsParallel())
            {
                var attr = type.GetCustomAttributes()
                    .Where(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(VersionAttribute)))
                    .Select(a => new VersionAttributeWrapper(a))
                    .ToList();
                if (attr.Count > 0)
                    attributes.Add(type, attr);
            }

            var allMatching = new Dictionary<Type, (ApplicationVersion GameVersion, int ImplementationVersion)>();
            foreach (var pair in attributes)
            {
                var maxFound = TaleWorlds.Core.Extensions.MaxBy(pair.Value
                    .Where(a => a.GameVersion.IsSameOverride(version))
                    .DefaultIfEmpty(), a => a?.ImplementationVersion);

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