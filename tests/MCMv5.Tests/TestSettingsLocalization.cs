﻿using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Common;

namespace MCMv5.Tests
{
    internal sealed class TestSettingsLocalization : BaseTestGlobalSettings<TestSettingsLocalization>
    {
        public override string Id => "Testing_Localization_v5";
        public override string DisplayName => "MCMv5 Testing Localization";


        [SettingPropertyInteger("Option 1", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("{=Tgabbs}Level1")]
        public int Test_Option1 { get; set; } = 0;

        [SettingPropertyInteger("Option 2", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("{=Tgabbs}Level1")]
        public int Test_Option2 { get; set; } = 0;

        [SettingPropertyInteger("Option 3", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("{=Tgabbs}Level1")]
        public int Test_Option3 { get; set; } = 0;

        [SettingPropertyInteger("Option 4", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("{=Tgafds}Level1/Level2")]
        public int Test_Option4 { get; set; } = 0;

        [SettingPropertyInteger("Option 5", 0, 100, Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("{=Tgafds}Level1/Level2")]
        public int Test_Option5 { get; set; } = 0;

        [SettingPropertyDropdown("Option 6", Order = 0, RequireRestart = false, HintText = "Option 1")]
        [SettingPropertyGroup("{=Tgafds}Level1/Level2")]
        public Dropdown<string> PropertyDropdownSelectedIndex0 { get; set; } = new(new[]
        {
            "{=TgabbF}Value 1",
            "{=TgabbG}Value 2",
            "{=TgabbD}Value 3",
        }, 0);
    }
}