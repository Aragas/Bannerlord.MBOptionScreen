using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// <c>&lt;Bool Id="" DisplayName="" Default="false" IsToggle="false" /&gt;</c>
    /// </summary>
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
        public const string ElementName = "Bool";

        /// <inheritdoc/>
        public bool IsToggle { get; set; }

        /// <summary>The declared default (<c>Default</c> attribute).</summary>
        public bool DefaultValue { get; set; }

        /// <summary>The current value (<c>Value</c> attribute), the default when absent.</summary>
        public bool Value { get; set; }

        /// <inheritdoc/>
        protected override bool Read(XmlNode node)
        {
            if (!ReadBase(node)) return false;
            IsToggle = XmlModelUtils.GetBool(node, nameof(IsToggle), false);
            DefaultValue = XmlModelUtils.GetBool(node, XmlModelUtils.DefaultAttribute, false);
            Value = XmlModelUtils.GetBool(node, XmlModelUtils.ValueAttribute, DefaultValue);
            return true;
        }
    }
}
