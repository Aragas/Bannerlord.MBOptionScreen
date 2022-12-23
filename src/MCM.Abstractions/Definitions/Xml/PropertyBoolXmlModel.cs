using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
    [XmlType("PropertyBool")]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class PropertyBoolXmlModel : PropertyBaseXmlModel, IPropertyDefinitionBool, IPropertyDefinitionGroupToggle
    {
        [XmlAttribute("IsToggle")]
        public bool IsToggle { get; set; }

        [XmlElement("Value")]
        public bool Value { get; set; }
    }
}