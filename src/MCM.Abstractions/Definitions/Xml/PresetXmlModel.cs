using System.Collections.Generic;
using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// <code>
    /// &lt;Preset Id="performance" DisplayName="{=id}Performance"&gt;
    ///   &lt;Property Id="SomeBool" Value="true" /&gt;
    /// &lt;/Preset&gt;
    /// </code>
    /// Values are written the way the property's <c>Default</c> attribute is written. A dropdown takes the option index.
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class PresetXmlModel
    {
        public const string ElementName = "Preset";
        public const string PropertyElementName = "Property";

        public string Id { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        /// <summary>Property id to raw value text.</summary>
        public Dictionary<string, string> Values { get; set; } = new();

        /// <summary>
        /// Returns null when the element has no <c>Id</c>.
        /// </summary>
        public static PresetXmlModel? FromXml(XmlNode node)
        {
            var id = XmlModelUtils.GetString(node, nameof(Id), string.Empty);
            if (string.IsNullOrEmpty(id)) return null;

            var model = new PresetXmlModel
            {
                Id = id,
                DisplayName = XmlModelUtils.GetString(node, nameof(DisplayName), id),
            };

            foreach (var child in XmlModelUtils.Elements(node, PropertyElementName))
            {
                var propertyId = XmlModelUtils.GetString(child, "Id", string.Empty);
                if (string.IsNullOrEmpty(propertyId)) continue;
                model.Values[propertyId] = XmlModelUtils.GetString(child, XmlModelUtils.ValueAttribute, string.Empty);
            }

            return model;
        }
    }
}
