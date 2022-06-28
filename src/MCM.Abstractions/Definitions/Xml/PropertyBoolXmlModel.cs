using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
    [XmlType("PropertyBool")]
    public class PropertyBoolXmlModel : PropertyBaseXmlModel, IPropertyDefinitionBool, IPropertyDefinitionGroupToggle
    {
        [XmlAttribute("IsToggle")]
        public bool IsToggle { get; set; }
        
        [XmlElement("Value")]
        public bool Value { get; set; }
    }
}