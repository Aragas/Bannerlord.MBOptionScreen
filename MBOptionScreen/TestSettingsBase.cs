using MBOptionScreen.Settings;

namespace MBOptionScreen
{
    public abstract class TestSettingsBase<T> : AttributeSettings<T> where T : SettingsBase, new()
    {
        public override string ModuleFolderName => "Testing";
    }
}