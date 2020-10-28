using MCM.Abstractions.Settings.Base.PerSave;

namespace MCMv4.Tests
{
    public abstract class BaseTestPerSaveSettingsBase<T> : AttributePerSaveSettings<T> where T : PerSaveSettings, new()
    {
        public override string FolderName => "Testing";
    }
}