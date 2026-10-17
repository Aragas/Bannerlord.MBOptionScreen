using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// <c>&lt;Text Id="" DisplayName="" Default="" /&gt;</c>
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class PropertyTextXmlModel : PropertyBaseXmlModel, IPropertyDefinitionText
    {
        public const string ElementName = "Text";

        /// <summary>The declared default (<c>Default</c> attribute).</summary>
        public string DefaultValue { get; set; } = string.Empty;

        /// <summary>The current value (<c>Value</c> attribute), the default when absent.</summary>
        public string Value { get; set; } = string.Empty;

        /// <inheritdoc/>
        protected override bool Read(XmlNode node)
        {
            if (!ReadBase(node)) return false;
            DefaultValue = XmlModelUtils.GetString(node, XmlModelUtils.DefaultAttribute, string.Empty);
            Value = XmlModelUtils.GetString(node, XmlModelUtils.ValueAttribute, DefaultValue);
            return true;
        }
    }
}
