using MCM.Abstractions.Settings.Containers.Global;

namespace MCM.Implementation.ModLib.Settings.Containers.v1
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IModLibSettingsContainer : IGlobalSettingsContainer { }
}