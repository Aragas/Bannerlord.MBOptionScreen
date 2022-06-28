using MCM.Abstractions.Base;

namespace MCM.Abstractions
{
    /// <summary>
    /// Interface that declares that the <see cref="ISettingsContainer"/> can override its settings
    /// </summary>
    public interface ISettingsContainerCanOverride
    {
        bool OverrideSettings(BaseSettings settings);
    }
}