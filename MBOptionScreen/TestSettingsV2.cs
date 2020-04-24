using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v2;
using MBOptionScreen.Data;
using MBOptionScreen.Settings;

namespace MBOptionScreen
{
    internal sealed class TestSettingsV2 : AttributeSettings<TestSettingsV2>
    {
        public override string Id { get; set; } = "Testing_v2";
        public override string ModName => "Testing v2 API";
        public override string ModuleFolderName => "Testing";


        [SettingPropertyBool("Enable Crash Error Reporting", RequireRestart = true, HintText = "When enabled, shows a message box showing the cause of a crash.")]
        [SettingPropertyGroup("Debugging")]
        public bool DebugMode { get; set; } = true;

        [SettingPropertyBool("Test Property 1")]
        [SettingPropertyGroup("Debugging/Test Group", IsMainToggle = true)]
        public bool TestProperty1 { get; set; } = true;

        [SettingPropertyBool("Test Property 5")]
        [SettingPropertyGroup("Debugging/Test Group")]
        public bool TestProperty5 { get; set; } = false;

        [SettingPropertyBool("Test Property 2")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2", IsMainToggle = true)]
        public bool TestProperty2 { get; set; } = true;

        [SettingPropertyFloatingInteger("Test Property 4", 0f, 0.5f)]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2")]
        public float TestProperty4 { get; set; } = 0.2f;

        [SettingPropertyInteger("Test Property 3", 0, 10, RequireRestart = true)]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public int TestProperty3 { get; set; } = 2;

        [SettingPropertyText("Test Property 6", RequireRestart = true)]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public string TestProperty6 { get; set; } = "";

        [SettingPropertyDropdown("Test Property 7", 0)]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public DefaultDropdown<string> TestProperty7 { get; set; } = new DefaultDropdown<string>(new string[]
        {
            "Test1",
            "Test2",
            "Test3"
        }, 0);

        [SettingPropertyDropdown("Test Property 8", 0)]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public DefaultDropdown<CustomObject> TestProperty8 { get; set; } = new DefaultDropdown<CustomObject>(new CustomObject[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3")
        }, 2);

        [SettingPropertyBool("Test Property 9", RequireRestart = true)]
        [SettingPropertyGroup("Debugging/Test Group/Restart", Order = 1)]
        public bool TestProperty9 { get; set; } = false;

        [SettingPropertyBool("Test Property 10", RequireRestart = true)]
        [SettingPropertyGroup("Debugging/Test Group/Restart", Order = 2)]
        public bool TestProperty10 { get; set; } = false;

        [SettingPropertyFloatingInteger("Test Property 11", 0f, 1f, "#0%", HintText = "value between 0 and 1 formatted to a percentage")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 4", Order = 3)]
        public float TestProperty11 { get; set; } = 0.5f;

        [SettingPropertyInteger("Test Property 12", 0, 5000, "0 Denars")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 4", Order = 4)]
        public int TestProperty12 { get; set; } = 1250;

        [SettingPropertyInteger("Test Property 13", 0, 500, "Per 0 Units")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 4", Order = 5)]
        public int TestProperty13 { get; set; } = 100;

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