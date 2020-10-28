using ModLib;

using TaleWorlds.MountAndBlade;

namespace ModLibV1.Tests
{
    public sealed class SubModule : MBSubModuleBase
    {
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            SettingsDatabase.RegisterSettings(new TestSettings());
        }
    }
}