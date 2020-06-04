using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;

namespace MCM.Custom.ScreenTests
{
    internal sealed class TestSettingsV3 : BaseTestGlobalSettings<TestSettingsV3>
    {
        public static string Format(int value, TestSettingsV3 settings)
        {
            settings.Test_Opion2 = 1;
            return "";
        }

        public override string Id => "Testing_v3";
        public override string DisplayName => "Testing v3 API";

        [SettingPropertyInteger("Option 1", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1")]
        public int Test_Opion1 { get; set; } = 0;

        [SettingPropertyInteger("Option 2", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1/Level2")]
        public int Test_Opion2 { get; set; } = 0;

        [SettingPropertyInteger("Option 3", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1/Level2/Level3")]
        public int Test_Opion3 { get; set; } = 0;

        [SettingPropertyInteger("Option 4", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1/Level2/Level3/Level4")]
        public int Test_Opion4 { get; set; } = 0;
        [SettingPropertyInteger("Option 5", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1/Level2/Level3/Level4/Level5")]
        public int Test_Opion5 { get; set; } = 0;
    }
}