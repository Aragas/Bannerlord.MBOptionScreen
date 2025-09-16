using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;

namespace MCMv5.Tests;

internal sealed class TestSettingsOrder : BaseTestGlobalSettings<TestSettingsOrder>
{
    public override string Id => "Testing_Order_v5";
    public override string DisplayName => "MCMv5 Testing Order";


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


    [SettingPropertyBool("Group Order Toggle", IsToggle = true)] // TODO: cover in docs
    [SettingPropertyGroup("Group Order")]
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


    [SettingPropertyGroupMetadata]
    [SettingPropertyGroup("GroupA", GroupOrder = 11)]
    public object GroupAMetadata { get; set; } = new();
    [SettingPropertyBool("A")]
    [SettingPropertyGroup("GroupA/X")]
    public bool GroupANestedProperty { get; set; } = true;

    [SettingPropertyGroupMetadata]
    [SettingPropertyGroup("GroupB", GroupOrder = 13)]
    public object GroupBMetadata { get; set; } = new();
    [SettingPropertyBool("B")]
    [SettingPropertyGroup("GroupB/X")]
    public bool GroupBNestedProperty { get; set; } = true;

    [SettingPropertyGroupMetadata]
    [SettingPropertyGroup("GroupC", GroupOrder = 12)]
    public object GroupCMetadata { get; set; } = new();
    [SettingPropertyBool("C")]
    [SettingPropertyGroup("GroupC/X")]
    public bool GroupCNestedProperty { get; set; } = true;

    [SettingPropertyGroupMetadata]
    [SettingPropertyGroup("GroupD", GroupOrder = 15)]
    public object GroupDMetadata { get; set; } = new();
    [SettingPropertyBool("D")]
    [SettingPropertyGroup("GroupD/X")]
    public bool GroupDNestedProperty { get; set; } = true;

    [SettingPropertyGroupMetadata]
    [SettingPropertyGroup("GroupE", GroupOrder = 14)]
    public object GroupEMetadata { get; set; } = new();
    [SettingPropertyBool("E")]
    [SettingPropertyGroup("GroupE/X")]
    public bool GroupENestedProperty { get; set; } = true;
}