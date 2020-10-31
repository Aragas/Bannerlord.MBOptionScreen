using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MCM.Abstractions.Settings.Definitions.Xml
{
    [Serializable]
    [XmlType("PropertyGroups")]
    public class PropertyGroupXmlModel : IPropertyGroupDefinition
    {
        [XmlAttribute("DisplayName")]
        public string GroupName { get; set; } = default!;

        [XmlAttribute("Order")]
        public int GroupOrder { get; set; } = default!;

        [XmlArray("Properties")]
        [XmlArrayItem("PropertyBool", typeof(PropertyBoolXmlModel))]
        [XmlArrayItem("PropertyDropdown", typeof(PropertyDropdownXmlModel))]
        [XmlArrayItem("PropertyFloatingInteger", typeof(PropertyFloatingIntegerXmlModel))]
        [XmlArrayItem("PropertyInteger", typeof(PropertyIntegerXmlModel))]
        [XmlArrayItem("PropertyText", typeof(PropertyTextXmlModel))]
        public List<PropertyBaseXmlModel> Properties { get; set; } = default!;

        //[XmlArray("SettingsPropertyGroups")]
        //public List<PropertyGroupXmlModel> Groups { get; set; } = default!;
    }
}