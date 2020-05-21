#if DEBUG
using MCM.Abstractions.Settings.Base.PerCharacter;

namespace MCM.Implementation.Testing
{
    public abstract class BaseTestPerCharacterSettingsBase<T> : AttributePerCharacterSettings<T> where T : PerCharacterSettings, new()
    {
        public override string FolderName => "Testing";
    }
}
#endif