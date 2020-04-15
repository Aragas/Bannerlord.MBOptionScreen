using MBOptionScreen.Settings;

using System.Xml.Serialization;

namespace MBOptionScreen
{
    internal class MBOptionScreenSettings : SettingsBase<MBOptionScreenSettings>
    {
        public override string ModName => $"OptionScreen v1";
        public override string ModuleFolderName => "OptionScreen";

        [XmlElement]
        public override string ID { get; set; } = "OptionScreen_v1";
    }
}