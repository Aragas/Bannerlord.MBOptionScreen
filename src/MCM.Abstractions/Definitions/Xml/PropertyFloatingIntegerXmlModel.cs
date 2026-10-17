using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// <c>&lt;FloatingInteger Id="" DisplayName="" MinValue="0" MaxValue="1" Default="0.5" ValueFormat="0.00" /&gt;</c>
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class PropertyFloatingIntegerXmlModel : PropertyBaseXmlModel, IPropertyDefinitionWithMinMax, IPropertyDefinitionWithFormat
    {
        public const string ElementName = "FloatingInteger";

        /// <inheritdoc/>
        public decimal MinValue { get; set; }

        /// <inheritdoc/>
        public decimal MaxValue { get; set; }

        /// <inheritdoc/>
        public string ValueFormat { get; set; } = "0.00";

        /// <summary>The declared default (<c>Default</c> attribute), <c>MinValue</c> when absent.</summary>
        public decimal DefaultValue { get; set; }

        /// <summary>The current value (<c>Value</c> attribute), the default when absent.</summary>
        public decimal Value { get; set; }

        /// <inheritdoc/>
        protected override bool Read(XmlNode node)
        {
            if (!ReadBase(node)) return false;
            MinValue = XmlModelUtils.GetDecimal(node, nameof(MinValue), 0);
            MaxValue = XmlModelUtils.GetDecimal(node, nameof(MaxValue), 0);
            ValueFormat = XmlModelUtils.GetString(node, nameof(ValueFormat), "0.00");
            DefaultValue = XmlModelUtils.GetDecimal(node, XmlModelUtils.DefaultAttribute, MinValue);
            Value = XmlModelUtils.GetDecimal(node, XmlModelUtils.ValueAttribute, DefaultValue);
            return true;
        }
    }
}
