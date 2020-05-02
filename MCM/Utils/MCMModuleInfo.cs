using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace MCM.Utils
{
    internal class MCMModuleInfo
    {
        private static readonly FieldInfo? _loadedSubmoduleTypesField = AccessTools.Field(typeof(TaleWorlds.MountAndBlade.Module), "_loadedSubmoduleTypes");
        public static List<MCMModuleInfo> GetLoadedSubModules()
        {
            //MBDebug.Print("Loading submodules...");
            var moduleInfoList = new List<ModuleInfo>();
            var moduleNames = Utilities.GetModulesNames();
            foreach (var moduleName in moduleNames)
            {
                var moduleInfo = new ModuleInfo();
                moduleInfo.Load(moduleName);
                moduleInfoList.Add(moduleInfo);
            }
            var loadedSubmoduleTypes = _loadedSubmoduleTypesField?.GetValue(TaleWorlds.MountAndBlade.Module.CurrentModule) as IDictionary<string, Type> ?? new Dictionary<string, Type>();
            var modules = new List<MCMModuleInfo>();
            foreach (var moduleInfo in moduleInfoList)
            {
                var module = new MCMModuleInfo(moduleInfo);
                foreach (var subModule in moduleInfo.SubModules.Where(subModule => loadedSubmoduleTypes.ContainsKey(subModule.SubModuleClassType)))
                    module.SubModuleInfos.Add(new MCMSubModuleInfo(module, subModule, loadedSubmoduleTypes[subModule.SubModuleClassType]));
                modules.Add(module);
            }

            return modules;
        }


        public ModuleInfo ModuleInfo { get; }
        public List<MCMSubModuleInfo> SubModuleInfos { get; } = new List<MCMSubModuleInfo>();

        public MCMModuleInfo(ModuleInfo moduleInfo)
        {
            ModuleInfo = moduleInfo;
        }

        public override string ToString() => ModuleInfo.Name;

        public DirectoryInfo? GetBinaryDirectory()
        {
            return ModulePath()?
                .GetDirectories()
                .FirstOrDefault(f => string.Equals(f.Name, ModuleInfo.Alias, StringComparison.Ordinal))?
                .GetDirectories("bin")
                .FirstOrDefault()?
                .GetDirectories(Common.ConfigName)
                .FirstOrDefault();
        }

        private static DirectoryInfo? ModulePath()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                var file = new FileInfo(assembly.Location);
                return file.Directory?.Parent?.Parent?.GetDirectories("Modules").FirstOrDefault();
            }
            return null;
        }
    }
    internal class MCMSubModuleInfo
    {
        public MCMModuleInfo Owner { get; }
        public SubModuleInfo SubModuleInfo { get; }
        public Type SubModuleType { get; }

        public MCMSubModuleInfo(MCMModuleInfo owner, SubModuleInfo subModuleInfo, Type subModuleType)
        {
            Owner = owner;
            SubModuleInfo = subModuleInfo;
            SubModuleType = subModuleType;
        }

        public override string ToString() => SubModuleInfo.Name;
    }
}