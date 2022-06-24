using MCM.Abstractions.Settings.Base;

namespace MCM.Abstractions.Settings.Containers
{
    /// <summary>
    /// Interface that declares that the <see cref="ISettingsContainer"/> can reset its settings
    /// </summary>
    public interface ISettingsContainerCanReset
    {
        bool ResetSettings(BaseSettings settings);
    }
}