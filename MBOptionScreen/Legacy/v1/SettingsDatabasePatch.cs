using HarmonyLib;

using MBOptionScreen.Settings;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen.Legacy.v1
{
    public class BaseSettingsDatabasePatch
    {
        protected static Type GetSettingsDatabase()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .FirstOrDefault(a => Path.GetFileName(a.Location) == "MBOptionScreen.dll");

            return assembly.GetType("MBOptionScreen.Settings.SettingsDatabase");
        }

        public static void PatchAll()
        {
            var harmony = new Harmony("bannerlord.mboptionscreen.settingsdatabase_v1");
            harmony.Patch(
                original: SettingsDatabasePatch1.TargetMethod(),
                prefix: new HarmonyMethod(typeof(SettingsDatabasePatch1), nameof(SettingsDatabasePatch1.Prefix)));
        }
    }

    public class SettingsDatabasePatch1 : BaseSettingsDatabasePatch
    {
        public static MethodBase TargetMethod() => AccessTools.Method(GetSettingsDatabase(), "GetSettings");

        public static bool Prefix(ref object __result, string id)
        {
            var settings = SettingsDatabase.GetSettings(id);
            if (settings is SettingsWrapper settingsWrapper)
                __result = settingsWrapper._object;
            else
                __result = settings;

            return false;
        }
    }
}
