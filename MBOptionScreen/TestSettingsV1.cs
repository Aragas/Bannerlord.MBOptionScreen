using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v1;
using MBOptionScreen.Data;
using MBOptionScreen.Settings;

namespace MBOptionScreen
{
    internal sealed class TestSettingsV1 : AttributeSettings<TestSettingsV1>
    {
        public override string Id { get; set; } = "Testing_v1";
        public override string ModName => "Testing v1 API";
        public override string ModuleFolderName => "Testing";


        [SettingProperty("Enable Crash Error Reporting", requireRestart: true, hintText: "When enabled, shows a message box showing the cause of a crash.")]
        [SettingPropertyGroup("Debugging")]
        public bool DebugMode { get; set; } = true;

        [SettingProperty("Test Property 1", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group", IsMainToggle = true)]
        public bool TestProperty1 { get; set; } = false;

        [SettingProperty("Test Property 5", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group")]
        public bool TestProperty5 { get; set; } = false;

        [SettingProperty("Test Property 2", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2", IsMainToggle = true)]
        public bool TestProperty2 { get; set; } = false;

        [SettingProperty("Test Property 4", 0f, 0.5f, 0f, 100f, requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2")]
        public float TestProperty4 { get; set; } = 0.2f;

        [SettingProperty("Test Property 3", 0, 10, requireRestart: true, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public int TestProperty3 { get; set; } = 2;

        [SettingProperty("Test Property 6", requireRestart: true, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public string TestProperty6 { get; set; } = "";

        [SettingProperty("Test Property 7", "", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public DefaultDropdown<string> TestProperty7 { get; set; } = new DefaultDropdown<string>(new string[]
        {
            "Test1",
            "Test2",
            "Test3"
        }, 0);

        [SettingProperty("Test Property 8", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public DefaultDropdown<CustomObject> TestProperty8 { get; set; } = new DefaultDropdown<CustomObject>(new CustomObject[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3")
        }, 2);

        [SettingProperty("Test Property 9", requireRestart: true, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Restart")]
        public bool TestProperty9 { get; set; } = false;

        [SettingProperty("Test Property 10", requireRestart: true, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Restart")]
        public bool TestProperty10 { get; set; } = false;

        public class CustomObject
        {
            private readonly string _value;

            public CustomObject(string value)
            {
                _value = value;
            }

            public override string ToString() => _value;
        }
    }
}