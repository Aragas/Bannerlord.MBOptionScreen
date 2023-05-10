using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

using System.Collections.Generic;
using System.Xml;

namespace MCM.UI.UIExtenderEx
{
    [PrefabExtension("Options", "descendant::ListPanel[@Id='TabToggleList']/Children/OptionsTabToggle[5]", "Options")]
    internal sealed class OptionsPrefabExtension1 : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Prepend;

        private readonly XmlDocument _xmlDocument = new();

        public OptionsPrefabExtension1()
        {
            _xmlDocument.LoadXml("<OptionsTabToggle DataSource=\"{ModOptions}\" PositionYOffset=\"2\" Parameter.ButtonBrush=\"Header.Tab.Center\" Parameter.TabName=\"ModOptionsPage\" />");
        }

        [PrefabExtensionXmlNode]
        public XmlNode GetPrefabExtension() => _xmlDocument;
    }

    [PrefabExtension("Options", "descendant::TabControl[@Id='TabControl']/Children/*[5]", "Options")]
    internal sealed class OptionsPrefabExtension2 : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Prepend;

        private readonly XmlDocument _xmlDocument = new();

        public OptionsPrefabExtension2()
        {
            _xmlDocument.LoadXml("<ModOptionsPageView Id=\"ModOptionsPage\" DataSource=\"{ModOptions}\" />");
        }

        [PrefabExtensionXmlDocument]
        public XmlDocument GetPrefabExtension() => _xmlDocument;
    }

    [PrefabExtension("Options", "descendant::Widget[@Id='DescriptionsRightPanel']", "Options")]
    internal sealed class OptionsPrefabExtension3 : PrefabExtensionSetAttributePatch
    {
        public override List<Attribute> Attributes => new()
        {
            new Attribute("SuggestedWidth", "@DescriptionWidth")
        };
    }
}