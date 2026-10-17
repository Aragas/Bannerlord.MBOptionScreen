using System.Collections.Generic;
using System.Xml;

namespace MCM.Abstractions.Xml
{
    /// <summary>
    /// Settings declared in XML instead of code. The block can be the root of its own file or a child of
    /// <c>&lt;Module&gt;</c> in a mod's <c>SubModule.xml</c>, where MCM discovers it without the mod referencing MCM.
    /// The file is also the store: every property carries its <c>Default</c>, and the current value is written
    /// back as a <c>Value</c> attribute by MCM or by the mod itself (see <see cref="SettingsXmlWriter"/>).
    /// <code>
    /// &lt;Settings&gt;
    ///   &lt;Group Name="{=id}General"&gt;
    ///     &lt;Bool Id="Enabled" DisplayName="{=id}Enabled" HintText="{=id}..." Default="true" RequireRestart="false" /&gt;
    ///   &lt;/Group&gt;
    ///   &lt;Preset Id="off" DisplayName="Off"&gt;&lt;Property Id="Enabled" Value="false" /&gt;&lt;/Preset&gt;
    /// &lt;/Settings&gt;
    /// </code>
    /// Properties placed directly under <c>&lt;Settings&gt;</c> go to the default (<c>Misc</c>) group.
    /// In a <c>SubModule.xml</c>, <c>Id</c> defaults to the module id and <c>DisplayName</c> to the module name.
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class SettingsXmlModel
    {
        private sealed class FlattenedGroup : IPropertyGroupDefinition
        {
            public string GroupName { get; }
            public int GroupOrder { get; }

            public FlattenedGroup(string groupName, int groupOrder)
            {
                GroupName = groupName;
                GroupOrder = groupOrder;
            }
        }

        public const string ElementName = "Settings";
        public const string ModuleElementName = "Module";

        public string Id { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        /// <summary>Not read from the XML. The values are stored in the declaring file, not in a settings folder.</summary>
        public string FolderName { get; } = string.Empty;

        /// <summary>Not read from the XML. The values are stored in the declaring file, not in a settings folder.</summary>
        public string SubFolder => string.Empty;

        public int UIVersion { get; set; } = 1;

        /// <summary>The first character is used, <c>/</c> when empty.</summary>
        public string SubGroupDelimiter { get; set; } = "/";

        /// <summary>Not read from the XML. The values are stored in the declaring file, not in a settings format.</summary>
        public string FormatType { get; set; } = string.Empty;

        internal char SubGroupDelimiterChar => string.IsNullOrEmpty(SubGroupDelimiter) ? '/' : SubGroupDelimiter[0];

        /// <summary>Properties outside any group. They are shown in the default group.</summary>
        public List<PropertyBaseXmlModel> Properties { get; set; } = [];

        public List<PropertyGroupXmlModel> Groups { get; set; } = [];

        public List<PresetXmlModel> Presets { get; set; } = [];

        /// <summary>
        /// Reads a <c>&lt;Settings&gt;</c> element. Returns null when no <c>Id</c> can be determined.
        /// </summary>
        /// <param name="node">The <c>&lt;Settings&gt;</c> element.</param>
        /// <param name="moduleId">The id of the declaring module, used when <c>Id</c> is absent.</param>
        /// <param name="moduleName">The name of the declaring module, used when <c>DisplayName</c> is absent.</param>
        public static SettingsXmlModel? FromXml(XmlNode node, string moduleId = "", string moduleName = "")
        {
            var id = XmlModelUtils.GetString(node, nameof(Id), moduleId);
            if (string.IsNullOrEmpty(id)) return null;

            var model = new SettingsXmlModel
            {
                Id = id,
                DisplayName = XmlModelUtils.GetString(node, nameof(DisplayName), string.IsNullOrEmpty(moduleName) ? id : moduleName),
                UIVersion = XmlModelUtils.GetInt(node, nameof(UIVersion), 1),
                SubGroupDelimiter = XmlModelUtils.GetString(node, nameof(SubGroupDelimiter), "/"),
            };

            foreach (var child in XmlModelUtils.Elements(node))
            {
                switch (child.Name)
                {
                    case PropertyGroupXmlModel.ElementName:
                        if (PropertyGroupXmlModel.FromXml(child) is { } group)
                            model.Groups.Add(group);
                        break;
                    case PresetXmlModel.ElementName:
                        if (PresetXmlModel.FromXml(child) is { } preset)
                            model.Presets.Add(preset);
                        break;
                    default:
                        if (PropertyBaseXmlModel.FromXml(child) is { } property)
                            model.Properties.Add(property);
                        break;
                }
            }

            return model;
        }

        /// <summary>
        /// Reads every <c>&lt;Settings&gt;</c> block of a document: the root itself when the document is a
        /// standalone settings file (<c>Id</c> required), or the children of the <c>&lt;Module&gt;</c> root of a
        /// <c>SubModule.xml</c>, where <c>Id</c> defaults to the module id and <c>DisplayName</c> to the module name.
        /// Blocks resolving to an id already seen are skipped.
        /// </summary>
        public static IEnumerable<SettingsXmlModel> FromDocument(XmlDocument document)
        {
            var seen = new HashSet<string>();
            foreach (var (node, moduleId, moduleName) in EnumerateDeclarations(document))
            {
                if (FromXml(node, moduleId, moduleName) is { } model && seen.Add(model.Id))
                    yield return model;
            }
        }

        /// <summary>
        /// Every <c>&lt;Settings&gt;</c> element of a document with the module id and name it can take its defaults from.
        /// </summary>
        public static IEnumerable<(XmlElement Node, string ModuleId, string ModuleName)> EnumerateDeclarations(XmlDocument document)
        {
            if (document.DocumentElement is not { } root)
                yield break;

            if (root.Name == ElementName)
            {
                yield return (root, string.Empty, string.Empty);
                yield break;
            }

            if (root.Name != ModuleElementName)
                yield break;

            var moduleId = root.SelectSingleNode("Id") is { } idNode ? XmlModelUtils.GetString(idNode, "value", string.Empty) : string.Empty;
            var moduleName = root.SelectSingleNode("Name") is { } nameNode ? XmlModelUtils.GetString(nameNode, "value", string.Empty) : string.Empty;
            foreach (var child in XmlModelUtils.Elements(root, ElementName))
                yield return ((XmlElement) child, moduleId, moduleName);
        }

        /// <summary>
        /// Every property with the group it belongs to. Ungrouped properties belong to the default group and a
        /// nested group is flattened to <c>Parent/Nested</c> with the sub group delimiter, the way MCM names sub groups.
        /// </summary>
        public IEnumerable<(IPropertyGroupDefinition Group, PropertyBaseXmlModel Property)> GetGroupedProperties()
        {
            foreach (var property in Properties)
                yield return (SettingsPropertyGroupDefinition.DefaultGroup, property);

            var stack = new Stack<(PropertyGroupXmlModel Group, string Name)>();
            for (var i = Groups.Count - 1; i >= 0; i--)
                stack.Push((Groups[i], Groups[i].GroupName));

            while (stack.Count > 0)
            {
                var (group, name) = stack.Pop();
                var definition = new FlattenedGroup(name, group.GroupOrder);
                foreach (var property in group.Properties)
                    yield return (definition, property);
                for (var i = group.Groups.Count - 1; i >= 0; i--)
                    stack.Push((group.Groups[i], $"{name}{SubGroupDelimiterChar}{group.Groups[i].GroupName}"));
            }
        }

        /// <summary>
        /// Every property of every group, including nested groups, plus the ungrouped ones.
        /// </summary>
        public IEnumerable<PropertyBaseXmlModel> GetAllProperties()
        {
            foreach (var (_, property) in GetGroupedProperties())
                yield return property;
        }
    }
}
