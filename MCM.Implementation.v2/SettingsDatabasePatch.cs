using HarmonyLib;

using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.v2
{
    public class BaseSettingsDatabasePatch
    {
        protected static IEnumerable<Type?> GetSettingsDatabaseTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Where(a => Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen.v"))
                .Select(a => a?.GetType("MBOptionScreen.Settings.SettingsDatabase"));
        }
    }

    public class SettingsDatabasePatch1 : BaseSettingsDatabasePatch
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