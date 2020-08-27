/*
using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Assemblies;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.SubModuleWrappers;

using MCM.Utils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MCM
{
    public class ImplementationLoaderSubModule : MBSubModuleBaseListWrapper
    {
        private delegate MBSubModuleBase ConstructorDelegate();

        private static IEnumerable<MBSubModuleBase> LoadAllImplementations(string filter, ILogger? logger = null)
        {
            logger?.LogInformation("Loading implementations...");

            var implementationAssemblies = new List<Assembly>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();

            var thisAssembly = typeof(ImplementationLoaderSubModule).Assembly;

            var assemblyFile = new FileInfo(thisAssembly.Location);
            if (!assemblyFile.Exists)
            {
                logger?.LogError("Assembly file does not exists!");
                yield break;
            }

            var assemblyDirectory = assemblyFile.Directory;
            if (assemblyDirectory?.Exists != true)
            {
                logger?.LogError("Assembly directory does not exists!");
                yield break;
            }

            var implementations = assemblyDirectory.GetFiles(filter);
            if (implementations.Length == 0)
            {
                logger?.LogError("No implementations found.");
                yield break;
            }

            var gameVersion = ApplicationVersionUtils.TryParse(ApplicationVersionUtils.GameVersionStr(), out var v) ? v : (ApplicationVersion?)null;
            if (gameVersion == null)
            {
                logger?.LogError("Failed to get Game version!");
                yield break;
            }


            var implementationsFiles = implementations.Where(x => assemblies.All(a => Path.GetFileNameWithoutExtension(a.Location) != Path.GetFileNameWithoutExtension(x.Name)));
            var implementationsWithVersions = GetImplementations(implementationsFiles, logger).ToList();
            if (implementationsWithVersions.Count == 0)
            {
                logger?.LogError("No compatible implementations were found!");
                yield break;
            }

            var implementationsForGameVersion = ImplementationForGameVersion(gameVersion.Value, implementationsWithVersions).ToList();
            switch (implementationsForGameVersion.Count)
            {
                case { } i when i > 1:
                {
                    logger?.LogInformation("Found multiple matching implementations:");
                    foreach (var (implementation1, version1) in implementationsForGameVersion)
                        logger?.LogInformation("Implementation {name} for game {gameVersion}.", implementation1.Name, version1);


                    logger?.LogInformation("Loading the latest available.");

                    var (implementation, version) = ImplementationLatest(implementationsForGameVersion);
                    logger?.LogInformation("Implementation {name} for game {gameVersion} is loaded.", implementation.Name, version);
                    implementationAssemblies.Add(Assembly.LoadFrom(implementation.FullName));
                    break;
                }

                case { } i when i == 1:
                {
                    logger?.LogInformation("Found matching implementation. Loading it.");

                    var (implementation, version) = implementationsForGameVersion[0];
                    logger?.LogInformation("Implementation {name} for game {gameVersion} is loaded.", implementation.Name, version);
                    implementationAssemblies.Add(Assembly.LoadFrom(implementation.FullName));
                    break;
                }

                case { } i when i == 0:
                {
                    logger?.LogInformation("Found no matching implementations. Loading the latest available.");

                    var (implementation, version) = ImplementationLatest(implementationsWithVersions);
                    logger?.LogInformation("Implementation {name} for game {gameVersion} is loaded.", implementation.Name, version);
                    implementationAssemblies.Add(Assembly.LoadFrom(implementation.FullName));
                    break;
                }
            }

            var subModules = implementationAssemblies.SelectMany(a =>
            {
                try
                {
                    return a.GetTypes().Where(t => typeof(MBSubModuleBase).IsAssignableFrom(t));
                }
                catch (Exception e) when (e is ReflectionTypeLoadException)
                {
                    logger?.LogError("Implementation {name} is not compatible with the current game!", Path.GetFileName(a.Location));
                    return Enumerable.Empty<Type>();
                }
            }).ToList();

            if (subModules.Count == 0)
                logger?.LogError("No implementation was initialized!");

            foreach (var subModuleType in subModules)
            {
                var constructor = subModuleType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, Type.EmptyTypes, null);
                if (constructor == null)
                {
                    logger?.LogError("SubModule {subModuleType} is missing a default constructor!", subModuleType);
                    continue;
                }

                var constructorFunc = ConstructorUtils.Delegate<ConstructorDelegate>(constructor);
                if (constructorFunc == null)
                {
                    logger?.LogError("SubModule {subModuleType}'s default constructor could not be converted to a delegate!", subModuleType);
                    continue;
                }

                yield return constructorFunc();
            }

            logger?.LogInformation("Finished loading implementations.");
        }

        private static IEnumerable<(FileInfo Implementation, ApplicationVersion Version)> GetImplementations(IEnumerable<FileInfo> implementations, ILogger? logger = null)
        {
            using var assemblyVerifier = new AssemblyVerifier("ButterLib");
            var assemblyLoader = assemblyVerifier.GetLoader(out var exception);
            if (assemblyLoader == null)
            {
                if (exception != null)
                    logger?.LogError(0, exception, "AssemblyLoadProxy could not be initialized.");
                else
                    logger?.LogError("AssemblyLoadProxy could not be initialized.");

                yield break;
            }

            // Load all current assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.FullName.StartsWith("mscorlib") && !a.IsDynamic && !string.IsNullOrEmpty(a.Location)))
                assemblyLoader.LoadFile(assembly.Location);

            foreach (var implementation in implementations)
            {
                logger?.LogInformation("Found implementation {name}.", implementation.Name);

                var asm = AppDomain.CurrentDomain.GetAssemblies();
                var result = assemblyLoader.LoadFileAndTest(implementation.FullName, out var exception1);
                if (!result)
                {
                    logger?.LogError("Implementation {name} is not compatible with the current game!", implementation.Name);
                    continue;
                }

                var assembly = Assembly.ReflectionOnlyLoadFrom(implementation.FullName);

                var metadataList = assembly.GetCustomAttributesData();
                var implementationGameVersionStr = (string?) metadataList.FirstOrDefault(x => x.ConstructorArguments.Count == 2 && (string?) x.ConstructorArguments[0].Value == "GameVersion")?.ConstructorArguments[1].Value;
                if (string.IsNullOrEmpty(implementationGameVersionStr))
                {
                    logger?.LogError("Implementation {name} is missing GameVersion AssemblyMetadataAttribute!", implementation.Name);
                    continue;
                }

                if (!ApplicationVersionUtils.TryParse(implementationGameVersionStr, out var implementationGameVersion))
                {
                    logger?.LogError("Implementation {name} has invalid GameVersion AssemblyMetadataAttribute!", implementation.Name);
                    continue;
                }

                yield return (implementation, implementationGameVersion);
            }
        }

        private static IEnumerable<(FileInfo Implementation, ApplicationVersion Version)> ImplementationForGameVersion(ApplicationVersion gameVersion, IEnumerable<(FileInfo Implementation, ApplicationVersion Verion)> implementations)
        {
            foreach (var (implementation, version) in implementations)
            {
                if (version.Revision == -1) // Implementation does not specify the revision
                {
                    if (gameVersion.IsSameWithoutRevision(version))
                    {
                        yield return (implementation, version);
                    }
                }
                else // Implementation specified the revision
                {
                    if (gameVersion.IsSameWithRevision(version))
                    {
                        yield return (implementation, version);
                    }
                }
            }
        }
        private static (FileInfo Implementation, ApplicationVersion Version) ImplementationLatest(IEnumerable<(FileInfo Implementation, ApplicationVersion Version)> implementations)
        {
            return implementations.MaxBy(x => x.Version);
        }

        private ILogger _logger = default!;

        protected override void OnSubModuleLoad()
        {
            _logger = ButterLibSubModule.Instance.GetTempServiceProvider().GetRequiredService<ILogger<ImplementationLoaderSubModule>>();

            var t1 = LoadAllImplementations("MCMv4.UI.*.dll", _logger).Select(x => new MBSubModuleBaseWrapper(x)).ToList();
            var t2 = LoadAllImplementations("MCMv4.Implementation.*.dll", _logger).Select(x => new MBSubModuleBaseWrapper(x)).ToList();
            SubModules.AddRange(t1.Concat(t2));

            base.OnSubModuleLoad();
        }
    }
}
*/