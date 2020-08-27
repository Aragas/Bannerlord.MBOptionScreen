/*
using MCM.Implementation;
using MCM.UI;

using NUnit.Framework;

using System;
using System.Runtime.Serialization;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MCM.Tests
{
    public class IntegratedLoaderSubModuleTests
    {
        private bool @bool;

        [SetUp]
        public void Setup()
        {
            // Force load Implementation assembly
            // Avoiding compiler optimization))0)
            @bool = typeof(MCMImplementationSubModule) != null && typeof(MCMUISubModule) != null;
        }

        [Test]
        public void LoadsExternalAssemblies()
        {
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //Assert.DoesNotContain(assemblies, a => !a.IsDynamic && !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MCM.Implementation"));
            //Assert.DoesNotContain(assemblies, a => !a.IsDynamic && !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MCM.UI"));
            //var subModule = new IntegratedLoaderSubModule();
            //Assert.Contains(assemblies, a => !a.IsDynamic && Path.GetFileNameWithoutExtension(a.Location).StartsWith("MCM.Implementation"));
            //Assert.Contains(assemblies, a => !a.IsDynamic && Path.GetFileNameWithoutExtension(a.Location).StartsWith("MCM.UI"));
        }

        [Test]
        public void OverridesTests()
        {
            var game = (Game) FormatterServices.GetUninitializedObject(typeof(Game));

            var starter = (BasicGameStarter) FormatterServices.GetUninitializedObject(typeof(BasicGameStarter));

            var mission = (Mission) FormatterServices.GetUninitializedObject(typeof(Mission));

            var subModule = new ImplementationLoaderSubModule();
            subModule.GetType().GetMethod("OnSubModuleLoad")?.Invoke(subModule, Array.Empty<object>());
            subModule.GetType().GetMethod("OnSubModuleUnloaded")?.Invoke(subModule, Array.Empty<object>());
            subModule.GetType().GetMethod("OnApplicationTick")?.Invoke(subModule, new object[] { 0.1f });
            subModule.GetType().GetMethod("OnBeforeInitialModuleScreenSetAsRoot")?.Invoke(subModule, Array.Empty<object>());
            subModule.GetType().GetMethod("OnGameStart")?.Invoke(subModule, new object[] { game, starter });
            subModule.OnNewGameCreated(game, starter);
            subModule.OnMultiplayerGameStart(game, starter);
            subModule.OnCampaignStart(game, starter);
            subModule.OnGameInitializationFinished(game);
            subModule.OnMissionBehaviourInitialize(mission);
            subModule.OnGameInitializationFinished(game);
            subModule.OnGameEnd(game);
            subModule.BeginGameStart(game);
            subModule.DoLoading(game);

            Assert.True(@bool);
        }
    }
}
*/