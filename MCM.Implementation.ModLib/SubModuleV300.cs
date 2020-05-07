using MCM.Abstractions.Synchronization;
using MCM.Implementation.ModLib.Functionality;
using MCM.Utils;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.ModLib
{
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        private ApplicationVersion GameVersion { get; }

        public SubModuleV300()
        {
            GameVersion = ApplicationVersionUtils.GameVersion();
        }

        /// <summary>
        /// Start initialization
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            using var synchronizationProvider = DI.GetImplementation<ISynchronizationProvider, SynchronizationProviderWrapper>(GameVersion, new object[] { "OnSubModuleLoad_ModLibv3" })!;
            if (synchronizationProvider.IsFirstInitialization)
            {

            }
        }

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            using var synchronizationProvider = DI.GetImplementation<ISynchronizationProvider, SynchronizationProviderWrapper>(GameVersion, new object[] { "OnBeforeInitialModuleScreenSetAsRoot_ModLibv3" })!;
            if (synchronizationProvider.IsFirstInitialization)
            {
                if (MCMSettings.Instance!.OverrideModLib)
                    BaseModLibScreenOverrider.Instance.OverrideModLibScreen();
            }
        }
    }
}