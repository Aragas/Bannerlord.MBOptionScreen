using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MCM.Abstractions.Settings.Providers;
using MCM.Utils;

namespace MCM.Implementation.MBO
{
    internal class BaseSettingsDatabaseV1Patch
    {
        protected static Type? GetSettingsDatabaseType() => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .FirstOrDefault(a => Path.GetFileName(a.Location) == "MBOptionScreen.dll")?
            .GetType("MBOptionScreen.Settings.SettingsDatabase");
    }

    internal sealed class SettingsDatabaseV1Patch1 : BaseSettingsDatabaseV1Patch
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

    internal class BaseSettingsDatabaseV2Patch
    {
        protected static IEnumerable<Type?> GetSettingsDatabaseTypes() => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Where(a => Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen.v"))
            .Select(a => a?.GetType("MBOptionScreen.Settings.SettingsDatabase"));
    }

    internal class SettingsDatabaseV2Patch1 : BaseSettingsDatabaseV2Patch
    {
        public static IEnumerable<MethodBase> TargetMethods() => Enumerable.Select<Type, MethodInfo>(GetSettingsDatabaseTypes(), type => AccessTools.Method(type, "GetSettings"))
            .Where(m => m != null);

        public static bool Prefix(ref object? __result, string id)
        {
            __result = SettingsUtils.UnwrapSettings(BaseSettingsProvider.Instance.GetSettings(id));
            return false;
        }
    }
}