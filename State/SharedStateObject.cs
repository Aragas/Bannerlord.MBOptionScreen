using MBOptionScreen.Interfaces;

using System;

namespace MBOptionScreen.State
{
    internal class SharedStateObject : ISharedStateObject
    {
        public bool HasInitialized { get; internal set; }

        public ISettingsProvider SettingsStorage { get; internal set; }
        public IResourceInjector ResourceInjector { get; internal set; }
        public Type ModOptionScreen { get; internal set; }

        public SharedStateObject(ISettingsProvider settingsStorage, IResourceInjector resourceInjector, Type modOptionScreen)
        {
            SettingsStorage = settingsStorage;
            ResourceInjector = resourceInjector;
            ModOptionScreen = modOptionScreen;
        }
    }
}
