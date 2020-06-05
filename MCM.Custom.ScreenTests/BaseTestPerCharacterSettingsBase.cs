using MCM.Abstractions.Settings.Base.PerCharacter;

namespace MCM.Custom.ScreenTests
{
    public abstract class BaseTestPerCharacterSettingsBase<T> : AttributePerCharacterSettings<T> where T : PerCharacterSettings, new()
    {
        public override string FolderName => "Testing";
    }
}