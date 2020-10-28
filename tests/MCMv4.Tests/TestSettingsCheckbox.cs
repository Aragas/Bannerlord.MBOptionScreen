using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Ref;

namespace MCMv4.Tests
{
    internal sealed class TestSettingsCheckbox : BaseTestGlobalSettings<TestSettingsCheckbox>
    {
        private bool _prop1;

        public class SelectableClass
        {
            public bool IsSelected { get; set; }
            public string Name { get; set; }
            public string HintText { get; set; }

            public IRef Ref { get; set; }

            public SelectableClass(IRef @ref, string name, string hintText = "")
            {
                Ref = @ref;
                Name = name;
                HintText = hintText;
            }

            public override string ToString() => Name;
        }

        public override string Id => "Testing_Checkbox_v1";
        public override string DisplayName => "Testing Checkbox";

        public bool Prop1
        {
            get => _prop1;
            set => _prop1 = value;
        }

        public bool Prop2 { get; set; }
        public bool Prop3 { get; set; }
        public bool Prop4 { get; set; }

        public TestSettingsCheckbox()
        {
            PropertyDropdownSelectedIndex0 = new CheckboxDropdownMCM<SelectableClass>(new[]
            {
                new SelectableClass(
                    new PropertyRef(GetType().GetProperty(nameof(Prop1))!, this),
                    "Property 1"),
                new SelectableClass(
                    new PropertyRef(GetType().GetProperty(nameof(Prop2))!, this),
                    "Property 2"),
                new SelectableClass(
                    new PropertyRef(GetType().GetProperty(nameof(Prop3))!, this),
                    "Property 3"),
                new SelectableClass(
                    new PropertyRef(GetType().GetProperty(nameof(Prop4))!, this),
                    "Property 4"),
            }, 0);
        }

        [SettingPropertyDropdown("Property Dropdown SelectedIndex 0", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown")]
        public CheckboxDropdownMCM<SelectableClass> PropertyDropdownSelectedIndex0 { get; set; }
    }
}