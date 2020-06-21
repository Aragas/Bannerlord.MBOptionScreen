using MCM.Abstractions.Settings.Providers;
using MCM.Abstractions.Synchronization;

using System.Runtime.CompilerServices;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

[assembly: InternalsVisibleTo("MCM.Custom.ScreenTests")]
[assembly: InternalsVisibleTo("MCM.Tests")]

namespace MCM.Implementation
{
    public sealed class MCMImplementationSubModule : MBSubModuleBase
    {
        /// <summary>
        /// Start initialization
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnSubModuleLoad_MCMv3");
            if (synchronizationProvider.IsFirstInitialization)
            {

            }
        }

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnBeforeInitialModuleScreenSetAsRoot_MCMv3");
            if (synchronizationProvider.IsFirstInitialization)
            {

            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);
            BaseSettingsProvider.Instance.OnGameStarted(game);
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            BaseSettingsProvider.Instance.OnGameEnded(game);
        }
    }
}