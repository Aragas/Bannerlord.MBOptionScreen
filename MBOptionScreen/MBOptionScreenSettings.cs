using MBOptionScreen.Attributes;
using MBOptionScreen.Settings;

namespace MBOptionScreen
{
    internal class MBOptionScreenSettings : AttributeSettings<MBOptionScreenSettings>
    {
        public override string Id { get; set; } = "OptionScreen_v1";
        public override string ModName => $"MBOptionScreen";
        public override string ModuleFolderName => "OptionScreen";


        [SettingPropertyBool("Override ModLib Option Screen", requireRestart: true, hintText: "")]
        [SettingPropertyGroup("General")]
        public bool OverrideModLib { get; set; } = true;
    }
}