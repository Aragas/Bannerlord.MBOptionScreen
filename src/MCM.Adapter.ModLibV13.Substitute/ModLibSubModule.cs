using TaleWorlds.MountAndBlade;

namespace ModLib.Definitions
{
    public class ModLibSubModule : MBSubModuleBase
    {
        public static string ModuleFolderName { get; } = "ModLib";

        private bool _isInitialized;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (!_isInitialized)
            {
                SettingsDatabase.LoadAllSettings();

                _isInitialized = true;
            }
        }
    }
}