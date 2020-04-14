using ModLib.Attributes;
using System.Xml.Serialization;

namespace ModLib
{
    public class Settings : SettingsBase
    {
        public override string ModName => "ModLib";
        public override string ModuleFolderName => ModLibSubModule.ModuleFolderName;
        private const string instanceID = "ModLibSettings";
        private static Settings _instance = null;
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FileDatabase.Get<Settings>(instanceID);
                    if (_instance == null)
                    {
                        _instance = new Settings();
                        SettingsDatabase.SaveSettings(_instance);
                    }
                }
                return _instance;
            }
        }

        [XmlElement]
        public override string ID { get; set; } = instanceID;
        [XmlElement]
        [SettingProperty("Enable Crash Error Reporting", "When enabled, shows a message box showing the cause of a crash.")]
        [SettingPropertyGroup("Debugging")]
        public bool DebugMode { get; set; } = true;

        [XmlElement]
        [SettingProperty("Test Property 1", "")]
        [SettingPropertyGroup("Debugging/Test Group")]
        public bool TestProperty1 { get; set; } = false;

        [XmlElement]
        [SettingProperty("Test Property 2", "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2", true)]
        public bool TestProperty2 { get; set; } = false;

        [XmlElement]
        [SettingProperty("Test Property 4", 0f, 0.5f, "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 2")]
        public float TestProperty4 { get; set; } = 0.2f;

        [XmlElement]
        [SettingProperty("Test Property 3", 0, 10, "")]
        [SettingPropertyGroup("Debugging/Test Group/Test Group 3")]
        public int TestProperty3 { get; set; } = 2;

    }
}
