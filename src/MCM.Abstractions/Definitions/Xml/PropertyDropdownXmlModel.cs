using System.Collections.Generic;
using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// <code>
    /// &lt;Dropdown Id="" DisplayName="" Default="0"&gt;
    ///   &lt;Option Value="First" /&gt;
    ///   &lt;Option Value="Second" /&gt;
    /// &lt;/Dropdown&gt;
    /// </code>
    /// <c>Default</c> and <c>Value</c> are option indices.
    /// </summary>
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
        public const string ElementName = "Dropdown";
        public const string OptionElementName = "Option";

        /// <summary>The options, in display order.</summary>
        public string[] Values { get; set; } = [];

        /// <summary>The declared default option index (<c>Default</c> attribute).</summary>
        public int DefaultSelectedIndex { get; set; }

        /// <summary>The current option index (<c>Value</c> attribute), the default when absent.</summary>
        public int SelectedIndex { get; set; }

        /// <inheritdoc/>
        protected override bool Read(XmlNode node)
        {
            if (!ReadBase(node)) return false;

            var values = new List<string>();
            foreach (var child in XmlModelUtils.Elements(node, OptionElementName))
                values.Add(XmlModelUtils.GetString(child, XmlModelUtils.ValueAttribute, child.InnerText ?? string.Empty));
            if (values.Count == 0) return false;
            Values = values.ToArray();

            DefaultSelectedIndex = Clamp(XmlModelUtils.GetInt(node, XmlModelUtils.DefaultAttribute, 0));
            SelectedIndex = Clamp(XmlModelUtils.GetInt(node, XmlModelUtils.ValueAttribute, DefaultSelectedIndex));
            return true;
        }

        private int Clamp(int index) => index >= 0 && index < Values.Length ? index : 0;
    }
}
