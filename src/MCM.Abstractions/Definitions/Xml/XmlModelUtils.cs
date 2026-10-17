using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// Attribute readers for the settings XML models. Numbers are read with the invariant culture and
    /// a missing or malformed attribute yields the fallback.
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal static class XmlModelUtils
    {
        public const string DefaultAttribute = "Default";
        public const string ValueAttribute = "Value";

        public static string GetString(XmlNode node, string attribute, string fallback) =>
            node.Attributes?[attribute]?.Value ?? fallback;

        public static bool GetBool(XmlNode node, string attribute, bool fallback) =>
            bool.TryParse(node.Attributes?[attribute]?.Value, out var value) ? value : fallback;

        public static int GetInt(XmlNode node, string attribute, int fallback) =>
            int.TryParse(node.Attributes?[attribute]?.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : fallback;

        public static float GetFloat(XmlNode node, string attribute, float fallback) =>
            float.TryParse(node.Attributes?[attribute]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : fallback;

        public static decimal GetDecimal(XmlNode node, string attribute, decimal fallback) =>
            decimal.TryParse(node.Attributes?[attribute]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : fallback;

        public static IEnumerable<XmlNode> Elements(XmlNode node, string? name = null)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;
                if (name is not null && child.Name != name) continue;
                yield return child;
            }
        }
    }
}
