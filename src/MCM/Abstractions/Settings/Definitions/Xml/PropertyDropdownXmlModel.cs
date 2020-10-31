using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Settings.Definitions.Xml
{
    [Serializable]
    [XmlType("PropertyDropdown")]
    public class PropertyDropdownXmlModel : PropertyBaseXmlModel, IPropertyDefinitionDropdown
    {
        [XmlAttribute("SelectedIndex")]
        public int SelectedIndex { get; set; } = default!;
    }
}