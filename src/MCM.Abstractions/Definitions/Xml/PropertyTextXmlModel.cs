using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
    [XmlType("PropertyText")]
    public class PropertyTextXmlModel : PropertyBaseXmlModel, IPropertyDefinitionText
    {
        [XmlElement("Value")]
        public string Value { get; set; }
    }
}