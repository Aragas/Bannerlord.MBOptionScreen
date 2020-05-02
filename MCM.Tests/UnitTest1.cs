using System;
using System.IO;
using SandBox;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using Xunit;

namespace MCM.Tests
{
    public class IntegratedLoaderSubModuleTests
    {
        [Fact]
        public void LoadsExternalAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assert.DoesNotContain(assemblies, a => !a.IsDynamic && !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MCM.Implementation"));
            Assert.DoesNotContain(assemblies, a => !a.IsDynamic && !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MCM.UI"));
            var subModule = new IntegratedLoaderSubModule();
            Assert.Contains(assemblies, a => !a.IsDynamic && Path.GetFileNameWithoutExtension(a.Location).StartsWith("MCM.Implementation"));
            Assert.Contains(assemblies, a => !a.IsDynamic && Path.GetFileNameWithoutExtension(a.Location).StartsWith("MCM.UI"));
        }

        [Fact]
        public void OverridesTests()
        {
            var game = Game.CreateGame(new Campaign(CampaignGameMode.Campaign), new CampaignGameManager(0));
            var starter = new BasicGameStarter();

            var subModule = new IntegratedLoaderSubModule();
            subModule.GetType().GetMethod("OnSubModuleLoad")?.Invoke(subModule, Array.Empty<object>());
            subModule.GetType().GetMethod("OnSubModuleUnloaded")?.Invoke(subModule, Array.Empty<object>());
            subModule.GetType().GetMethod("OnApplicationTick")?.Invoke(subModule, new object[] { 0.1f });
            subModule.GetType().GetMethod("OnBeforeInitialModuleScreenSetAsRoot")?.Invoke(subModule, Array.Empty<object>());
            subModule.GetType().GetMethod("OnGameStart")?.Invoke(subModule, new object[] { game, starter });
            subModule.OnNewGameCreated(game, starter);
            subModule.OnMultiplayerGameStart(game, starter);
            subModule.OnCampaignStart(game, starter);
            subModule.OnGameInitializationFinished(game);
            subModule.OnMissionBehaviourInitialize(null);
            subModule.OnGameInitializationFinished(game);
            subModule.OnGameEnd(game);
            subModule.BeginGameStart(game);
            subModule.DoLoading(game);
        }
    }
    public class VersionResolverTests
    {
        [Fact]
        public void Test1()
        {

        }
    }
    public class DITests
    {
        [Fact]
        public void Test1()
        {

        }
    }
}