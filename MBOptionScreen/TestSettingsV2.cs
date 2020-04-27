using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v2;
using MBOptionScreen.Data;

namespace MBOptionScreen
{
    internal sealed class TestSettingsV2 : TestSettingsBase<TestSettingsV2>
    {
        public override string Id { get; set; } = "Testing_v2";
        public override string ModName => "Testing v2 API";


        [SettingPropertyBool("Property Bool Default False", RequireRestart = false)]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolDefaultFalse { get; set; } = false;
        [SettingPropertyBool("Property Bool Default True", RequireRestart = false)]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolDefaultTrue { get; set; } = true;
        [SettingPropertyBool("Property Bool Require Restart")]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolRequireRestart { get; set; }
        [SettingPropertyBool("Property Bool With Hint", RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolWithHint { get; set; }


        [SettingPropertyInteger("Property Int Default 0", 0, 100, RequireRestart = false)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntDefault0 { get; set; } = 0;
        [SettingPropertyInteger("Property Int Default 1", 0, 100, RequireRestart = false)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntDefault1 { get; set; } = 1;
        [SettingPropertyInteger("Property Int Require Restart", 0, 100)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntRequireRestart { get; set; }
        [SettingPropertyInteger("Property Int -10 to 10", -10, 10, RequireRestart = false)]
        [SettingPropertyGroup("Int")]
        public int PropertyInt1to10 { get; set; }
        [SettingPropertyInteger("Property Int With Hint", 0, 100, RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Int")]
        public int PropertyIntWithHint { get; set; }


        [SettingPropertyFloatingInteger("Property Float Default 0f", 0f, 100f, RequireRestart = false)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatDefault0f { get; set; } = 0f;
        [SettingPropertyFloatingInteger("Property Float Default 1f", 0f, 100f, RequireRestart = false)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatDefault1f { get; set; } = 1f;
        [SettingPropertyFloatingInteger("Property Float Require Restart", 0f, 100f, RequireRestart = false)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatRequireRestart { get; set; }
        [SettingPropertyFloatingInteger("Property Float -10 to 10", -10f, 10f)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloat1to10 { get; set; }
        [SettingPropertyFloatingInteger("Property Float With Hint", 0f, 100f, RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatWithHint { get; set; }


        [SettingPropertyText("Property Text Default Empty", RequireRestart = false)]
        [SettingPropertyGroup("Text")]
        public string PropertyTextDefaultEmpty { get; set; } = "";
        [SettingPropertyText("Property Text Default Text", RequireRestart = false)]
        [SettingPropertyGroup("Text")]
        public string PropertyTextDefaultText { get; set; } = "Text";
        [SettingPropertyText("Property Text Require Restart")]
        [SettingPropertyGroup("Text")]
        public string PropertyTextRequireRestart { get; set; }
        [SettingPropertyText("Property Text With Hint", RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Text")]
        public string PropertyTextWithHint { get; set; }


        [SettingPropertyDropdown("Property Dropdown SelectedIndex 0", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown")]
        public DefaultDropdown<string> PropertyDropdownSelectedIndex0 { get; } = new DefaultDropdown<string>(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 0);
        [SettingPropertyDropdown("Property Dropdown SelectedIndex 1", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown")]
        public DefaultDropdown<string> PropertyDropdownSelectedIndex1 { get; } = new DefaultDropdown<string>(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 1);
        [SettingPropertyDropdown("Property Dropdown SelectedIndex 2", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown")]
        public DefaultDropdown<string> PropertyDropdownSelectedIndex2 { get; } = new DefaultDropdown<string>(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 2);
        [SettingPropertyDropdown("Property Dropdown Require Restart")]
        [SettingPropertyGroup("Dropdown")]
        public DefaultDropdown<string> PropertyDropdownRequireRestart { get; } = new DefaultDropdown<string>(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 0);
        [SettingPropertyDropdown("Property Dropdown With Hint", RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Dropdown")]
        public DefaultDropdown<string> PropertyDropdownWithHint { get; } = new DefaultDropdown<string>(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 0);


        [SettingPropertyDropdown("Property Dropdown Custom SelectedIndex 0", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Custom")]
        public DefaultDropdown<CustomObject> PropertyDropdownCustomSelectedIndex0 { get; } = new DefaultDropdown<CustomObject>(new CustomObject[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3"),
        }, 0);
        [SettingPropertyDropdown("Property Dropdown Custom SelectedIndex 1", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Custom")]
        public DefaultDropdown<CustomObject> PropertyDropdownCustomSelectedIndex1 { get; } = new DefaultDropdown<CustomObject>(new CustomObject[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3"),
        }, 1);
        [SettingPropertyDropdown("Property Dropdown Custom SelectedIndex 2", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Custom")]
        public DefaultDropdown<CustomObject> PropertyDropdownCustomSelectedIndex2 { get; } = new DefaultDropdown<CustomObject>(new CustomObject[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3"),
        }, 2);
        [SettingPropertyDropdown("Property Dropdown Custom Require Restart")]
        [SettingPropertyGroup("Dropdown Custom")]
        public DefaultDropdown<CustomObject> PropertyDropdownCustomRequireRestart { get; } = new DefaultDropdown<CustomObject>(new CustomObject[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3"),
        }, 0);
        [SettingPropertyDropdown("Property Dropdown Custom With Hint", RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Dropdown Custom")]
        public DefaultDropdown<CustomObject> PropertyDropdownCustomWithHint { get; } = new DefaultDropdown<CustomObject>(new CustomObject[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3"),
        }, 0);
        public class CustomObject
        {
            private readonly string _value;
            public CustomObject(string value) => _value = value;
            public override string ToString() => _value;
        }
    }
}