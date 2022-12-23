using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
    [XmlRoot("Settings")]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class SettingsXmlModel
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = default!;

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; } = default!;

        [XmlAttribute("FolderName")]
        public string FolderName { get; } = string.Empty;

        [XmlAttribute("SubFolder")]
        public string SubFolder => string.Empty;

        [XmlAttribute("UIVersion")]
        public int UIVersion { get; set; }

        [XmlAttribute("SubGroupDelimiter")]
        public string SubGroupDelimiter { get; set; } = default!;

        [XmlAttribute("FormatType")]
        public string FormatType { get; set; } = default!;

        [XmlArray("Properties")]
        [XmlArrayItem("PropertyBool", typeof(PropertyBoolXmlModel))]
        [XmlArrayItem("PropertyDropdown", typeof(PropertyDropdownXmlModel))]
        [XmlArrayItem("PropertyFloatingInteger", typeof(PropertyFloatingIntegerXmlModel))]
        [XmlArrayItem("PropertyInteger", typeof(PropertyIntegerXmlModel))]
        [XmlArrayItem("PropertyText", typeof(PropertyTextXmlModel))]
        public List<PropertyBaseXmlModel> Properties { get; set; } = default!;

        [XmlArray("PropertyGroups")]
        [XmlArrayItem("PropertyGroup", typeof(PropertyGroupXmlModel))]
        public List<PropertyGroupXmlModel> Groups { get; set; } = default!;
    }
}