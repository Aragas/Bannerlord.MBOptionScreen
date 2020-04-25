using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v2;

namespace MBOptionScreen
{
    internal sealed class TestSettingsOrder : TestSettingsBase<TestSettingsOrder>
    {
        public override string Id { get; set; } = "Testing_Order_v1";
        public override string ModName => "Testing Order";


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
        [SettingPropertyGroup("A", Order = 1)]
        public bool Group_A { get; set; } = true;

        [SettingPropertyBool("B")]
        [SettingPropertyGroup("B", Order = 3)]
        public bool Group_B { get; set; } = true;

        [SettingPropertyBool("C")]
        [SettingPropertyGroup("C", Order = 2)]
        public bool Group_C { get; set; } = true;

        [SettingPropertyBool("D")]
        [SettingPropertyGroup("D", Order = 5)]
        public bool Group_D { get; set; } = true;

        [SettingPropertyBool("E")]
        [SettingPropertyGroup("E", Order = 4)]
        public bool Group_E { get; set; } = true;


        [SettingPropertyBool("Group Order Toggle")]
        [SettingPropertyGroup("Group Order", IsMainToggle = true)]
        public bool SubGroup_Toggle { get; set; } = true;

        [SettingPropertyBool("A")]
        [SettingPropertyGroup("Group Order/A", Order = 1)]
        public bool SubGroup_A { get; set; } = true;

        [SettingPropertyBool("B")]
        [SettingPropertyGroup("Group Order/B", Order = 3)]
        public bool SubGroup_B { get; set; } = true;

        [SettingPropertyBool("C")]
        [SettingPropertyGroup("Group Order/C", Order = 2)]
        public bool SubGroup_C { get; set; } = true;

        [SettingPropertyBool("D")]
        [SettingPropertyGroup("Group Order/D", Order = 5)]
        public bool SubGroup_D { get; set; } = true;

        [SettingPropertyBool("E")]
        [SettingPropertyGroup("Group Order/E", Order = 4)]
        public bool SubGroup_E { get; set; } = true;
    }
}