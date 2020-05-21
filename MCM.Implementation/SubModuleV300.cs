using MCM.Abstractions.FluentBuilder.Implementation;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Providers;
using MCM.Abstractions.Synchronization;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MCM.Implementation
{
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        private bool _boolValue;
        private int _intValue;
        private float _floatValue;
        private string _stringValue = "";
        
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
#if DEBUG
                var builder = new DefaultSettingsBuilder("test_v1", "Test Fluent Settings")
                    .SetFormat("xml")
                    .SetFolderName("")
                    .SetSubFolder("")
                    .CreateGroup("Testing 1", groupBuilder => groupBuilder
                        .AddBool("Check Box", new ProxyRef<bool>(() => _boolValue, o => _boolValue = o), boolBuilder => boolBuilder
                            .SetHintText("Test")))
                    .CreateGroup("Testing 2", groupBuilder => groupBuilder
                        .AddInteger("Integer", 0, 10, new ProxyRef<int>(() => _intValue, o => _intValue = o), integerBuilder => integerBuilder
                            .SetHintText("Testing"))
                        .AddFloatingInteger("Floating Integer", 0, 10, new ProxyRef<float>(() => _floatValue, o => _floatValue = o), floatingBuilder => floatingBuilder
                            .SetRequireRestart(true)
                            .SetHintText("Test")))
                    .CreateGroup("Testing 3", groupBuilder => groupBuilder
                        .AddText("Test", new ProxyRef<string>(() => _stringValue, o => _stringValue = o), null));

                var globalSettings = builder.BuildAsGlobal();
                globalSettings.Register();
                globalSettings.Unregister();

                var perCharacterSettings = builder.BuildAsPerCharacter();
                perCharacterSettings.Register();
                perCharacterSettings.Unregister();
#endif
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