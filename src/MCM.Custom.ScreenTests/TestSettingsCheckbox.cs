using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Data;
using MCM.Abstractions.Ref;

namespace MCM.Custom.ScreenTests
{
    internal sealed class TestSettingsCheckbox : BaseTestGlobalSettings<TestSettingsCheckbox>
    {
        public override string Id => "Testing_Checkbox_v1";
        public override string DisplayName => "Testing Checkbox";

        public bool Prop1 { get; set; }
        public bool Prop2 { get; set; }
        public bool Prop3 { get; set; }
        public bool Prop4 { get; set; }

        public TestSettingsCheckbox()
        {
            PropertyDropdownSelectedIndex0 = new CheckboxDropdown(new IRef[]
            {
                new PropertyRef(GetType().GetProperty(nameof(Prop1)), this),
                new PropertyRef(GetType().GetProperty(nameof(Prop2)), this),
                new PropertyRef(GetType().GetProperty(nameof(Prop3)), this),
                new PropertyRef(GetType().GetProperty(nameof(Prop4)), this)
            }, 0);
        }


        [SettingPropertyDropdown("Property Dropdown SelectedIndex 0", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown")]
        public CheckboxDropdown PropertyDropdownSelectedIndex0 { get; set; }
    }
}