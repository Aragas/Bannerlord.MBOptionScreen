using MCM.Abstractions.Attributes;
using MCM.Abstractions.Loader;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.Loader
{
    [Version("e1.0.0",  2)]
    [Version("e1.0.1",  2)]
    [Version("e1.0.2",  2)]
    [Version("e1.0.3",  2)]
    [Version("e1.0.4",  2)]
    [Version("e1.0.5",  2)]
    [Version("e1.0.6",  2)]
    [Version("e1.0.7",  2)]
    [Version("e1.0.8",  2)]
    [Version("e1.0.9",  2)]
    [Version("e1.0.10", 2)]
    [Version("e1.0.11", 2)]
    [Version("e1.1.0",  2)]
    [Version("e1.2.0",  2)]
    [Version("e1.2.1",  2)]
    [Version("e1.3.0",  2)]
    [Version("e1.3.1",  2)]
    [Version("e1.4.0",  2)]
    [Version("e1.4.1",  2)]
    public class IntegratedLoader : IIntegratedLoader
    {
        private readonly List<Assembly> _mcmReferencingAssemblies = new List<Assembly>();
        private readonly List<Assembly> _mcmImplementationAssemblies = new List<Assembly>();
        public List<(MBSubModuleBase, Type)> MCMImplementationSubModules { get; } = new List<(MBSubModuleBase, Type)>();

        public void Load()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();

            _mcmImplementationAssemblies.Add(typeof(IntegratedLoaderSubModule).Assembly);

            // Loading as Standalone
            foreach (var assembly in assemblies.Where(assembly => assembly.GetName().Name == "MCMv3"))
            {
                _mcmReferencingAssemblies.Add(assembly);
            }
            // Loading as Integrated
            foreach (var assembly in assemblies)
            {
                var referencedAssemblies = assembly.GetReferencedAssemblies();
                if (referencedAssemblies.All(r => r.Name != "MCMv3"))
                    continue;
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
                var matches = assemblyDirectory.GetFiles("MCMv3.dll")
                    .Concat(assemblyDirectory.GetFiles("MCMv3.Implementation.*.dll")
                        .Where(f => f.Name != "MCMv3.Implementation.v3.1.0.dll")
                        .Where(f => f.Name != "MCMv3.Implementation.v3.1.1.dll")
                        .Where(f => f.Name != "MCMv3.Implementation.v3.1.2.dll")
                        .Where(f => f.Name != "MCMv3.Implementation.v3.1.3.dll")
                        .Where(f => f.Name != "MCMv3.Implementation.v3.1.4.dll")) // Ignore 3.1.0 and 3.1.1
                    .Concat(assemblyDirectory.GetFiles("MCMv3.UI.v*.dll")
                        .Where(f => f.Name != "MCMv3.UI.v3.1.0.dll")
                        .Where(f => f.Name != "MCMv3.UI.v3.1.1.dll")
                        .Where(f => f.Name != "MCMv3.UI.v3.1.2.dll")
                        .Where(f => f.Name != "MCMv3.UI.v3.1.3.dll")
                        .Where(f => f.Name != "MCMv3.UI.v3.1.4.dll")) // Ignore 3.1.0 and 3.1.0
                    .Concat(assemblyDirectory.GetFiles("MCMv3.Custom.*.dll")) // Might be useful later
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