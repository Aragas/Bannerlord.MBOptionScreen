using System;

namespace MBOptionScreen.Interfaces
{
    /// <summary>
    /// A shareable object between multiple mods that will use this library
    /// Life length expectation - OnSubModuleLoad()->OnBeforeInitialModuleScreenSetAsRoot()
    /// </summary>
    public interface ISharedStateObject
    {
        bool HasInitialized { get; set; }

        ISettingsProvider SettingsStorage { get; set; }
        IResourceInjector ResourceInjector { get; set; }
        Type ModOptionScreen { get; set; }
    }
}