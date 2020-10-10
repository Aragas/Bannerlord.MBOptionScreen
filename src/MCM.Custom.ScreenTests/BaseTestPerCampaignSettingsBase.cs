using MCM.Abstractions.Settings.Base.PerSave;

namespace MCM.Custom.ScreenTests
{
    public abstract class BaseTestPerSaveSettingsBase<T> : AttributePerSaveSettings<T> where T : PerSaveSettings, new()
    {
        public override string FolderName => "Testing";
    }
}