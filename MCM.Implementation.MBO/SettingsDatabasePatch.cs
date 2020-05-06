using HarmonyLib;

using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.v1
{
    internal class BaseSettingsDatabasePatch
    {
        protected static Type? GetSettingsDatabaseType() => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .FirstOrDefault(a => Path.GetFileName(a.Location) == "MBOptionScreen.dll")?
            .GetType("MBOptionScreen.Settings.SettingsDatabase");
    }

    internal sealed class SettingsDatabasePatch1 : BaseSettingsDatabasePatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            var type = GetSettingsDatabaseType();
            if (type != null)
                yield return AccessTools.Method(type, "GetSettings");
        }

        public static bool Prefix(ref object? __result, string id)
        {
            __result = SettingsUtils.UnwrapSettings(BaseSettingsProvider.Instance.GetSettings(id));
            return false;
        }
    }
}

namespace MCM.Implementation.v2
{
    internal class BaseSettingsDatabasePatch
    {
        protected static IEnumerable<Type?> GetSettingsDatabaseTypes() => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Where(a => Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen.v"))
            .Select(a => a?.GetType("MBOptionScreen.Settings.SettingsDatabase"));
    }

    internal class SettingsDatabasePatch1 : BaseSettingsDatabasePatch
    {
        public static IEnumerable<MethodBase> TargetMethods() => GetSettingsDatabaseTypes()
            .Select(type => AccessTools.Method(type, "GetSettings"))
            .Where(m => m != null);

        public static bool Prefix(ref object? __result, string id)
        {
            __result = SettingsUtils.UnwrapSettings(BaseSettingsProvider.Instance.GetSettings(id));
            return false;
        }
    }
}