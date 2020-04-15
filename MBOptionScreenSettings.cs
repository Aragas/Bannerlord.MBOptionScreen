using MBOptionScreen.Settings;

using System.Xml.Serialization;

namespace MBOptionScreen
{
    internal class MBOptionScreenSettings : AttributeSettings<MBOptionScreenSettings>
    {
        public override string ModName => $"OptionScreen v1";
        public override string ModuleFolderName => "OptionScreen";

        [XmlElement]
        public override string Id { get; set; } = "OptionScreen_v1";
    }
}