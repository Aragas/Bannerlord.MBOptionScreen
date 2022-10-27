using MCM.Abstractions.Base.PerSave;

namespace MCMv5.Tests
{
    public abstract class BaseTestPerSaveSettingsBase<T> : AttributePerSaveSettings<T> where T : PerSaveSettings, new()
    {
        public override string FolderName => "MCMv5.Tests";
    }
}