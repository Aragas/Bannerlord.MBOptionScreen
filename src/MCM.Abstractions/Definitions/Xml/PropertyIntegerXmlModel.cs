using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
    [XmlType("PropertyInteger")]
    public class PropertyIntegerXmlModel : PropertyBaseXmlModel, IPropertyDefinitionWithMinMax
    {
        [XmlAttribute("MinValue")]
        public decimal MinValue { get; set; }

        [XmlAttribute("MaxValue")]
        public decimal MaxValue { get; set; }
        
        [XmlElement("Value")]
        public decimal Value { get; set; }
    }
}