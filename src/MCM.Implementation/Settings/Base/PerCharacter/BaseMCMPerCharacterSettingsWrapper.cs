using MCM.Abstractions.Settings.Base.PerCharacter;

namespace MCM.Implementation.Settings.Base.PerCharacter
{
    public abstract class BaseMCMPerCharacterSettingsWrapper : BasePerCharacterSettingsWrapper
    {
        protected BaseMCMPerCharacterSettingsWrapper(object @object) : base(@object) { }
    }
}