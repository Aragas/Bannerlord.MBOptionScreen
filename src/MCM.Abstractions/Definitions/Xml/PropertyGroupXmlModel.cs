using System.Collections.Generic;
using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// <code>
    /// &lt;Group Name="{=id}Display Name" Order="0"&gt;
    ///   &lt;Bool ... /&gt;
    ///   &lt;Group Name="Nested"&gt; ... &lt;/Group&gt;
    /// &lt;/Group&gt;
    /// </code>
    /// A nested group becomes a sub group. Its full name is <c>Parent/Nested</c>, joined with the settings' sub group delimiter.
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class PropertyGroupXmlModel : IPropertyGroupDefinition
    {
        public const string ElementName = "Group";

        /// <inheritdoc/>
        public string GroupName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public int GroupOrder { get; set; } = -1;

        public List<PropertyBaseXmlModel> Properties { get; set; } = [];

        public List<PropertyGroupXmlModel> Groups { get; set; } = [];

        /// <summary>
        /// Returns null when the element has no <c>Name</c>.
        /// </summary>
        public static PropertyGroupXmlModel? FromXml(XmlNode node)
        {
            var name = XmlModelUtils.GetString(node, "Name", string.Empty);
            if (string.IsNullOrEmpty(name)) return null;

            var model = new PropertyGroupXmlModel
            {
                GroupName = name,
                GroupOrder = XmlModelUtils.GetInt(node, "Order", -1),
            };

            foreach (var child in XmlModelUtils.Elements(node))
            {
                if (child.Name == ElementName)
                {
                    if (FromXml(child) is { } group)
                        model.Groups.Add(group);
                }
                else if (PropertyBaseXmlModel.FromXml(child) is { } property)
                {
                    model.Properties.Add(property);
                }
            }

            return model;
        }
    }
}
