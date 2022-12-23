using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Xml
{
    [Serializable]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    abstract class PropertyBaseXmlModel : IPropertyDefinitionBase, IPropertyDefinitionWithId
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = default!;

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; } = default!;

        [XmlAttribute("Order")]
        public int Order { get; set; }

        [XmlAttribute("HintText")]
        public string HintText { get; set; } = string.Empty;

        [XmlAttribute("RequireRestart")]
        public bool RequireRestart { get; set; } = true;
    }
}