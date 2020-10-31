using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Settings.Definitions.Xml
{
    [Serializable]
    [XmlType("PropertyInteger")]
    public class PropertyIntegerXmlModel : PropertyBaseXmlModel, IPropertyDefinitionWithMinMax
    {
        [XmlAttribute("MinValue")]
        public decimal MinValue { get; set; } = default!;

        [XmlAttribute("MaxValue")]
        public decimal MaxValue { get; set; } = default!;
    }
}