using MCM.Abstractions.Base;

namespace MCM.Abstractions
{
    /// <summary>
    /// Interface that declares that the <see cref="ISettingsContainer"/> can override its settings
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsContainerCanOverride
    {
        bool OverrideSettings(BaseSettings settings);
    }
}