using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
    [XmlType("PropertyGroups")]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class PropertyGroupXmlModel : IPropertyGroupDefinition
    {
        [XmlAttribute("DisplayName")]
        public string GroupName { get; set; } = default!;

        [XmlAttribute("Order")]
        public int GroupOrder { get; set; }

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