using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// Common attributes of every property declared in a <c>&lt;Settings&gt;</c> XML block:
    /// <c>Id</c>, <c>DisplayName</c>, <c>Order</c>, <c>HintText</c>, <c>RequireRestart</c>.
    /// </summary>
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
        /// <inheritdoc/>
        public string Id { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string DisplayName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public int Order { get; set; } = -1;

        /// <inheritdoc/>
        public string HintText { get; set; } = string.Empty;

        /// <inheritdoc/>
        public bool RequireRestart { get; set; } = true;

        /// <summary>
        /// Creates the model matching the element name (<c>Bool</c>, <c>Integer</c>, <c>FloatingInteger</c>, <c>Text</c>, <c>Dropdown</c>).
        /// Returns null for unknown elements and for elements without an <c>Id</c>.
        /// </summary>
        public static PropertyBaseXmlModel? FromXml(XmlNode node)
        {
            PropertyBaseXmlModel? model = node.Name switch
            {
                PropertyBoolXmlModel.ElementName => new PropertyBoolXmlModel(),
                PropertyIntegerXmlModel.ElementName => new PropertyIntegerXmlModel(),
                PropertyFloatingIntegerXmlModel.ElementName => new PropertyFloatingIntegerXmlModel(),
                PropertyTextXmlModel.ElementName => new PropertyTextXmlModel(),
                PropertyDropdownXmlModel.ElementName => new PropertyDropdownXmlModel(),
                _ => null,
            };
            return model is not null && model.Read(node) ? model : null;
        }

        /// <summary>
        /// Reads the attributes shared by all property elements. Returns false when the element has no <c>Id</c>.
        /// </summary>
        protected bool ReadBase(XmlNode node)
        {
            Id = XmlModelUtils.GetString(node, nameof(Id), string.Empty);
            if (string.IsNullOrEmpty(Id)) return false;

            DisplayName = XmlModelUtils.GetString(node, nameof(DisplayName), Id);
            Order = XmlModelUtils.GetInt(node, nameof(Order), -1);
            HintText = XmlModelUtils.GetString(node, nameof(HintText), string.Empty);
            RequireRestart = XmlModelUtils.GetBool(node, nameof(RequireRestart), true);
            return true;
        }

        /// <summary>
        /// Fills the model from the element. Returns false when the element is not valid.
        /// </summary>
        protected virtual bool Read(XmlNode node) => ReadBase(node);
    }
}
