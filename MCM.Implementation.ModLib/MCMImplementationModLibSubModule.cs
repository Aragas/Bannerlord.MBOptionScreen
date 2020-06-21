using MCM.Abstractions.Synchronization;
using MCM.Implementation.ModLib.Functionality;

using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.ModLib
{
    public sealed class MCMImplementationModLibSubModule : MBSubModuleBase
    {
        /// <summary>
        /// Start initialization
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnSubModuleLoad_ModLibv3");
            if (synchronizationProvider.IsFirstInitialization)
            {

            }
        }

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnBeforeInitialModuleScreenSetAsRoot_ModLibv3");
            if (synchronizationProvider.IsFirstInitialization)
            {
                if (MCMModLibSettings.Instance!.OverrideModLib)
                    BaseModLibScreenOverrider.Instance.OverrideModLibScreen();
            }
        }
    }
}