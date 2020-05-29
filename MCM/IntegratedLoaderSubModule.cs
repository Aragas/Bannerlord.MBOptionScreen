#define Tick_

using HarmonyLib;

using MCM.Abstractions.Loader;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MCM
{
    // Find all MCM.Implementation.dll
    // Find the latest ILoader
    // Use it to load the rest
    /// <summary>
    /// Will search for any MCM.Implementation assembly and use it for loading if the standalone module was didn't load before
    /// </summary>
    public class IntegratedLoaderSubModule : MBSubModuleBase
    {
        private static void LoadAllImplementationAssemblies()
        {
            var mcmReferencingAssemblies = new List<Assembly>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();
            foreach (var assembly in assemblies.Where(assembly => assembly.GetName().Name == "MCMv3"))
            {
                mcmReferencingAssemblies.Add(assembly);
            }
            foreach (var assembly in assemblies)
            {
                var referencedAssemblies = assembly.GetReferencedAssemblies();
                if (referencedAssemblies.All(r => r.Name != "MCMv3"))
                    continue;
                mcmReferencingAssemblies.Add(assembly);
            }
            foreach (var assembly in mcmReferencingAssemblies)
            {
                var assemblyFile = new FileInfo(assembly.Location);
                if (!assemblyFile.Exists)
                    continue;

                var assemblyDirectory = assemblyFile.Directory;
                if (assemblyDirectory == null || !assemblyDirectory.Exists)
                    continue;

                var matches = assemblyDirectory.GetFiles("MCMv3.Implementation.*.dll");
                if (!matches.Any())
                    continue;

                foreach (var match in matches.Where(m => assemblies.All(a => Path.GetFileNameWithoutExtension(a.Location) != Path.GetFileNameWithoutExtension(m.Name))))
                    Assembly.LoadFrom(match.FullName);
            }
        }


        private readonly IIntegratedLoader _loader;
        private readonly Dictionary<Type, Dictionary<string, MethodInfo?>> _reflectionCache = new Dictionary<Type, Dictionary<string, MethodInfo?>>();
        private readonly object[] _emptyParams = Array.Empty<object>();
#if Tick
        private readonly object[] _dtParams = new object[1];
#endif

        public IntegratedLoaderSubModule()
        {
            LoadAllImplementationAssemblies();
            _loader = DI.GetImplementation<IIntegratedLoader, IntegratedLoaderWrapper>()!;
            _loader.Load();

            foreach (var (_, subModuleType) in _loader.MCMImplementationSubModules)
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

            foreach (var (subModule, subModuleType) in _loader.MCMImplementationSubModules)
                _reflectionCache[subModuleType]["OnSubModuleLoad"]?.Invoke(subModule, _emptyParams);
        }
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            foreach (var (subModule, subModuleType) in _loader.MCMImplementationSubModules)
                _reflectionCache[subModuleType]["OnSubModuleUnloaded"]?.Invoke(subModule, _emptyParams);
        }
#if Tick
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            _dtParams[0] = dt;
            foreach (var (subModule, subModuleType) in _loader.MCMImplementationSubModules)
                _reflectionCache[subModuleType]["OnApplicationTick"]?.Invoke(subModule, _dtParams);
        }
#endif
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            foreach (var (subModule, subModuleType) in _loader.MCMImplementationSubModules)
                _reflectionCache[subModuleType]["OnBeforeInitialModuleScreenSetAsRoot"]?.Invoke(subModule, _emptyParams);
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            var @params = new object[] { game, gameStarterObject };
            foreach (var (subModule, subModuleType) in _loader.MCMImplementationSubModules)
                _reflectionCache[subModuleType]["OnGameStart"]?.Invoke(subModule, @params);
        }

        public override bool DoLoading(Game game)
        {
            return base.DoLoading(game) && _loader.MCMImplementationSubModules.All(tuple => tuple.Item1.DoLoading(game));

        }
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);

            foreach (var (subModule, _) in _loader.MCMImplementationSubModules)
                subModule.OnGameLoaded(game, initializerObject);
        }
        public override void OnCampaignStart(Game game, object starterObject)
        {
            base.OnCampaignStart(game, starterObject);

            foreach (var (subModule, _) in _loader.MCMImplementationSubModules)
                subModule.OnCampaignStart(game, starterObject);
        }
        public override void BeginGameStart(Game game)
        {
            base.BeginGameStart(game);


            foreach (var (subModule, _) in _loader.MCMImplementationSubModules)
                subModule.BeginGameStart(game);
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            
            foreach (var (subModule, _) in _loader.MCMImplementationSubModules)
                subModule.OnGameEnd(game);
        }
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            foreach (var (subModule, _) in _loader.MCMImplementationSubModules)
                subModule.OnGameInitializationFinished(game);
        }
        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);

            foreach (var (subModule, _) in _loader.MCMImplementationSubModules)
                subModule.OnMissionBehaviourInitialize(mission);
        }
        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);

            foreach (var (subModule, _) in _loader.MCMImplementationSubModules)
                subModule.OnMultiplayerGameStart(game, starterObject);
        }
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            base.OnNewGameCreated(game, initializerObject);
            
            foreach (var (subModule, _) in _loader.MCMImplementationSubModules)
                subModule.OnNewGameCreated(game, initializerObject);
        }
    }
}