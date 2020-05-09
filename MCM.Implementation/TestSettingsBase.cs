using MCM.Abstractions.Settings;

namespace MCM.Implementation
{
    public abstract class TestSettingsBase1<T> : AttributePerCharacterSettings<T> where T : PerCharacterSettings, new()
    {
        public override string FolderName => "Testing";
    }

    public abstract class TestSettingsBase<T> : AttributeGlobalSettings<T> where T : GlobalSettings, new()
    {
        public override string FolderName => "Testing";
    }
}