using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v2;
using MBOptionScreen.Settings;

namespace MBOptionScreen
{
    internal sealed class MBOptionScreenSettings : AttributeSettings<MBOptionScreenSettings>
    {
        public override string Id { get; set; } = "MBOptionScreen_v2";
        public override string ModName => $"MBOptionScreen {typeof(MBOptionScreenSettings).Assembly.GetName().Version.ToString(3)}";
        public override string ModuleFolderName => "";


        [SettingPropertyBool("Override ModLib Option Screen", RequireRestart = true)]
        [SettingPropertyGroup("General")]
        public bool OverrideModLib { get; set; } = true;
    }
}