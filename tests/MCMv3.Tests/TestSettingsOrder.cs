using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;

namespace MCMv3.Tests
{
    internal sealed class TestSettingsOrder : BaseTestGlobalSettings<TestSettingsOrder>
    {
        public override string Id => "Testing_Order_v3";
        public override string DisplayName => "MCMv3 Testing Order";


        [SettingPropertyBool("Correct Order: ACBED")]
        public bool CorrectOrder { get; set; } = true;

        [SettingPropertyBool("A", Order = 1)]
        [SettingPropertyGroup("Property Order")]
        public bool Prop_A { get; set; } = true;

        [SettingPropertyBool("B", Order = 3)]
        [SettingPropertyGroup("Property Order")]
        public bool Prop_B { get; set; } = true;

        [SettingPropertyBool("C", Order = 2)]
        [SettingPropertyGroup("Property Order")]
        public bool Prop_C { get; set; } = true;

        [SettingPropertyBool("D", Order = 5)]
        [SettingPropertyGroup("Property Order")]
        public bool Prop_D { get; set; } = true;

        [SettingPropertyBool("E", Order = 4)]
        [SettingPropertyGroup("Property Order")]
        public bool Prop_E { get; set; } = true;


        [SettingPropertyBool("A")]
        [SettingPropertyGroup("A", GroupOrder = 1)]
        public bool Group_A { get; set; } = true;

        [SettingPropertyBool("B")]
        [SettingPropertyGroup("B", GroupOrder = 3)]
        public bool Group_B { get; set; } = true;

        [SettingPropertyBool("C")]
        [SettingPropertyGroup("C", GroupOrder = 2)]
        public bool Group_C { get; set; } = true;

        [SettingPropertyBool("D")]
        [SettingPropertyGroup("D", GroupOrder = 5)]
        public bool Group_D { get; set; } = true;

        [SettingPropertyBool("E")]
        [SettingPropertyGroup("E", GroupOrder = 4)]
        public bool Group_E { get; set; } = true;


        [SettingPropertyBool("Group Order Toggle")]
        [SettingPropertyGroup("Group Order", IsMainToggle = true)]
        public bool SubGroup_Toggle { get; set; } = true;

        [SettingPropertyBool("A")]
        [SettingPropertyGroup("Group Order/A", GroupOrder = 1)]
        public bool SubGroup_A { get; set; } = true;

        [SettingPropertyBool("B")]
        [SettingPropertyGroup("Group Order/B", GroupOrder = 3)]
        public bool SubGroup_B { get; set; } = true;

        [SettingPropertyBool("C")]
        [SettingPropertyGroup("Group Order/C", GroupOrder = 2)]
        public bool SubGroup_C { get; set; } = true;

        [SettingPropertyBool("D")]
        [SettingPropertyGroup("Group Order/D", GroupOrder = 5)]
        public bool SubGroup_D { get; set; } = true;

        [SettingPropertyBool("E")]
        [SettingPropertyGroup("Group Order/E", GroupOrder = 4)]
        public bool SubGroup_E { get; set; } = true;
    }
}