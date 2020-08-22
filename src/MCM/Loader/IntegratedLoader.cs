using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.MountAndBlade;

namespace MCM.Loader
{
    internal class IntegratedLoader
    {
        private readonly List<Assembly> _mcmReferencingAssemblies = new List<Assembly>();
        private readonly List<Assembly> _mcmImplementationAssemblies = new List<Assembly>();
        public List<(MBSubModuleBase, Type)> MCMImplementationSubModules { get; } = new List<(MBSubModuleBase, Type)>();

        public void Load()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();

            _mcmImplementationAssemblies.Add(typeof(IntegratedLoaderSubModule).Assembly);

            // Loading as Standalone
            foreach (var assembly in assemblies.Where(assembly => assembly.GetName().Name == "MCMv4"))
            {
                _mcmReferencingAssemblies.Add(assembly);
            }

            foreach (var assembly in _mcmReferencingAssemblies)
            {
                var assemblyFile = new FileInfo(assembly.Location);
                if (!assemblyFile.Exists)
                    continue;
                var assemblyDirectory = assemblyFile.Directory;
                if (assemblyDirectory == null || !assemblyDirectory.Exists)
                    continue;
                var matches = assemblyDirectory.GetFiles("MCMv3.Custom.*.dll")
                    .ToList();
                if (!matches.Any())
                    continue;

                foreach (var match in matches.Where(m => assemblies.All(a => Path.GetFileName(a.Location) != Path.GetFileName(m.Name))))
                {
                    _mcmImplementationAssemblies.Add(Assembly.LoadFrom(match.FullName));
                    assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();
                }
            }

            var submodules = _mcmImplementationAssemblies.SelectMany(assembly => assembly.GetTypes().Where(t =>
                t.FullName != typeof(IntegratedLoaderSubModule).FullName && typeof(MBSubModuleBase).IsAssignableFrom(t)));
            foreach (var subModuleType in submodules)
            {
                if (subModuleType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, Type.EmptyTypes, null)?.Invoke(Array.Empty<object>()) is MBSubModuleBase subModule)
                    MCMImplementationSubModules.Add((subModule, subModuleType));
            }

            StaticDI.Update();
        }
    }
}