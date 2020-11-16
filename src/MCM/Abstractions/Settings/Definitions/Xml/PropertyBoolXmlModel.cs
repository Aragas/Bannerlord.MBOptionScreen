using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Settings.Definitions.Xml
{
    [Serializable]
    [XmlType("PropertyBool")]
    public class PropertyBoolXmlModel : PropertyBaseXmlModel, IPropertyDefinitionBool, IPropertyDefinitionGroupToggle
    {
        [XmlAttribute("IsToggle")]
        public bool IsToggle { get; set; }
    }
}