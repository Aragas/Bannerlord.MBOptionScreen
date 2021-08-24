using MCM.Abstractions.FluentBuilder.Implementation;
using MCM.Abstractions.Ref;

using TaleWorlds.MountAndBlade;

namespace MCMv3.Tests
{
    public sealed class SubModule : MBSubModuleBase
    {
        private bool _boolValue;
        private int _intValue;
        private float _floatValue;
        private string _stringValue = string.Empty;

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            var builder = new DefaultSettingsBuilder("Testing_Global_v3", "MCMv3 Testing Fluent Settings")!
                .SetFormat("xml")
                .SetFolderName(string.Empty)
                .SetSubFolder(string.Empty)
                .CreateGroup("Testing 1", groupBuilder => groupBuilder
                    .AddBool("prop_1", "Check Box", new ProxyRef<bool>(() => _boolValue, o => _boolValue = o), boolBuilder => boolBuilder
                        .SetHintText("Test")
                        .SetRequireRestart(false)))
                .CreateGroup("Testing 2", groupBuilder => groupBuilder
                    .AddInteger("prop_2", "Integer", 0, 10, new ProxyRef<int>(() => _intValue, o => _intValue = o), integerBuilder => integerBuilder
                         .SetHintText("Testing"))
                    .AddFloatingInteger("prop_3", "Floating Integer", 0, 10, new ProxyRef<float>(() => _floatValue, o => _floatValue = o), floatingBuilder => floatingBuilder
                         .SetRequireRestart(true)
                         .SetHintText("Test")))
                .CreateGroup("Testing 3", groupBuilder => groupBuilder
                    .AddText("prop_4", "Test", new ProxyRef<string>(() => _stringValue, o => _stringValue = o), null));

            var globalSettings = builder.BuildAsGlobal();
            globalSettings.Register();
            //globalSettings.Unregister();

            //var PerSaveSettings = builder.BuildAsPerSave();
            //PerSaveSettings.Register();
            //PerSaveSettings.Unregister();
        }
    }
}