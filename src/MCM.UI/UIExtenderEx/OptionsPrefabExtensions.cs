using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

using System.Collections.Generic;
using System.Xml;

namespace MCM.UI.UIExtenderEx
{
    [PrefabExtension("Options", "descendant::ListPanel[@Id='TabToggleList']/Children/OptionsTabToggle[5]")]
    public sealed class OptionsPrefabExtension1 : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Prepend;

        private readonly XmlDocument _xmlDocument = new();

        public OptionsPrefabExtension1()
        {
            _xmlDocument.LoadXml("<OptionsTabToggle DataSource=\"{ModOptions}\" PositionYOffset=\"2\" Parameter.ButtonBrush=\"Header.Tab.Center\" Parameter.TabName=\"ModOptionsPage\" />");
        }

        [PrefabExtensionXmlNode()]
        public XmlNode GetPrefabExtension() => _xmlDocument;
    }

    [PrefabExtension("Options", "descendant::TabControl[@Id='TabControl']/Children/*[5]")]
    internal sealed class OptionsPrefabExtension2 : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Prepend;

        private readonly XmlDocument _xmlDocument = new();

        public OptionsPrefabExtension2()
        {
            if (Bannerlord.ButterLib.Common.Helpers.ApplicationVersionUtils.GameVersion() is { } gameVersion)
            {
                if (gameVersion.Major <= 1 && gameVersion.Minor <= 5 && gameVersion.Revision <= 3)
                    _xmlDocument.LoadXml("<ModOptionsPageView_v1 Id=\"ModOptionsPage\" DataSource=\"{ModOptions}\" />");
                else
                    _xmlDocument.LoadXml("<ModOptionsPageView_v2 Id=\"ModOptionsPage\" DataSource=\"{ModOptions}\" />");
            }
        }

        [PrefabExtensionXmlDocument]
        public XmlDocument GetPrefabExtension() => _xmlDocument;
    }

    [PrefabExtension("Options", "descendant::Widget[@Id='DescriptionsRightPanel']")]
    public sealed class OptionsPrefabExtension3 : PrefabExtensionSetAttributePatch
    {
        public override List<Attribute> Attributes => new()
        {
            new Attribute( "SuggestedWidth", "@DescriptionWidth" )
        };
    }
}