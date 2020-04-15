using MBOptionScreen.Interfaces;

using System;

namespace MBOptionScreen.State
{
    internal class SharedStateObject : ISharedStateObject
    {
        public bool HasInitialized { get; internal set; }

        public IFileStorage FileStorage { get; internal set; }
        public ISettingsStorage SettingsStorage { get; internal set; }
        public IResourceInjector ResourceInjector { get; internal set; }
        public Type ModOptionScreen { get; internal set; }

        public SharedStateObject(IFileStorage fileStorage, ISettingsStorage settingsStorage, IResourceInjector resourceInjector, Type modOptionScreen)
        {
            FileStorage = fileStorage;
            SettingsStorage = settingsStorage;
            ResourceInjector = resourceInjector;
            ModOptionScreen = modOptionScreen;
        }
    }
}
