using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v1;
using MBOptionScreen.Settings;

namespace MBOv2.Tests
{
    internal sealed class TestSettings : AttributeSettings<TestSettings>
    {
        public override string Id { get; set; } = "Testing_v1_v2";
        public override string ModName => "MBOv2 Testing API";
        public override string ModuleFolderName => "MBOv2.Tests";


        [SettingProperty("Property Bool Default False")]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolDefaultFalse { get; set; } = false;
        [SettingProperty("Property Bool Default True")]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolDefaultTrue { get; set; } = true;
        [SettingProperty("Property Bool Require Restart")]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolRequireRestart { get; set; }
        [SettingProperty("Property Bool With Hint", hintText: "Hint Text")]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolWithHint { get; set; }


        [SettingProperty("Property Int Default 0", 0, 100f)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntDefault0 { get; set; } = 0;
        [SettingProperty("Property Int Default 1", 0f, 100f)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntDefault1 { get; set; } = 1;
        [SettingProperty("Property Int Require Restart", 0f, 100f)]
        [SettingPropertyGroup("Int")]
        public int PropertyIntRequireRestart { get; set; }
        [SettingProperty("Property Int -10 to 10", -10f, 10f)]
        [SettingPropertyGroup("Int")]
        public int PropertyInt1to10 { get; set; }
        [SettingProperty("Property Int With Hint", 0f, 100f, hintText: "Hint Text")]
        [SettingPropertyGroup("Int")]
        public int PropertyIntWithHint { get; set; }


        [SettingProperty("Property Float Default 0f", 0f, 100f)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatDefault0f { get; set; } = 0f;
        [SettingProperty("Property Float Default 1f", 0f, 100f)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatDefault1f { get; set; } = 1f;
        [SettingProperty("Property Float Require Restart", 0f, 100f)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatRequireRestart { get; set; }
        [SettingProperty("Property Float -10 to 10", -10f, 10f)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloat1to10 { get; set; }
        [SettingProperty("Property Float With Hint", 0f, 100f, hintText: "Hint Text")]
        [SettingPropertyGroup("Float")]
        public float PropertyFloatWithHint { get; set; }


        [SettingProperty("Property Text Default Empty")]
        [SettingPropertyGroup("Text")]
        public string PropertyTextDefaultEmpty { get; set; } = string.Empty;
        [SettingProperty("Property Text Default Text")]
        [SettingPropertyGroup("Text")]
        public string PropertyTextDefaultText { get; set; } = "Text";
        [SettingProperty("Property Text Require Restart")]
        [SettingPropertyGroup("Text")]
        public string PropertyTextRequireRestart { get; set; } = string.Empty;
        [SettingProperty("Property Text With Hint", hintText: "Hint Text")]
        [SettingPropertyGroup("Text")]
        public string PropertyTextWithHint { get; set; } = string.Empty;
    }
}