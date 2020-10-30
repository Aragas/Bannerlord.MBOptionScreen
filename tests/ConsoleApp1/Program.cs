using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    [XmlRoot("Settings"), Serializable]
    public class SettingsXmlModel
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = default!;

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; } = default!;

        [XmlAttribute("UIVersion")]
        public int UIVersion { get; set; } = default!;

        [XmlAttribute("SubGroupDelimiter")]
        public string SubGroupDelimiter { get; set; } = default!;

        [XmlArray("SettingsProperties")]
        public List<SettingPropertyBaseXmlModel> Properties { get; set; } = default!;

        [XmlArray("SettingsPropertyGroups")]
        public List<SettingPropertyGroupXmlModel> Groups { get; set; } = default!;
    }

    [XmlType("SettingPropertyGroup"), Serializable]
    public class SettingPropertyGroupXmlModel
    {
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; } = default!;

        [XmlArray("SettingsProperties")]
        public List<SettingPropertyBaseXmlModel> Properties { get; set; } = default!;

        [XmlArray("SettingsPropertyGroups")]
        public List<SettingPropertyGroupXmlModel> Groups { get; set; } = default!;
    }

    [Serializable]
    [XmlInclude(typeof(SettingPropertyBoolXmlModel))]
    [XmlInclude(typeof(SettingPropertyIntegerXmlModel))]
    [XmlInclude(typeof(SettingPropertyFloatingIntegerXmlModel))]
    public abstract class SettingPropertyBaseXmlModel
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = default!;

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; } = default!;

        [XmlAttribute("Order")]
        public int Order { get; set; } = default!;

        [XmlAttribute("HintText")]
        public string HintText { get; set; } = "";

        [XmlAttribute("RequireRestart")]
        public bool RequireRestart { get; set; } = true;
    }

    [XmlType("SettingPropertyBool"), Serializable]
    public class SettingPropertyBoolXmlModel : SettingPropertyBaseXmlModel
    {
        [XmlAttribute("IsToggle")]
        public bool IsToggle { get; set; } = false;
    }

    [XmlType("SettingPropertyInteger"), Serializable]
    public class SettingPropertyIntegerXmlModel : SettingPropertyBaseXmlModel
    {
        [XmlAttribute("MinValue")]
        public decimal MinValue { get; set; } = default!;

        [XmlAttribute("MaxValue")]
        public decimal MaxValue { get; set; } = default!;
    }

    [XmlType("SettingPropertyFloatingInteger"), Serializable]
    public class SettingPropertyFloatingIntegerXmlModel : SettingPropertyBaseXmlModel
    {
        [XmlAttribute("MinValue")]
        public decimal MinValue { get; set; } = default!;

        [XmlAttribute("MaxValue")]
        public decimal MaxValue { get; set; } = default!;
    }

    class Program
    {
        static void Main(string[] args)
        {
            using var fs = new FileStream("ExternalSettingsTest.xml", FileMode.OpenOrCreate);
            var formatter = new XmlSerializer(typeof(SettingsXmlModel));
            var obj = (SettingsXmlModel) formatter.Deserialize(fs);

            var reader = new XmlDocument();
            reader.Load("ExternalSettingsTest.xml");

            Console.WriteLine("Hello World!");
        }
    }
}
