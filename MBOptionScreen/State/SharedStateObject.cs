using MBOptionScreen.Interfaces;

using System;

namespace MBOptionScreen.State
{
    public class SharedStateObject : ISharedStateObject
    {
        public bool HasInitialized { get; set; }

        public ISettingsProvider SettingsStorage { get; set; }
        public IResourceInjector ResourceInjector { get; set; }
        public Type ModOptionScreen { get; set; }

        public SharedStateObject(ISettingsProvider settingsStorage, IResourceInjector resourceInjector, Type modOptionScreen)
        {
            SettingsStorage = settingsStorage;
            ResourceInjector = resourceInjector;
            ModOptionScreen = modOptionScreen;
        }
    }
}
