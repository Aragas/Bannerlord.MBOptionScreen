using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs;

using System.Xml;

namespace MCM.UI.UIExtenderEx
{
    [PrefabExtension("Options", "descendant::OptionsScreenWidget[@Id='Options']/Children/Standard.TopPanel/Children/ListPanel/Children")]
    public sealed class OptionsPrefabExtension1 : PrefabExtensionInsertPatch
    {
        public override string Id => "Options1";
        public override int Position => 3;
        private XmlDocument XmlDocument { get; } = new();

        public OptionsPrefabExtension1()
        {
            XmlDocument.LoadXml("<OptionsTabToggle DataSource=\"{ModOptions}\" PositionYOffset=\"2\" Parameter.ButtonBrush=\"Header.Tab.Center\" Parameter.TabName=\"ModOptionsPage\" />");
        }

        public override XmlDocument GetPrefabExtension() => XmlDocument;
    }

    [PrefabExtension("Options", "descendant::TabControl[@Id='TabControl']/Children")]
    internal sealed class OptionsPrefabExtension2 : PrefabExtensionInsertPatch
    {
        public override string Id => "Options2";
        public override int Position => 3;
        private XmlDocument XmlDocument { get; } = new();

        public OptionsPrefabExtension2()
        {
            if (ApplicationVersionUtils.GameVersion() is { } gameVersion)
            {
                if (gameVersion.Major <= 1 && gameVersion.Minor <= 5 && gameVersion.Revision <= 3)
                    XmlDocument.LoadXml("<ModOptionsPageView_v1 Id=\"ModOptionsPage\" DataSource=\"{ModOptions}\" />");
                else
                    XmlDocument.LoadXml("<ModOptionsPageView_v2 Id=\"ModOptionsPage\" DataSource=\"{ModOptions}\" />");
            }
        }

        public override XmlDocument GetPrefabExtension() => XmlDocument;
    }

    [PrefabExtension("Options", "descendant::Widget[@Id='DescriptionsRightPanel']")]
    public sealed class OptionsPrefabExtension3 : PrefabExtensionSetAttributePatch
    {
        public override string Id => "Options3";
        public override string Attribute => "SuggestedWidth";
        public override string Value => "@DescriptionWidth";
    }
}