using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.v2
{
    public sealed class MBOptionScreenSubModulePatch1
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();
            foreach (var assembly in assemblies)
            {
                if (Path.GetFileNameWithoutExtension(assembly.Location).StartsWith("MBOptionScreen.v2"))
                {
                    var type = assembly.GetType("MBOptionScreen.MBOptionScreenSubModule") ??
                               assembly.GetType("MBOptionScreen.MBOptionScreenSubModuleV2010") ??
                               assembly.GetType("MBOptionScreen.MBOptionScreenSubModuleV2011");

                    if (type != null)
                    {
                        var method = AccessTools.Method(type, "OnSubModuleLoad");
                        if (method != null)
                            yield return method;
                    }
                }
            }
        }

        public static bool Prefix() => false;
    }

    public sealed class MBOptionScreenSubModulePatch2
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();
            foreach (var assembly in assemblies)
            {
                if (Path.GetFileNameWithoutExtension(assembly.Location).StartsWith("MBOptionScreen.v2"))
                {
                    var type = assembly.GetType("MBOptionScreen.MBOptionScreenSubModule") ??
                               assembly.GetType("MBOptionScreen.MBOptionScreenSubModuleV2010") ??
                               assembly.GetType("MBOptionScreen.MBOptionScreenSubModuleV2011");

                    if (type != null)
                    {
                        var method = AccessTools.Method(type, "OnBeforeInitialModuleScreenSetAsRoot");
                        if (method != null)
                            yield return method;
                    }
                }
            }
        }

        public static bool Prefix() => false;
    }
}