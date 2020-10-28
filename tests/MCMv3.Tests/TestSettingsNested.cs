using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;

namespace MCMv3.Tests
{
    internal sealed class TestSettingsNested : BaseTestGlobalSettings<TestSettingsNested>
    {
        public override string Id => "Testing_Nested_v3";
        public override string DisplayName => "MCMv3 Testing Nested";


        [SettingPropertyInteger("Option 1", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1")]
        public int Test_Option1 { get; set; } = 0;

        [SettingPropertyInteger("Option 2", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1/Level2")]
        public int Test_Option2 { get; set; } = 0;

        [SettingPropertyInteger("Option 3", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1/Level2/Level3")]
        public int Test_Option3 { get; set; } = 0;

        [SettingPropertyInteger("Option 4", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1/Level2/Level3/Level4")]
        public int Test_Option4 { get; set; } = 0;
        [SettingPropertyInteger("Option 5", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("Level1/Level2/Level3/Level4/Level5")]
        public int Test_Option5 { get; set; } = 0;
    }
}