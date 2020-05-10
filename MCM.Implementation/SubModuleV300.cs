using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Abstractions.Synchronization;
using MCM.Utils;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MCM.Implementation
{
    public sealed class SubModuleV300 : MBSubModuleBase
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
            base.OnGameInitializationFinished(game);
            BaseSettingsProvider.Instance.OnGameStarted(game);
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            BaseSettingsProvider.Instance.OnGameEnded(game);

        }
    }
}