using MCM.Abstractions.Settings.Containers.PerSave;

namespace MCM.Implementation.Settings.Containers.PerSave
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMCMPerSaveSettingsContainer : IPerSaveSettingsContainer { }
}