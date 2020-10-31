using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Settings.Definitions.Xml
{
    [Serializable]
    [XmlType("PropertyText")]
    public class PropertyTextXmlModel : PropertyBaseXmlModel, IPropertyDefinitionText { }
}