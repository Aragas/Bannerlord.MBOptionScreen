﻿using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v1;
using MCM.Common;

namespace MCMv5.Tests
{
    internal sealed class TestSettingsV1 : BaseTestGlobalSettings<TestSettingsV1>
    {
        public override string Id => "Testing_v1_v4";
        public override string DisplayName => "MCMv4 Testing v1 API";


        [SettingProperty("Property Bool Default False", RequireRestart = false)]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolDefaultFalse { get; set; } = false;
        [SettingProperty("Property Bool Default True", RequireRestart = false)]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolDefaultTrue { get; set; } = true;
        [SettingProperty("Property Bool Require Restart")]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolRequireRestart { get; set; }
        [SettingProperty("Property Bool With Hint", RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolWithHint { get; set; }


        [SettingProperty("Property Int Default 0", 0, 100f, RequireRestart = false)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntDefault0 { get; set; } = 0;
        [SettingProperty("Property Int Default 1", 0f, 100f, RequireRestart = false)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntDefault1 { get; set; } = 1;
        [SettingProperty("Property Int Require Restart", 0f, 100f)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntRequireRestart { get; set; }
        [SettingProperty("Property Int -10 to 10", -10f, 10f, RequireRestart = false)]
        [SettingPropertyGroup("Int")]
        public int PropertyInt1to10 { get; set; }
        [SettingProperty("Property Int With Hint", 0f, 100f, RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Int")]
        public int PropertyIntWithHint { get; set; }


        [SettingProperty("Property Float Default 0f", 0f, 100f, RequireRestart = false)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatDefault0f { get; set; } = 0f;
        [SettingProperty("Property Float Default 1f", 0f, 100f, RequireRestart = false)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatDefault1f { get; set; } = 1f;
        [SettingProperty("Property Float Require Restart", 0f, 100f)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatRequireRestart { get; set; }
        [SettingProperty("Property Float -10 to 10", -10f, 10f, RequireRestart = false)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloat1to10 { get; set; }
        [SettingProperty("Property Float With Hint", 0f, 100f, RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatWithHint { get; set; }


        [SettingProperty("Property Text Default Empty", RequireRestart = false)]
        [SettingPropertyGroup("Text")]
        public string PropertyTextDefaultEmpty { get; set; } = string.Empty;
        [SettingProperty("Property Text Default Text", RequireRestart = false)]
        [SettingPropertyGroup("Text")]
        public string PropertyTextDefaultText { get; set; } = "Text";
        [SettingProperty("Property Text Require Restart")]
        [SettingPropertyGroup("Text")]
        public string PropertyTextRequireRestart { get; set; } = string.Empty;
        [SettingProperty("Property Text With Hint", RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Text")]
        public string PropertyTextWithHint { get; set; } = string.Empty;


        [SettingProperty("Property Dropdown SelectedIndex 0", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown")]
        public Dropdown<string> PropertyDropdownSelectedIndex0 { get; } = new(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 0);
        [SettingProperty("Property Dropdown SelectedIndex 1", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown")]
        public Dropdown<string> PropertyDropdownSelectedIndex1 { get; } = new(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 1);
        [SettingProperty("Property Dropdown SelectedIndex 2", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown")]
        public Dropdown<string> PropertyDropdownSelectedIndex2 { get; } = new(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 2);
        [SettingProperty("Property Dropdown Require Restart")]
        [SettingPropertyGroup("Dropdown")]
        public Dropdown<string> PropertyDropdownRequireRestart { get; } = new(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 0);
        [SettingProperty("Property Dropdown With Hint", RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Dropdown")]
        public Dropdown<string> PropertyDropdownWithHint { get; } = new(new string[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 0);


        [SettingProperty("Property Dropdown Custom SelectedIndex 0", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Custom")]
        public Dropdown<CustomObject> PropertyDropdownCustomSelectedIndex0 { get; } = new(new CustomObject[]
        {
            new("Test1"),
            new("Test2"),
            new("Test3"),
        }, 0);
        [SettingProperty("Property Dropdown Custom SelectedIndex 1", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Custom")]
        public Dropdown<CustomObject> PropertyDropdownCustomSelectedIndex1 { get; } = new(new CustomObject[]
        {
            new("Test1"),
            new("Test2"),
            new("Test3"),
        }, 1);
        [SettingProperty("Property Dropdown Custom SelectedIndex 2", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Custom")]
        public Dropdown<CustomObject> PropertyDropdownCustomSelectedIndex2 { get; } = new(new CustomObject[]
        {
            new("Test1"),
            new("Test2"),
            new("Test3"),
        }, 2);
        [SettingProperty("Property Dropdown Custom Require Restart")]
        [SettingPropertyGroup("Dropdown Custom")]
        public Dropdown<CustomObject> PropertyDropdownCustomRequireRestart { get; } = new(new CustomObject[]
        {
            new("Test1"),
            new("Test2"),
            new("Test3"),
        }, 0);
        [SettingProperty("Property Dropdown Custom With Hint", RequireRestart = false, HintText = "Hint Text")]
        [SettingPropertyGroup("Dropdown Custom")]
        public Dropdown<CustomObject> PropertyDropdownCustomWithHint { get; } = new(new CustomObject[]
        {
            new("Test1"),
            new("Test2"),
            new("Test3"),
        }, 0);
        public class CustomObject
        {
            private readonly string _value;
            public CustomObject(string value) => _value = value;
            public override string ToString() => _value;
        }
    }
}