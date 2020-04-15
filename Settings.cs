using ModLib.Attributes;
using System.Xml.Serialization;

namespace ModLib
{
    public class Settings : SettingsBase
    {
        public override string ModName => "ModLib";
        public override string ModuleFolderName => ModLibSubModule.ModuleFolderName;
        public const string SettingsInstanceID = "ModLibSettings";
        public static Settings Instance
        {
            get
            {
                return (Settings)SettingsDatabase.GetSettings(SettingsInstanceID);
            }
        }

        [XmlElement]
        public override string ID { get; set; } = SettingsInstanceID;
        [XmlElement]
        [SettingProperty("Enable Crash Error Reporting", "When enabled, shows a message box showing the cause of a crash.")]
        [SettingPropertyGroup("Debugging")]
        public bool DebugMode { get; set; } = true;

        [XmlElement]
        [SettingProperty("Test Property 1", "")]
        [SettingPropertyGroup("Debugging/Test Group", true)]
        public bool TestProperty1 { get; set; } = false;

        [XmlElement]
        [SettingProperty("Test Property 5", "")]
        [SettingPropertyGroup("Debugging/Test Group")]
        public bool TestProperty5 { get; set; } = false;

        [XmlElement]
        [SettingProperty("Test Property 2", "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2", true)]
        public bool TestProperty2 { get; set; } = false;

        [XmlElement]
        [SettingProperty("Test Property 4", 0f, 0.5f, 0f, 100f, "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2")]
        public float TestProperty4 { get; set; } = 0.2f;

        [XmlElement]
        [SettingProperty("Test Property 3", 0, 10, "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public int TestProperty3 { get; set; } = 2;

    }
}
