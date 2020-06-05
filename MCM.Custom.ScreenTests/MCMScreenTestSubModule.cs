using MCM.Abstractions.FluentBuilder.Implementation;
using MCM.Abstractions.Ref;

using TaleWorlds.MountAndBlade;

namespace MCM.Custom.ScreenTests
{
    public sealed class MCMScreenTestSubModule : MBSubModuleBase
    {
#if DEBUG
        private bool _boolValue;
        private int _intValue;
        private float _floatValue;
        private string _stringValue = "";
#endif

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
#if DEBUG
            var builder = new DefaultSettingsBuilder("Testing_Global_v1", "Testing Fluent Settings")
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
            //globalSettings.Unregister();

            //var perCharacterSettings = builder.BuildAsPerCharacter();
            //perCharacterSettings.Register();
            //perCharacterSettings.Unregister();
#endif
        }
    }
}