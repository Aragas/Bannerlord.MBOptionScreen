using MCM.Abstractions.Settings;

namespace MCM.Implementation
{
    public abstract class TestSettingsBase<T> : AttributeGlobalSettings<T> where T : GlobalSettings, new()
    {
        public override string FolderName => "Testing";
    }
}