using TaleWorlds.MountAndBlade;

namespace ModLib.Definitions
{
    public class ModLibSubModule : MBSubModuleBase
    {
        public static string ModuleFolderName { get; } = "ModLib";

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            SettingsDatabase.LoadAllSettings();
        }
    }
}