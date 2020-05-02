using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

using Path = System.IO.Path;

namespace MCM
{
    //
    /// <summary>
    /// Will search for any MCM.Implementation assembly and use it for loading if the standalone module was didn't load before
    /// </summary>
    public class IntegratedLoaderSubModule : MBSubModuleBase
    {
        private readonly List<Assembly> _mcmReferencingAssemblies = new List<Assembly>();
        private readonly List<Assembly> _mcmImplementationAssemblies = new List<Assembly>();
        private readonly List<MBSubModuleBase> _mcmImplementationSubModules = new List<MBSubModuleBase>();

        public IntegratedLoaderSubModule()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();

            // Loading as Standalone
            foreach (var assembly in assemblies)
            {
                if (Path.GetFileNameWithoutExtension(assembly.Location) == "MCM")
                    _mcmReferencingAssemblies.Add(assembly);
            }
            // Loading as Integrated
            foreach (var assembly in assemblies)
            {
                var referencedAssemblies = assembly.GetReferencedAssemblies();
                if (!referencedAssemblies.Any(r => r.Name.StartsWith("MCM")))
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
                var matches = assemblyDirectory.GetFiles("MCM.Implementation.v*.dll")
                    .Concat(assemblyDirectory.GetFiles("MCM.UI.v*.dll"))
                    // Might be useful later
                    .Concat(assemblyDirectory.GetFiles("MCM.Custom.*.dll"))
                    // ModLib is required at a more early stage, needs SubModule.xml definition entry
                    //.Concat(assemblyDirectory.GetFiles("ModLib.dll"))
                    .ToList();
                if (!matches.Any())
                    continue;

                foreach (var match in matches.Where(m => assemblies.All(a => Path.GetFileNameWithoutExtension(a.Location) != Path.GetFileNameWithoutExtension(m.Name))))
                    _mcmImplementationAssemblies.Add(Assembly.LoadFrom(match.FullName));
            }

            foreach (var subModuleType in _mcmImplementationAssemblies.SelectMany(assembly => assembly.GetTypes().Where(t => typeof(MBSubModuleBase).IsAssignableFrom(t))))
            {
                if (subModuleType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, Type.EmptyTypes, null)?.Invoke(Array.Empty<object>()) is MBSubModuleBase subModule)
                    _mcmImplementationSubModules.Add(subModule);
            }
        }

        protected override void OnSubModuleLoad()
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.GetType().GetMethod("OnSubModuleLoad", AccessTools.All)?.Invoke(subModule, Array.Empty<object>());
        }
        protected override void OnSubModuleUnloaded()
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.GetType().GetMethod("OnSubModuleUnloaded", AccessTools.All)?.Invoke(subModule, Array.Empty<object>());
        }
        protected override void OnApplicationTick(float dt)
        {
            // TODO:
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.GetType().GetMethod("OnApplicationTick", AccessTools.All)?.Invoke(subModule, new object[] { dt });
        }
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.GetType().GetMethod("OnBeforeInitialModuleScreenSetAsRoot", AccessTools.All)?.Invoke(subModule, Array.Empty<object>());
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.GetType().GetMethod("OnGameStart", AccessTools.All)?.Invoke(subModule, new object[] { game, gameStarterObject });
        }

        public override bool DoLoading(Game game)
        {
            return _mcmImplementationSubModules.All(subModule => subModule.DoLoading(game));

        }
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.OnGameLoaded(game, initializerObject);
        }
        public override void OnCampaignStart(Game game, object starterObject)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.OnCampaignStart(game, starterObject);
        }
        public override void BeginGameStart(Game game)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.BeginGameStart(game);
        }
        public override void OnGameEnd(Game game)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.OnGameEnd(game);
        }
        public override void OnGameInitializationFinished(Game game)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.OnGameInitializationFinished(game);
        }
        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.OnMissionBehaviourInitialize(mission);
        }
        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.OnMultiplayerGameStart(game, starterObject);
        }
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            foreach (var subModule in _mcmImplementationSubModules)
                subModule.OnNewGameCreated(game, initializerObject);
        }
    }
}