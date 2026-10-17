using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// <c>&lt;Integer Id="" DisplayName="" MinValue="0" MaxValue="10" Default="5" ValueFormat="0" /&gt;</c>
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class PropertyIntegerXmlModel : PropertyBaseXmlModel, IPropertyDefinitionWithMinMax, IPropertyDefinitionWithFormat
    {
        public const string ElementName = "Integer";

        /// <inheritdoc/>
        public decimal MinValue { get; set; }

        /// <inheritdoc/>
        public decimal MaxValue { get; set; }

        /// <inheritdoc/>
        public string ValueFormat { get; set; } = "0";

        /// <summary>The declared default (<c>Default</c> attribute), <c>MinValue</c> when absent.</summary>
        public decimal DefaultValue { get; set; }

        /// <summary>The current value (<c>Value</c> attribute), the default when absent.</summary>
        public decimal Value { get; set; }

        /// <inheritdoc/>
        protected override bool Read(XmlNode node)
        {
            if (!ReadBase(node)) return false;
            MinValue = XmlModelUtils.GetInt(node, nameof(MinValue), 0);
            MaxValue = XmlModelUtils.GetInt(node, nameof(MaxValue), 0);
            ValueFormat = XmlModelUtils.GetString(node, nameof(ValueFormat), "0");
            DefaultValue = XmlModelUtils.GetInt(node, XmlModelUtils.DefaultAttribute, (int) MinValue);
            Value = XmlModelUtils.GetInt(node, XmlModelUtils.ValueAttribute, (int) DefaultValue);
            return true;
        }
    }
}
