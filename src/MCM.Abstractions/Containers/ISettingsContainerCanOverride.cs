using MCM.Abstractions.Settings.Base;

namespace MCM.Abstractions.Settings.Containers
{
    /// <summary>
    /// Interface that declares that the <see cref="ISettingsContainer"/> can override its settings
    /// </summary>
    public interface ISettingsContainerCanOverride
    {
        bool OverrideSettings(BaseSettings settings);
    }
}