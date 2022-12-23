using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
    [XmlType("PropertyInteger")]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class PropertyIntegerXmlModel : PropertyBaseXmlModel, IPropertyDefinitionWithMinMax
    {
        [XmlAttribute("MinValue")]
        public decimal MinValue { get; set; }

        [XmlAttribute("MaxValue")]
        public decimal MaxValue { get; set; }

        [XmlElement("Value")]
        public decimal Value { get; set; }
    }
}