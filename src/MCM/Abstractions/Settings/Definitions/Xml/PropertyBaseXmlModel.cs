using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Settings.Definitions.Xml
{
    [Serializable]
    public abstract class PropertyBaseXmlModel : IPropertyDefinitionBase, IPropertyDefinitionWithId
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = default!;

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; } = default!;

        [XmlAttribute("Order")]
        public int Order { get; set; } = default!;

        [XmlAttribute("HintText")]
        public string HintText { get; set; } = string.Empty;

        [XmlAttribute("RequireRestart")]
        public bool RequireRestart { get; set; } = true;
    }
}