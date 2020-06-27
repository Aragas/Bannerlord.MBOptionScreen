using MCM.Abstractions.Settings.Containers.PerCharacter;

namespace MCM.Implementation.Settings.Containers.PerCharacter
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class MCMPerCharacterSettingsContainerWrapper : BasePerCharacterSettingsContainerWrapper, IMCMPerCharacterSettingsContainer
    {
        public MCMPerCharacterSettingsContainerWrapper(object @object) : base(@object) { }
    }
}