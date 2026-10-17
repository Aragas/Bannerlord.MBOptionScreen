using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// Writes current values back into the file a <c>&lt;Settings&gt;</c> block was declared in, as <c>Value</c>
    /// attributes on the property elements. Only those attributes change: the file is edited as text, not
    /// re-serialized, so comments, quotes, attribute layout, line endings, encoding and byte order mark stay as they are.
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    static class SettingsXmlWriter
    {
        private static readonly HashSet<string> PropertyElements = new()
        {
            PropertyBoolXmlModel.ElementName,
            PropertyIntegerXmlModel.ElementName,
            PropertyFloatingIntegerXmlModel.ElementName,
            PropertyTextXmlModel.ElementName,
            PropertyDropdownXmlModel.ElementName,
        };

        private static readonly Regex DeclaredEncoding = new("^<\\?xml[^>]*encoding\\s*=\\s*[\"']([^\"']+)[\"']", RegexOptions.CultureInvariant);

        /// <summary>
        /// Loads the file with its whitespace preserved.
        /// </summary>
        public static XmlDocument Load(string filePath)
        {
            var document = new XmlDocument { PreserveWhitespace = true };
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            document.Load(stream);
            return document;
        }

        /// <summary>
        /// Sets the <c>Value</c> attribute of every listed property of the block in the file. Values are the text a
        /// <c>Default</c> attribute takes; a dropdown takes the option index. Ids without a matching property element are
        /// ignored. Returns false when the block is not in the file. The file is written in place, so links a mod
        /// manager deployed stay intact, and only when a value changed.
        /// </summary>
        public static bool SetValues(string filePath, string settingsId, IEnumerable<KeyValuePair<string, string>> values)
        {
            var bytes = File.ReadAllBytes(filePath);
            var (encoding, preambleLength) = DetectEncoding(bytes);
            var xml = encoding.GetString(bytes, preambleLength, bytes.Length - preambleLength);

            if (!TrySetValues(xml, settingsId, values, out var updated))
                return false;
            if (updated == xml)
                return true;

            var content = encoding.GetBytes(updated);
            var result = new byte[preambleLength + content.Length];
            Array.Copy(bytes, result, preambleLength);
            Array.Copy(content, 0, result, preambleLength, content.Length);
            File.WriteAllBytes(filePath, result);
            return true;
        }

        /// <summary>
        /// <see cref="SetValues(string, string, IEnumerable{KeyValuePair{string, string}})"/> on the text of a document.
        /// </summary>
        public static bool TrySetValues(string xml, string settingsId, IEnumerable<KeyValuePair<string, string>> values, out string result)
        {
            result = xml;

            var document = new XmlDocument { PreserveWhitespace = true };
            document.LoadXml(xml);
            if (FindDeclaration(document, settingsId) is not { } declaration)
                return false;

            // The DOM finds the elements, a reader over the same text says where they start. Both see the elements in
            // the same order, so the element path, the index among its sibling elements at every level, joins them
            var targets = new Dictionary<string, string>();
            var elements = GetPropertyElements(declaration);
            foreach (var kv in values)
            {
                if (elements.TryGetValue(kv.Key, out var element))
                    targets[GetPath(element)] = kv.Value;
            }
            if (targets.Count == 0)
                return true;

            var edits = new List<(int Start, int Length, string Text)>();
            foreach (var (tagStart, value) in FindStartTags(xml, targets))
            {
                if (GetValueEdit(xml, tagStart, value) is { } edit)
                    edits.Add(edit);
            }

            var builder = new StringBuilder(xml);
            foreach (var (start, length, text) in edits.OrderByDescending(x => x.Start))
                builder.Remove(start, length).Insert(start, text);
            result = builder.ToString();
            return true;
        }

        /// <summary>
        /// Finds the <c>&lt;Settings&gt;</c> block with the id, resolving a missing <c>Id</c> attribute to the module id
        /// the way <see cref="SettingsXmlModel.FromDocument"/> does.
        /// </summary>
        public static XmlElement? FindDeclaration(XmlDocument document, string settingsId)
        {
            foreach (var (node, moduleId, _) in SettingsXmlModel.EnumerateDeclarations(document))
            {
                var id = XmlModelUtils.GetString(node, "Id", moduleId);
                if (id == settingsId)
                    return node;
            }
            return null;
        }

        /// <summary>
        /// The property elements of a declaration by property id, nested groups included.
        /// </summary>
        public static Dictionary<string, XmlElement> GetPropertyElements(XmlElement declaration)
        {
            var result = new Dictionary<string, XmlElement>();
            Collect(declaration, result);
            return result;
        }

        private static void Collect(XmlElement parent, Dictionary<string, XmlElement> result)
        {
            foreach (var child in XmlModelUtils.Elements(parent))
            {
                if (child is not XmlElement element) continue;

                if (element.Name == PropertyGroupXmlModel.ElementName)
                {
                    Collect(element, result);
                    continue;
                }

                if (!PropertyElements.Contains(element.Name)) continue;
                var id = element.GetAttribute("Id");
                if (string.IsNullOrEmpty(id) || result.ContainsKey(id)) continue;
                result[id] = element;
            }
        }

        private static string GetPath(XmlNode element)
        {
            var indices = new List<int>();
            for (var node = element; node.ParentNode is { } parent && node is XmlElement; node = parent)
            {
                var index = 0;
                for (var sibling = node.PreviousSibling; sibling is not null; sibling = sibling.PreviousSibling)
                {
                    if (sibling.NodeType == XmlNodeType.Element) index++;
                }
                indices.Add(index);
            }
            indices.Reverse();
            return string.Join("/", indices);
        }

        /// <summary>The offset of the <c>&lt;</c> of every target element.</summary>
        private static IEnumerable<(int TagStart, string Value)> FindStartTags(string xml, Dictionary<string, string> targets)
        {
            var lineStarts = GetLineStarts(xml);
            var path = new List<int>();
            var nextIndex = new List<int> { 0 };

            using var reader = new XmlTextReader(new StringReader(xml)) { DtdProcessing = DtdProcessing.Ignore, WhitespaceHandling = WhitespaceHandling.None };
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element) continue;

                var depth = reader.Depth;
                if (path.Count > depth) path.RemoveRange(depth, path.Count - depth);
                if (nextIndex.Count > depth + 1) nextIndex.RemoveRange(depth + 1, nextIndex.Count - depth - 1);
                while (nextIndex.Count < depth + 1) nextIndex.Add(0);

                path.Add(nextIndex[depth]++);
                nextIndex.Add(0);

                if (targets.TryGetValue(string.Join("/", path), out var value))
                {
                    // LinePosition is the first character of the name, one based
                    yield return (lineStarts[reader.LineNumber - 1] + reader.LinePosition - 2, value);
                }
            }
        }

        /// <summary>Line starts the way the reader counts lines: <c>\r\n</c>, <c>\r</c> and <c>\n</c> each end one.</summary>
        private static List<int> GetLineStarts(string xml)
        {
            var result = new List<int> { 0 };
            for (var i = 0; i < xml.Length; i++)
            {
                if (xml[i] == '\r')
                {
                    if (i + 1 < xml.Length && xml[i + 1] == '\n') i++;
                    result.Add(i + 1);
                }
                else if (xml[i] == '\n')
                {
                    result.Add(i + 1);
                }
            }
            return result;
        }

        /// <summary>
        /// Replaces the text of an existing <c>Value</c> attribute in the start tag, or adds one after the last attribute.
        /// Null when the attribute already holds the text.
        /// </summary>
        private static (int Start, int Length, string Text)? GetValueEdit(string xml, int tagStart, string value)
        {
            var i = tagStart + 1;
            while (i < xml.Length && !IsTagSeparator(xml[i])) i++;
            var lastAttributeEnd = i;

            while (i < xml.Length)
            {
                while (i < xml.Length && char.IsWhiteSpace(xml[i])) i++;
                if (i >= xml.Length || xml[i] == '/' || xml[i] == '>') break;

                var nameStart = i;
                while (i < xml.Length && xml[i] != '=' && !char.IsWhiteSpace(xml[i])) i++;
                var name = xml.Substring(nameStart, i - nameStart);
                while (i < xml.Length && (char.IsWhiteSpace(xml[i]) || xml[i] == '=')) i++;
                if (i >= xml.Length) break;

                var quote = xml[i];
                var valueStart = i + 1;
                var valueEnd = xml.IndexOf(quote, valueStart);
                if (valueEnd < 0) break;

                if (name == XmlModelUtils.ValueAttribute)
                {
                    var escaped = Escape(value, quote);
                    return xml.Substring(valueStart, valueEnd - valueStart) == escaped ? null : (valueStart, valueEnd - valueStart, escaped);
                }

                i = valueEnd + 1;
                lastAttributeEnd = i;
            }

            return (lastAttributeEnd, 0, $" {XmlModelUtils.ValueAttribute}=\"{Escape(value, '"')}\"");
        }

        private static bool IsTagSeparator(char c) => char.IsWhiteSpace(c) || c == '/' || c == '>';

        private static string Escape(string value, char quote)
        {
            var builder = new StringBuilder(value.Length);
            foreach (var c in value)
            {
                switch (c)
                {
                    case '&': builder.Append("&amp;"); break;
                    case '<': builder.Append("&lt;"); break;
                    case '"' when quote == '"': builder.Append("&quot;"); break;
                    case '\'' when quote == '\'': builder.Append("&apos;"); break;
                    // Attribute value normalization would turn them into spaces
                    case '\t': builder.Append("&#9;"); break;
                    case '\n': builder.Append("&#10;"); break;
                    case '\r': builder.Append("&#13;"); break;
                    default: builder.Append(c); break;
                }
            }
            return builder.ToString();
        }

        /// <summary>The encoding from the byte order mark, else from the declaration, else UTF-8.</summary>
        private static (Encoding Encoding, int PreambleLength) DetectEncoding(byte[] bytes)
        {
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return (new UTF8Encoding(true), 3);
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                return (new UnicodeEncoding(false, true), 2);
            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
                return (new UnicodeEncoding(true, true), 2);

            var head = Encoding.ASCII.GetString(bytes, 0, Math.Min(bytes.Length, 200));
            if (DeclaredEncoding.Match(head) is { Success: true } match && !match.Groups[1].Value.Equals("utf-8", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    return (Encoding.GetEncoding(match.Groups[1].Value), 0);
                }
                catch (ArgumentException) { }
            }
            return (new UTF8Encoding(false), 0);
        }
    }
}
