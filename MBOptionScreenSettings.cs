using MBOptionScreen.Settings;

namespace MBOptionScreen
{
    internal class MBOptionScreenSettings : AttributeSettings<MBOptionScreenSettings>
    {
        public override string Id { get; set; } = "OptionScreen_v1";
        public override string ModName => $"OptionScreen v1";
        public override string ModuleFolderName => "OptionScreen";
    }
}