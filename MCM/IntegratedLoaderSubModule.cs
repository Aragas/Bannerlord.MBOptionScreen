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
    /// <summary>
    /// Will search for any MCM.Implementation assembly and use it for loading if the standalone module was didn't load before
    /// </summary>
    public class IntegratedLoaderSubModule : MBSubModuleBase
    {
        private readonly List<Assembly> _mcmReferencingAssemblies = new List<Assembly>();
        private readonly List<Assembly> _mcmImplementationAssemblies = new List<Assembly>();
        private readonly List<(MBSubModuleBase, Type)> _mcmImplementationSubModules = new List<(MBSubModuleBase, Type)>();
        private readonly Dictionary<Type, Dictionary<string, MethodInfo?>> _reflectionCache = new Dictionary<Type, Dictionary<string, MethodInfo?>>();
        private readonly object[] _emptyParams = Array.Empty<object>();
        private readonly object[] _dtParams = new object[1];

        public IntegratedLoaderSubModule()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .ToList();

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
                    .Concat(assemblyDirectory.GetFiles("MCMv3.Implementation.*.dll"))
                    .Concat(assemblyDirectory.GetFiles("MCMv3.UI.v*.dll"))
                    // Might be useful later
                    .Concat(assemblyDirectory.GetFiles("MCMv3.Custom.*.dll"))
                    // ModLib is required at a more early stage, needs SubModule.xml definition entry
                    //.Concat(assemblyDirectory.GetFiles("ModLib.dll"))
                    .ToList();
                if (!matches.Any())
                    continue;

                foreach (var match in matches.Where(m => assemblies.All(a => Path.GetFileNameWithoutExtension(a.Location) != Path.GetFileNameWithoutExtension(m.Name))))
                    _mcmImplementationAssemblies.Add(Assembly.LoadFrom(match.FullName));
            }

            var submodules = _mcmImplementationAssemblies.SelectMany(assembly => assembly.GetTypes().Where(t =>
                t.FullName != typeof(IntegratedLoaderSubModule).FullName && typeof(MBSubModuleBase).IsAssignableFrom(t)));
            foreach (var subModuleType in submodules)
            {
                if (subModuleType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, Type.EmptyTypes, null)?.Invoke(Array.Empty<object>()) is MBSubModuleBase subModule)
                    _mcmImplementationSubModules.Add((subModule, subModuleType));
            }

            foreach (var (_, subModuleType) in _mcmImplementationSubModules)
            {
                if (!_reflectionCache.ContainsKey(subModuleType))
                    _reflectionCache.Add(subModuleType, new Dictionary<string, MethodInfo?>());

                _reflectionCache[subModuleType]["OnSubModuleLoad"] = AccessTools.Method(subModuleType, "OnSubModuleLoad");
                _reflectionCache[subModuleType]["OnSubModuleUnloaded"] = AccessTools.Method(subModuleType, "OnSubModuleUnloaded");
                _reflectionCache[subModuleType]["OnApplicationTick"] = AccessTools.Method(subModuleType, "OnApplicationTick");
                _reflectionCache[subModuleType]["OnBeforeInitialModuleScreenSetAsRoot"] = AccessTools.Method(subModuleType, "OnBeforeInitialModuleScreenSetAsRoot");
                _reflectionCache[subModuleType]["OnGameStart"] = AccessTools.Method(subModuleType, "OnGameStart");
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            foreach (var (subModule, subModuleType) in _mcmImplementationSubModules)
                _reflectionCache[subModuleType]["OnSubModuleLoad"]?.Invoke(subModule, _emptyParams);
        }
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            foreach (var (subModule, subModuleType) in _mcmImplementationSubModules)
                _reflectionCache[subModuleType]["OnSubModuleUnloaded"]?.Invoke(subModule, _emptyParams);
        }
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            _dtParams[0] = dt;
            foreach (var (subModule, subModuleType) in _mcmImplementationSubModules)
                _reflectionCache[subModuleType]["OnApplicationTick"]?.Invoke(subModule, _dtParams);
        }
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            foreach (var (subModule, subModuleType) in _mcmImplementationSubModules)
                _reflectionCache[subModuleType]["OnBeforeInitialModuleScreenSetAsRoot"]?.Invoke(subModule, _emptyParams);
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            var @params = new object[] { game, gameStarterObject };
            foreach (var (subModule, subModuleType) in _mcmImplementationSubModules)
                _reflectionCache[subModuleType]["OnGameStart"]?.Invoke(subModule, @params);
        }

        public override bool DoLoading(Game game)
        {
            return base.DoLoading(game) && _mcmImplementationSubModules.All(tuple => tuple.Item1.DoLoading(game));

        }
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);

            foreach (var (subModule, _) in _mcmImplementationSubModules)
                subModule.OnGameLoaded(game, initializerObject);
        }
        public override void OnCampaignStart(Game game, object starterObject)
        {
            base.OnCampaignStart(game, starterObject);

            foreach (var (subModule, _) in _mcmImplementationSubModules)
                subModule.OnCampaignStart(game, starterObject);
        }
        public override void BeginGameStart(Game game)
        {
            base.BeginGameStart(game);


            foreach (var (subModule, _) in _mcmImplementationSubModules)
                subModule.BeginGameStart(game);
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            
            foreach (var (subModule, _) in _mcmImplementationSubModules)
                subModule.OnGameEnd(game);
        }
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            foreach (var (subModule, _) in _mcmImplementationSubModules)
                subModule.OnGameInitializationFinished(game);
        }
        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);

            foreach (var (subModule, _) in _mcmImplementationSubModules)
                subModule.OnMissionBehaviourInitialize(mission);
        }
        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);

            foreach (var (subModule, _) in _mcmImplementationSubModules)
                subModule.OnMultiplayerGameStart(game, starterObject);
        }
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            base.OnNewGameCreated(game, initializerObject);
            
            foreach (var (subModule, _) in _mcmImplementationSubModules)
                subModule.OnNewGameCreated(game, initializerObject);
        }
    }
}