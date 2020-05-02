using MCM.Abstractions.Settings;

namespace MCM.Implementation
{
    public abstract class TestSettingsBase<T> : AttributeSettingsBase<T> where T : SettingsBase, new()
    {
        public override string ModuleFolderName => "Testing";
    }
}