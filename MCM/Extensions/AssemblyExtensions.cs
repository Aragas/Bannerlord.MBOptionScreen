using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Extensions
{
    /// <summary>
    /// here's a get type defs mechanism that's both safe, faster, and ecma 335 approved
    /// </summary>
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type?> GetTypesFast(this Assembly assembly) => Enumerable.Range(2, 0xFFFFFF - 2)
            .Select(i => { try { return assembly.ManifestModule.ResolveType(0x02000000 | i); } catch { return null; } })
            .TakeWhile(x => x != null);
    }
}