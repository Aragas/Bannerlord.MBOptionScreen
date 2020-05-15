using MCM.Abstractions.Settings;

namespace MCM.Implementation.Settings
{
    public abstract class BaseMCMPerCharacterSettingsWrapper : BasePerCharacterSettingsWrapper
    {
        protected BaseMCMPerCharacterSettingsWrapper(object @object) : base(@object) { }
    }
}