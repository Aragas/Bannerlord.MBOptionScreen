using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
    [XmlType("PropertyDropdown")]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class PropertyDropdownXmlModel : PropertyBaseXmlModel, IPropertyDefinitionDropdown
    {
        [XmlAttribute("SelectedIndex")]
        public int SelectedIndex { get; set; }

        [XmlArray("Values")]
        public string[] Values { get; set; } = [];
    }
}