using MCM.Abstractions.Settings.SettingsContainer;

namespace MCM.Implementation.Settings.SettingsContainer
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class MCMPerCharacterSettingsContainerWrapper : BasePerCharacterSettingsContainerWrapper, IMCMPerCharacterSettingsContainer
    {
        public MCMPerCharacterSettingsContainerWrapper(object @object) : base(@object) { }
    }
}