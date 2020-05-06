using HarmonyLib;

using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MCM.Abstractions.Settings;

namespace MCM.Implementation.v1
{
    public class BaseSettingsDatabasePatch
    {
        protected static Type? GetSettingsDatabaseType() => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .FirstOrDefault(a => Path.GetFileName(a.Location) == "MBOptionScreen.dll")?
            .GetType("MBOptionScreen.Settings.SettingsDatabase");
    }

    public sealed class SettingsDatabasePatch1 : BaseSettingsDatabasePatch
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