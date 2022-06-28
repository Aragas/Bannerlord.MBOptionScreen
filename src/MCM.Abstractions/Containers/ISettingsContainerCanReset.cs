using MCM.Abstractions.Base;

namespace MCM.Abstractions
{
    /// <summary>
    /// Interface that declares that the <see cref="ISettingsContainer"/> can reset its settings
    /// </summary>
    public interface ISettingsContainerCanReset
    {
        bool ResetSettings(BaseSettings settings);
    }
}