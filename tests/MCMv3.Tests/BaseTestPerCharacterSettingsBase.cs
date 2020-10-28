using MCM.Abstractions.Settings.Base.PerCharacter;

namespace MCMv3.Tests
{
    public abstract class BaseTestPerCharacterSettingsBase<T> : AttributePerCharacterSettings<T> where T : PerCharacterSettings, new()
    {
        public override string FolderName => "Testing";
    }
}