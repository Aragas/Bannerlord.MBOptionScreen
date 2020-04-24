using MBOptionScreen.Synchronization;
using MBOptionScreen.Utils;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MBOptionScreen
{
    public sealed class MBOptionScreenSubModule : MBSubModuleBase
    {
        private ApplicationVersion GameVersion { get; }
        private IMBOptionScreenInitializer MBOptionScreenInitializer { get; } = default!;

        public MBOptionScreenSubModule()
        {
            GameVersion = ApplicationVersionUtils.GameVersion();
            MBOptionScreenInitializer = DI.GetImplementation<IMBOptionScreenInitializer, MBOptionScreenInitializerWrapper>(GameVersion);
        }

        /// <summary>
        /// Start initialization
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            using var synchronizationProvider = DI.GetImplementation<ISynchronizationProvider, SynchronizationProviderWrapper>(GameVersion, new object[] { "OnSubModuleLoad" });
            MBOptionScreenInitializer.StartInitialization(GameVersion, synchronizationProvider.IsFirstInitialization);
        }

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            using var synchronizationProvider = DI.GetImplementation<ISynchronizationProvider, SynchronizationProviderWrapper>(GameVersion, new object[] { "OnBeforeInitialModuleScreenSetAsRoot" });
            MBOptionScreenInitializer.EndInitialization(synchronizationProvider.IsFirstInitialization);
        }
    }
}