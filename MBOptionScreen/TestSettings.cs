using MBOptionScreen.Attributes;
using MBOptionScreen.Settings;

namespace MBOptionScreen
{
    internal class TestSettings : AttributeSettings<TestSettings>
    {
        public override string Id { get; set; } = "Testing";
        public override string ModName => "Testing";
        public override string ModuleFolderName => "Testing";


        [SettingProperty("Enable Crash Error Reporting", requireRestart: true, hintText: "When enabled, shows a message box showing the cause of a crash.")]
        [SettingPropertyGroup("Debugging")]
        public bool DebugMode { get; set; } = true;

        [SettingProperty("Test Property 1", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group", true)]
        public bool TestProperty1 { get; set; } = false;

        [SettingProperty("Test Property 5", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group")]
        public bool TestProperty5 { get; set; } = false;

        [SettingProperty("Test Property 2", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2", true)]
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
        public Dropdown<string> TestProperty7 { get; set; } = new Dropdown<string>(new string[] 
        {
            "Test1",
            "Test2",
            "Test3"
        }, 0);

        [SettingProperty("Test Property 8", requireRestart: false, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public Dropdown<string> TestProperty8 { get; set; } = new Dropdown<string>(new string[]
        {
            "Test1",
            "Test2",
            "Test3"
        }, 2);

        [SettingProperty("Test Property 9", requireRestart: true, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Restart")]
        public bool TestProperty9 { get; set; } = false;

        [SettingProperty("Test Property 10", requireRestart: true, hintText: "")]
        [SettingPropertyGroup("Debugging/Test Group/Restart")]
        public bool TestProperty10 { get; set; } = false;

        // v2
        [SettingPropertyFloatingInteger("Test Property 11", 0f, 1f, requireRestart: false, hintText: "value between 0 and 1 formatted to a percentage", valueFormat: "#0%")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 4")]
        public float TestProperty11 { get; set; } = 0.5f;

        /*[SettingPropertyInteger("Test Property 12", 0, 5000, requireRestart: false, hintText: "", valueFormat: "0 Denars")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 4")]
        public int TestProperty12 { get; set; } = 1250;

        [SettingPropertyInteger("Test Property 13", 0, 500, requireRestart: false, hintText: "", valueFormat: "Per 0 Units")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 4")]
        public int TestProperty13 { get; set; } = 100;*/

    }
}
